using System.Collections.Generic;
using MarketPlace.DataLayer.Entities.Contact;

namespace MarketPlace.DataLayer.DTOs.Contact
{
    public class FilterTicketDTO
    {
        public string Title { get; set; }
        public long? UserId { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }
        public List<Ticket> Tickets { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriorIty? TicketPriorIty { get; set; }
    }

    public enum FilterTicketState
    {
        All,
        NotDeleted,
        Deleted
    }  
    public enum FilterTicketOrder
    {
       CreateDate_AC,
       CreateDate_DC
    }

}
