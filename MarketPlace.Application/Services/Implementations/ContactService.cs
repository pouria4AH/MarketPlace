using System;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Contact;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.Entities.Contact;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

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
                UserId = userId != null && userId != 0 ? userId.Value : (long?)null,
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
            if (string.IsNullOrEmpty(ticket.Text)) return AddTicketResult.Error;

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

        public async Task<FilterTicketDTO> FilterTickets(FilterTicketDTO filter)
        {
            var query = _ticketRepository.GetQuery().AsQueryable();

            #region state
            switch (filter.FilterTicketState)
            {
                case FilterTicketState.All:
                    break;
                case FilterTicketState.Deleted:
                    query = query.Where(x => x.IsDelete);
                    break;
                case FilterTicketState.NotDeleted:
                    query = query.Where(x => !x.IsDelete);
                    break;
            }
            #endregion
            #region Order
            switch (filter.OrderBy)
            {
                case FilterTicketOrder.CreateDate_AC:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
                case FilterTicketOrder.CreateDate_DC:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
            }
            #endregion
            #region filter

            if (filter.FilterTicketState != null)
                query = query.Where(x => x.TicketSection == filter.TicketSection.Value);
            if (filter.TicketPriorIty != null)
                query = query.Where(x => x.TicketPriorIty == filter.TicketPriorIty.Value);
            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(x => x.OwnerId == filter.UserId.Value);
            if (!string.IsNullOrEmpty(filter.Title))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.Title}%"));

            #endregion

            #region paging

            var ticketCount = await query.CountAsync();
            var pager = Pager.Build(filter.PageId, filter.AllEntitiesCount, filter.TakeEntities,
                filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.paging(pager).ToListAsync();
            
            #endregion
            return filter.SetPaging(pager).SetTicket(allEntities);
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
