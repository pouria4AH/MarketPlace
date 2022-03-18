using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.Common;
using MarketPlace.DataLayer.DTOs.Seller;

namespace MarketPlace.Application.Services.interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId);
        Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter);
        Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId);
        Task<EditRequestResult> EditRequestsSeller(EditRequestSellerDTO request, long currentId);
        Task<bool> AcceptSellerRequests(long requesetId);
        Task<bool> RejectSellerRequests(RejectItemDTO reject);
    }
}
