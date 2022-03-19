using System.IO;

namespace MarketPlace.Application.Utils
{
    public static class PathExtension
    {
        #region defalt
        public static string DefaultAvatar = "/img/defaults/avatar.jpg";
        #endregion
        #region slider
        public static string SlideOrigin = "/img/slider/";
        #endregion

        #region uploder

        public static string UploadeImage = "/img/upload/";
        public static string UploadeImageServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/upload/");

        #endregion
        #region user avatar
        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

        public static string UserAvatarThumb = "/Content/Images/UserAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumb/");

        #endregion
        #region user product
        public static string UserProductOrigin = "/Content/Images/Product/origin/";
        public static string UserProductOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/Product/origin/");

        public static string UserProductThumb = "/Content/Images/Product/Thumb/";
        public static string UserProductThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/Product/Thumb/");

        #endregion

        #region site banner
        public static string SiteBannerOrigin = "/img/bg/";
        #endregion
    }
}
