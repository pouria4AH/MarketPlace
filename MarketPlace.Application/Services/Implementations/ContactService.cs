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

        private readonly IGenericRepository<ContactUs> _repository;

        public ContactService(IGenericRepository<ContactUs> repository)
        {
            _repository = repository;
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
            await _repository.AddEntity(contactUs);
            await _repository.SaveChanges();
        }
        #endregion
        #region dispose
        public async ValueTask DisposeAsync()
        {
            await _repository.DisposeAsync();
        }
        #endregion
    }
}
