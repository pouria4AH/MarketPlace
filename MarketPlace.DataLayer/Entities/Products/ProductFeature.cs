using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Common;

namespace MarketPlace.DataLayer.Entities.Products
{
    public class ProductFeature : BaseEntity
    {
        #region prop

        public long ProductId { get; set; }
        [Display(Name = "عنوان ویژگی")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureTitle { get; set; }
        [Display(Name = "مقدار ویژگی")]
        [MaxLength(200, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string FeatureValue { get; set; }

        #endregion

        #region relation

        public Product Product { get; set; }
        #endregion
    }
}
