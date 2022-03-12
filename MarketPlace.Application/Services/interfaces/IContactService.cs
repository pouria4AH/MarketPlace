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

        Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId);

        #endregion

    }
}
