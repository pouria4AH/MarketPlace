using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Common;
using MarketPlace.DataLayer.Entities.Products;

namespace MarketPlace.DataLayer.Entities.Store
{
    public class Seller : BaseEntity
    {
        #region properties
        public long UserId { get; set; }
        [Display(Name = "نام فروشگاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string StoreName { get; set; }
        [Display(Name = "تلفن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Phone { get; set; }
        [Display(Name = "تلفن همراه")]
        [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Mobile { get; set; }
        [Display(Name = "ادرس")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Address { get; set; }
        [Display(Name = "توضیحات")]
        [MaxLength(230, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Description { get; set; }
        [Display(Name = "توضیحات ادمین")]
        [MaxLength(230, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string AdminDescription { get; set; }
        [Display(Name = "دلیل رد | تایید")]
        public string SellerAcceptanceStateDescription { get; set; }
        public SellerAcceptanceState SellerAcceptanceState { get; set; }
        #endregion

        #region relations
        public User User { get; set; }
        #endregion
    }

    public enum SellerAcceptanceState
    {
        [Display(Name = "در حال برسی")]
        UnderProgress,
        [Display(Name = "قبول شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
