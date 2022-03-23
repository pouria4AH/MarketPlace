using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class CreateProductGalleryDTO
    {
        [Display(Name = "اولویت محصول")]
        public int DisplayPriority { get; set; }
        [Display(Name = "عکس محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public IFormFile Image { get; set; }
    }
}
