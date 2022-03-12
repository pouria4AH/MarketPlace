using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Common;

namespace MarketPlace.DataLayer.Entities.Contact
{
    public class Ticket : BaseEntity
    {
        #region properties
        public long OwnerId { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "خواننده شده توسط کاربر")]
        public bool IsReadByOwner { get; set; }

        [Display(Name = " خوانده شده توسط ادمین")]
        public bool IsReadByaAdmin { get; set; }

        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }

        [Display(Name = "اولویت")]
        public TicketPriorIty TicketPriorIty { get; set; }

        [Display(Name = "وضعیت")]
        public TicketState TicketState { get; set; }

        #endregion

        #region relation
        public User Owner { get; set; }
        public ICollection<TicketMessage> TicketMessages { get; set; }
        #endregion
    }
    public enum TicketSection
    {
        [Display(Name = "پشتیبانی")]
        Support,
        [Display(Name = "فنی")]
        Technical,
        [Display(Name = "اموزشی")]
        Academic
    }
    public enum TicketPriorIty
    {
        [Display(Name = "پایین")]
        Low,
        [Display(Name = "متوسط")]
        Medium,
        [Display(Name = "بالا")]
        High
    }
    public enum TicketState
    {
        [Display(Name = "در حال برسی")]
        UnderProcess,
        [Display(Name = "پاسخ داده شده")]
        Answered,
        [Display(Name = "بسته شده")]
        Closed
    }
}
