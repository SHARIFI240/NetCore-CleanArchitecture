using System.ComponentModel.DataAnnotations;

namespace KarAfarin.ViewModels.Admin.Category
{
    public class CreateUpdateCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی می باشد")]
        public string Title  { get; set; }
    }
}
