using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.Common
{
    public class RejectItemDTO
    {
        public long Id { get; set; }
        [Display(Name = "دلیل رد | تایید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string RejectDescription { get; set; }
    }
}
