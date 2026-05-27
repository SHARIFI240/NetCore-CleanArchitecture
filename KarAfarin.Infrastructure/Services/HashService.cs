using KarAfarin.Application.Common.Interfaces.Services;
using System.Security.Cryptography;


namespace KarAfarin.Infrastructure.Services
{
    public class HashService : IHashService
    {

        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 10_000;

        public string Hash(string inp)
        {
            using var rng = RandomNumberGenerator.Create();

            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(
                        inp,
                        salt,
                        Iterations,
                        HashAlgorithmName.SHA256);

            var key = pbkdf2.GetBytes(KeySize);

            return Convert.ToBase64String(
                Combine(salt, key));
        }

        public bool Verify(string inp, string hash)
        {
            var hashBytes = Convert.FromBase64String(hash);

            var salt = hashBytes[..SaltSize];
            var key = hashBytes[SaltSize..];

            using var pbkdf2 = new Rfc2898DeriveBytes(
                inp,
                salt,
                Iterations,
                HashAlgorithmName.SHA256);

            var keyToCheck = pbkdf2.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(
                key,
                keyToCheck);
        }

        private static byte[] Combine(byte[] salt, byte[] key)
        {
            var result = new byte[salt.Length + key.Length];
            Buffer.BlockCopy(salt, 0, result, 0, salt.Length);
            Buffer.BlockCopy(key, 0, result, salt.Length, key.Length);
            return result;
        }

    }
}
