using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Contact;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IContactService : IAsyncDisposable
    {
        Task CreateContactUs(ContactUsDTO contact,string userIP , long? userId);
    }
}
