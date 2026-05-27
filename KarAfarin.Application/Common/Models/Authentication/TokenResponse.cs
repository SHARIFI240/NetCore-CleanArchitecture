using System;
using System.Collections.Generic;
using System.Text;

namespace KarAfarin.Application.Common.Models.Authentication
{
    public class TokenResponse
    {
        /// <summary>
        /// توکن دسترسی (JWT) که باید در هدر Authorization به صورت Bearer ارسال شود
        /// مثال: "eyJhbGciOiJIUzI1NiIs..."
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// نوع توکن (معمولاً "Bearer")
        /// </summary>
        public string TokenType { get; set; } = "Bearer";

        /// <summary>
        /// مدت زمان اعتبار توکن دسترسی (بر حسب ثانیه یا دقیقه)
        /// برای مثال: 900 ثانیه (15 دقیقه)
        /// </summary>
        public int ExpiresIn { get; set; }

        /// <summary>
        /// توکن رفرش (Refresh Token) برای دریافت توکن جدید
        /// اگر در پاسخ اول ارسال نمی‌شود، اینجا می‌تواند خالی باشد.
        /// معمولاً در لاگین ارسال می‌شود.
        /// </summary>
        public string? RefreshToken { get; set; }

        /// <summary>
        /// مدت زمان اعتبار توکن رفرش (بر حسب دقیقه)
        /// مثال: 10080 (7 روز * 24 ساعت * 60 دقیقه)
        /// </summary>
        public int RefreshTokenExpiresIn { get; set; }

        /// <summary>
        /// شناسه توکن رفرش در دیتابیس (اختیاری، برای مدیریت و باطل کردن)
        /// </summary>
        public int? RefreshTokenId { get; set; }

        /// <summary>
        /// اطلاعات کاربر (اختیاری: نام، ایمیل، نقش‌ها)
        /// اگر کلاینت نیاز دارد، می‌توان این بخش را اضافه کرد.
        /// </summary>
        public UserDto? User { get; set; }


        /// <summary>
        /// در صورتی که با خطا مواجه شد
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// بر اساس نقش به چه صفحه ای هدایت شود
        /// </summary>
        public string ReturnToAddress { get; set; }

    }

    /// <summary>
    /// مدل اطلاعات کاربر (برای پاسخ)
    /// </summary>
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<string> Roles { get; set; } = new();
    }

}
