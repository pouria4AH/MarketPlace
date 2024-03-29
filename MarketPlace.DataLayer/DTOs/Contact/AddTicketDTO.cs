﻿using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Contact;

namespace MarketPlace.DataLayer.DTOs.Contact
{
    public class AddTicketDTO
    {
        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }
        [Display(Name = "بخش مورد نظر")]
        public TicketSection TicketSection { get; set; }
        [Display(Name = "اولویت")]
        public TicketPriorIty TicketPriorIty { get; set; }
        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }

    }

    public enum AddTicketResult
    {
        Error,
        Success
    }
}
