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
        #region user avatar
        public static string UserAvatarOrigin = "/Content/Images/UserAvatar/origin/";
        public static string UserAvatarOriginServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/origin/");

        public static string UserAvatarThumb = "/Content/Images/UserAvatar/Thumb/";
        public static string UserAvatarThumbServer = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Content/Images/UserAvatar/Thumb/");

        #endregion

        #region site banner
        public static string SiteBannerOrigin = "/img/bg/";
        #endregion
    }
}
