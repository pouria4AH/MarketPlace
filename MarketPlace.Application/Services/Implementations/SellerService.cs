using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Seller;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Store;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class SellerService : ISellerService
    {
        #region ctor

        private readonly IGenericRepository<Seller> _sellerRepository;
        private readonly IGenericRepository<User> _useRepository;

        public SellerService(IGenericRepository<Seller> sellerRepository, IGenericRepository<User> useRepository)
        {
            _sellerRepository = sellerRepository;
            _useRepository = useRepository;
        }

        #endregion

        #region seller

        public async Task<RequestSellerResult> AddNewSellerRequest(RequestSellerDTO seller, long userId)
        {
            var user = await _useRepository.GetEntityById(userId);
            if (user.IsBlocked) return RequestSellerResult.HasNotPermission;

            var hasInprogress = await _sellerRepository.GetQuery().AsQueryable().AnyAsync(x =>
                x.UserId == userId && x.SellerAcceptanceState == SellerAcceptanceState.UnderProgress);
            if (hasInprogress) return RequestSellerResult.HasUnderProgressRequest;

            var newSeller = new Seller
            {
                UserId = userId,
                Address = seller.Address,
                Phone = seller.Phone,
                StoreName = seller.StoreName,
                SellerAcceptanceState = SellerAcceptanceState.UnderProgress
            };
            await _sellerRepository.AddEntity(newSeller);
            await _sellerRepository.SaveChanges();
            return RequestSellerResult.Success;
        }


        #endregion


        #region dispose
        public async ValueTask DisposeAsync()
        {
            if (_useRepository != null) await _useRepository.DisposeAsync();
            if (_sellerRepository != null) await _sellerRepository.DisposeAsync();
        }

        #endregion

    }
}
