﻿using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Common;

namespace MarketPlace.DataLayer.Entities.Contact
{
    public class TicketMessage : BaseEntity
    {
        #region properties
        public long TicketId { get; set; }

        public long SenderId { get; set; }

        [Display(Name = "متن")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Text { get; set; }
        #endregion

        #region relation
        public Ticket Ticket { get; set; }
        public User Sender { get; set; }
        #endregion
    }
}
