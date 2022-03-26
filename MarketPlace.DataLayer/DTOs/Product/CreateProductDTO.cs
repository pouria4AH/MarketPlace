using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class CreateProductDTO
    {
        [Display(Name = "نام محصول")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "فعال / غبر فعال")]
        public bool IsActive { get; set; }

        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [MaxLength(230, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        public List<CreateProductColorDTO> ProductColors { get; set; }
        public List<long> SelectedCategories { get; set; }
    }

    public enum CreateProductResult
    {
        Success,
        Error,
        IsNotImage
    }
}
