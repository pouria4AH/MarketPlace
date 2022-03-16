using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Seller;

namespace MarketPlace.Application.Services.interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId);
        Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter);
        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId);
        Task<EditRequestResult> EditRequsetSeller(EditRequestSellerDTO request, long currentId);
    }
}
