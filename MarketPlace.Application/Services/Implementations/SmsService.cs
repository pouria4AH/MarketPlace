using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;

namespace MarketPlace.Application.Services.Implementations
{
    public class SmsService : ISmsService
    {
        private readonly string apiKey =
            "_____________";
        public async Task SendVerfiySms(string mobile, string activeCode)
        {
             Kavenegar.KavenegarApi api =  new Kavenegar.KavenegarApi(apiKey);
             api.VerifyLookup(mobile, activeCode, "MarketPlace");
        }

        public async Task SendUserPassword(string mobile, string password)
        {
            Kavenegar.KavenegarApi api = new Kavenegar.KavenegarApi(apiKey);
            api.VerifyLookup(mobile, password, "forgotPassword");
        }
    }
}
