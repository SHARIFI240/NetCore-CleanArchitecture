using System.ComponentModel.DataAnnotations;

namespace KarAfarin.ViewModels.Admin.Article
{
    public class CreateUpdateArticleViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "وارد کردن عنوان الزامی می باشد")]
        public string Title { get; set; }

        [Required(ErrorMessage = "وارد کردن دسته بندی الزامی می باشد")]
        public int CategoryRef { get; set; }


        [Required(ErrorMessage = "وارد کردن چکیده الزامی می باشد")]
        public string Summary { get; set; }


        [Required(ErrorMessage = "وارد کردن مطلب الزامی می باشد")]
        public string Content { get; set; }

        [Required(ErrorMessage = "وارد کردن کلمات کلیدی الزامی می باشد")]
        public string Keywords { get; set; }

        public string? CoverImage { get; set; }
    }
}