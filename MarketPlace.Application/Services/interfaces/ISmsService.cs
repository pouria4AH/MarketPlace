using System.Threading.Tasks;

namespace MarketPlace.Application.Services.interfaces
{
    public interface ISmsService
    {
        public Task SendVerfiySms(string mobile, string activeCode);
        public Task SendUserPassword(string mobile, string password);
    }
}
