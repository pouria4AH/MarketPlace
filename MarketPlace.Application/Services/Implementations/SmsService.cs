using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;

namespace MarketPlace.Application.Services.Implementations
{
    public class SmsService : ISmsService
    {
        private readonly string apiKey =
            "6B494F776A744159772F3634497262486F4C6A655A532F2B652F42632B5A68657A2B41614D624678736C733D";
        public async Task SendVerfiySms(string mobile, string activeCode)
        {
             Kavenegar.KavenegarApi api =  new Kavenegar.KavenegarApi(apiKey);
             api.VerifyLookup(mobile, activeCode, "MarketPlace");
        }
    }
}
