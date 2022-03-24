using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class CreateOurEditProductGalleryDTO
    {
        [Display(Name = "اولویت محصول")]
        public int DisplayPriority { get; set; }
        [Display(Name = "عکس محصول")]
        public IFormFile Image { get; set; }
        [Display(Name = " عکس محصول نام")]
        public string ImageName { get; set; }
    }

    public enum CreateOurEditProductGalleryResult
    {
        Success,
        NotForUserProduct,
        ImageIsNull,
        ProductNotFound
    }
}
