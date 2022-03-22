using System.ComponentModel.DataAnnotations;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class CreateProductColorDTO
    {
        [Display(Name = "رنگ محصول")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ColorName { get; set; }
        [Display(Name = "قیمت محصول")]
        public int Price { get; set; }
    }
}
