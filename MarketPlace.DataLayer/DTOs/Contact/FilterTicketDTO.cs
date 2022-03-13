using System.Collections.Generic;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Contact;

namespace MarketPlace.DataLayer.DTOs.Contact
{
    public class FilterTicketDTO : BasePaging
    {
        public string Title { get; set; }
        public long? UserId { get; set; }
        public FilterTicketState FilterTicketState { get; set; }
        public FilterTicketOrder OrderBy { get; set; }
        public List<Ticket> Tickets { get; set; }
        public TicketSection? TicketSection { get; set; }
        public TicketPriorIty? TicketPriorIty { get; set; }

        #region methods

        public FilterTicketDTO SetTicket(List<Ticket> tickets)
        {
            this.Tickets = tickets;
            return this;
        }

        public FilterTicketDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.TakeEntities = paging.TakeEntities;
            this.SkipEntities = paging.SkipEntities;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion
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
