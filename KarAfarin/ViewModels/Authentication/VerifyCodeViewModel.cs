using System.ComponentModel.DataAnnotations;

namespace KarAfarin.ViewModels.Authentication
{
    public class VerifyCodeViewModel
    {
        [RegularExpression("^(?:(\u0660\u0669[\u0660-\u0669][\u0660-\u0669]{8})|(\u06F0\u06F9[\u06F0-\u06F9][\u06F0-\u06F9]{8})|(09[0-9][0-9]{8}))$", ErrorMessage = "لطفا شماره تلفن همراه را به صورت صحیح وارد نمایید")]
        [Required(ErrorMessage = "وارد کردن شماره موبایل الزامی است")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "وارد کردن کد الزامی می باشد")]
        public string VerifyCode { get; set; }

        public string IpAddress { get; set; } = "";
    }
}