using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace KarAfarin.Infrastructure.Persistence.Extensions
{
    public static class DbContextDependencyExtensions
    {
        private static readonly ConcurrentDictionary<Type, List<IForeignKey>> _fkCache = new();

        private static readonly MethodInfo _anyAsyncMethod =
            typeof(EntityFrameworkQueryableExtensions)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(m => m.Name == "AnyAsync")
            .Where(m =>
            {
                var p = m.GetParameters();
                return p.Length == 3 &&
                       p[1].ParameterType.IsGenericType &&
                       p[1].ParameterType.GetGenericTypeDefinition() == typeof(Expression<>);
            })
            .Single();

        public static async Task<bool> HasDependenciesAsync<TEntity>(
            this DbContext context,
            object entityId,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            var entityType = context.Model.FindEntityType(typeof(TEntity))
                ?? throw new InvalidOperationException($"Entity type {typeof(TEntity).Name} not found.");

            var pk = entityType.FindPrimaryKey()
                ?? throw new InvalidOperationException("Primary key not found.");

            if (pk.Properties.Count != 1)
                throw new NotSupportedException("Composite primary keys not supported.");

            var referencingFks = _fkCache.GetOrAdd(typeof(TEntity),
                _ => entityType.GetReferencingForeignKeys().ToList());

            foreach (var fk in referencingFks)
            {
                if (fk.Properties.Count != 1)
                    throw new NotSupportedException("Composite foreign keys not supported.");

                var dependentClrType = fk.DeclaringEntityType.ClrType;

                var dependentSet = context.GetType()
                    .GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!
                    .MakeGenericMethod(dependentClrType)
                    .Invoke(context, null);

                var fkProperty = fk.Properties.First();
                var propertyInfo = dependentClrType.GetProperty(fkProperty.Name);
                if (propertyInfo == null) continue;

                var targetType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var convertedId = Convert.ChangeType(entityId, targetType);

                var parameter = Expression.Parameter(dependentClrType, "e");
                var left = Expression.Property(parameter, propertyInfo);
                var right = Expression.Constant(convertedId, propertyInfo.PropertyType);
                var body = Expression.Equal(left, right);

                var lambda = Expression.Lambda(body, parameter);

                var anyAsync = _anyAsyncMethod.MakeGenericMethod(dependentClrType);

                var task = (Task<bool>)anyAsync.Invoke(null, new object[]
                {
                dependentSet,
                lambda,
                cancellationToken
                });

                if (await task.ConfigureAwait(false))
                    return true;
            }

            return false;
        }
    }

}
