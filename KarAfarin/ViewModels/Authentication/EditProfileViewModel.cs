using System.ComponentModel.DataAnnotations;

namespace KarAfarin.ViewModels.Authentication
{
    public class EditProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "وارد کردن نام الزامی می باشد")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "وارد کردن نام خانوادگی الزامی می باشد")]
        public string LastName { get; set; }

        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$", ErrorMessage = "پست الکترونیک معتبر نمی باشد")]
        public string? Email { get; set; }
    }
}
