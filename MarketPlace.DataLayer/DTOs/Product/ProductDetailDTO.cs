using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Products;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class ProductDetailDTO
    {
        public long SellerId { get; set; }
        [Display(Name = "نام محصول")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Title { get; set; }
        [Display(Name = "نام عکس")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }
        [Display(Name = "فعال / غبر فعال")]
        public bool IsActive { get; set; }
        [Display(Name = "قیمت محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Price { get; set; }
        [Display(Name = "توضیحات کوتاه")]
        [MaxLength(230, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }
        [Display(Name = "توضیحات")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        public Entities.Store.Seller Seller { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductColors> ProductColors { get; set; }
        public List<ProductGallery> ProductGalleries { get; set; }
        public List<ProductFeature> ProductFeatures { get; set; }
    }
}
