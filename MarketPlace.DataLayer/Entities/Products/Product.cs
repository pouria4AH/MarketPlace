using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Common;
using MarketPlace.DataLayer.Entities.Store;

namespace MarketPlace.DataLayer.Entities.Products
{
    public class Product : BaseEntity
    {
        #region prop
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
        [Display(Name = "وضعیت")]
        public ProductAcceptanceState ProductAcceptanceState { get; set; }
        [Display(Name = "دلیل رد / تایید")]
        public string ProductAcceptOrRejectDescription { get; set; }

        #endregion

        #region relation

        public ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public ICollection<ProductColors> ProductColorses { get; set; }
        public ICollection<ProductGallery> ProductGalleries { get; set; }
        public Seller Seller { get; set; }

        #endregion
    }

    public enum ProductAcceptanceState
    {
        [Display(Name = "در حال برسی")]
        UnderProcess,
        [Display(Name = "تایید شده")]
        Accept,
        [Display(Name = "رد شده")]
        Reject
    }
}
