using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.Entities.Site;

namespace MarketPlace.Application.EntitiesExtensions
{
    public static class SliderExtensions
    {
        public static string GetSliderImageAddress(this Slider slider)
        {
            return PathExtension.SlideOrigin + slider.ImageName;
        }
    }

    public static class SiteBannerExtensions
    {
        public static string GetSiteBannerAddress(this SiteBanner banner)
        {
            return PathExtension.SiteBannerOrigin + banner.ImageAddress;
        }
    }
}
