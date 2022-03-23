using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Common;

namespace MarketPlace.DataLayer.Entities.Products
{
   public class ProductGallery : BaseEntity
   {
       #region prop

       public long ProductId { get; set; }
       [Display(Name = "ترتیب نمایش")]
        public int DisplayPriority { get; set; }
       [Display(Name = "عکس محصول")]
       [MaxLength(50, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
       [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ImageName { get; set; }

        #endregion

        #region realstion

        public Product Product { get; set; }

        #endregion
    }
}
