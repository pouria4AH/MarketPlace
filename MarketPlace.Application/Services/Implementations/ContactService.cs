using System;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.DataLayer.Entities.Contact;
using MarketPlace.DataLayer.Repository;

namespace MarketPlace.Application.Services.Implementations
{
   public class ContactService : IContactService
    {
        #region constractore

        private readonly IGenericRepository<ContactUs> _contactUsRepository;
        private readonly IGenericRepository<Ticket> _ticketRepository;
        private readonly IGenericRepository<TicketMessage> _ticketMessageRepository;
        public ContactService(IGenericRepository<ContactUs> contactUsRepository, IGenericRepository<Ticket> ticketRepository, IGenericRepository<TicketMessage> ticketMessageRepository)
        {
            _contactUsRepository = contactUsRepository;
            _ticketRepository = ticketRepository;
            _ticketMessageRepository = ticketMessageRepository;
        }

        #endregion
        #region contact us
        public async Task CreateContactUs(ContactUsDTO contact, string userIp, long? userId)
        {
            var contactUs = new ContactUs
            {
                FullName = contact.FullName,
                UserId = userId != null && userId != 0 ? userId.Value : (long?) null,
                UserIP = userIp,
                Email = contact.Email,
                Subject = contact.Subject,
                Text = contact.Text
            };
            await _contactUsRepository.AddEntity(contactUs);
            await _contactUsRepository.SaveChanges();
        }
        #endregion
        #region Ticket

        public async Task<AddTicketResult> AddUserTicket(AddTicketViewModel ticket, long userId)
        {
            if (!string.IsNullOrEmpty(ticket.Text)) return AddTicketResult.Error;

            var newTicket = new Ticket
            {
                OwnerId = userId,
                IsReadByOwner = true,
                Title = ticket.Title,
                TicketSection = ticket.TicketSection,
                TicketPriorIty = ticket.TicketPriorIty,
                TicketState = TicketState.UnderProcess
            };
            await _ticketRepository.AddEntity(newTicket);
            await _ticketRepository.SaveChanges();
            // Ticket Message
            var newMessage = new TicketMessage
            {
                TicketId = newTicket.Id,
                Text = ticket.Text,
                SenderId = userId
            };

            await _ticketMessageRepository.AddEntity(newMessage);
            await _ticketMessageRepository.SaveChanges();

            return AddTicketResult.Success;
        }

        #endregion
        #region dispose
        public async ValueTask DisposeAsync()
        {
            await _contactUsRepository.DisposeAsync();
        }
        #endregion
    }
}
