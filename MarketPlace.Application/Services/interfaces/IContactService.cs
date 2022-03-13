using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Contact;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        #region contac us
        Task CreateContactUs(ContactUsDTO contact, string userIP, long? userId);
        #endregion

        #region Ticket

        Task<AddTicketResult> AddUserTicket(AddTicketDTO ticket, long userId);
        Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter);
        Task<TicketDetailDTO> GetTicketForShow(long ticketId, long userId);

        #endregion

    }
}
