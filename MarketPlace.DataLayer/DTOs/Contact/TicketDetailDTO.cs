using System.Collections.Generic;
using MarketPlace.DataLayer.Entities.Contact;

namespace MarketPlace.DataLayer.DTOs.Contact
{
    public class TicketDetailDTO
    {
        public Ticket Ticket { get; set; }
        public List<TicketMessage> TicketMessage { get; set; }
    }
}
