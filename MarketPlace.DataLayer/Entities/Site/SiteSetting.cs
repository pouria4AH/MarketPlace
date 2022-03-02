using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.DataLayer.Entities.Common;

namespace MarketPlace.DataLayer.Entities.Site
{
    public class SiteSetting : BaseEntity
    {
        #region properties
        [Display(Name = "شماره همراه")]
        public string Mobile { get; set; }
        [Display(Name = "شماره ثابت")]
        public string Phone { get; set; }
        [Display(Name = "ادرس")]
        public string Address { get; set; }
        [Display(Name = "متن فوتر")]
        public string FooterText { get; set; }
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
        [Display(Name = "نقشه")]
        public string MapScript { get; set; }
        [Display(Name = "کپی رایت")]
        public string CopyRight { get; set; }
        [Display(Name = "اصلی هست یا خیر")]
        public bool IsDefault { get; set; }

        #endregion
    }
}
