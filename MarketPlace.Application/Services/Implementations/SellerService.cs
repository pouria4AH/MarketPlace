using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Paging;
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

        public async Task<FilterSellerDTO> FilterSellers(FilterSellerDTO filter)
        {
            #region comment old code


            //var query = _sellerRepository.GetQuery()
            //               .Include(x => x.User).AsQueryable();

            //           #region state

            //           switch (filter.State)
            //           {
            //               case FilterSellerState.All:
            //                   query = query.Where(x => !x.IsDelete);
            //                   break;
            //               case FilterSellerState.Accepted:
            //                   query = query.Where(x => x.SellerAcceptanceState == SellerAcceptanceState.Accepted && !x.IsDelete);
            //                   break;
            //               case FilterSellerState.Rejected:
            //                   query = query.Where(x => x.SellerAcceptanceState == SellerAcceptanceState.Rejected && !x.IsDelete);
            //                   break;
            //               case FilterSellerState.UnderProgress:
            //                   query = query.Where(x => x.SellerAcceptanceState == SellerAcceptanceState.UnderProgress && !x.IsDelete);
            //                   break;
            //           }
            //           #endregion

            //           #region filter

            //           if (filter.UserId != null && filter.UserId != 0)
            //               query = query.Where(x => x.UserId == filter.UserId);
            //           if (!string.IsNullOrEmpty(filter.StoreName))
            //               query = query.Where(x => EF.Functions.Like(x.StoreName, $"%{filter.StoreName}%"));
            //           if (!string.IsNullOrEmpty(filter.Phone))
            //               query = query.Where(x => EF.Functions.Like(x.Phone, $"%{filter.Phone}%"));
            //           if (!string.IsNullOrEmpty(filter.Address))
            //               query = query.Where(x => EF.Functions.Like(x.Address, $"%{filter.Address}%"));

            //           #endregion
            //           #region paging

            //           //query = query.OrderByDescending(x => x.Id);
            //           var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntities, filter.HowManyShowPageAfterAndBefore);
            //           var allEntities = await query.Paging(pager).ToListAsync();
            //           #endregion

            //           return filter.SetPaging(pager).SetSeller(allEntities);
            #endregion
            var query = _sellerRepository.GetQuery()
                 .Include(s => s.User)
                 .AsQueryable();

            #region state

            switch (filter.State)
            {
                case FilterSellerState.All:
                    query = query.Where(s => !s.IsDelete);
                    break;
                case FilterSellerState.Accepted:
                    query = query.Where(s => s.SellerAcceptanceState == SellerAcceptanceState.Accepted && !s.IsDelete);
                    break;

                case FilterSellerState.UnderProgress:
                    query = query.Where(s => s.SellerAcceptanceState == SellerAcceptanceState.UnderProgress && !s.IsDelete);
                    break;
                case FilterSellerState.Rejected:
                    query = query.Where(s => s.SellerAcceptanceState == SellerAcceptanceState.Rejected && !s.IsDelete);
                    break;
            }

            #endregion

            #region filter

            if (filter.UserId != null && filter.UserId != 0)
                query = query.Where(s => s.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.StoreName))
                query = query.Where(s => EF.Functions.Like(s.StoreName, $"%{filter.StoreName}%"));

            if (!string.IsNullOrEmpty(filter.Phone))
                query = query.Where(s => EF.Functions.Like(s.Phone, $"%{filter.Phone}%"));

            if (!string.IsNullOrEmpty(filter.Mobile))
                query = query.Where(s => EF.Functions.Like(s.Mobile, $"%{filter.Mobile}%"));

            if (!string.IsNullOrEmpty(filter.Address))
                query = query.Where(s => EF.Functions.Like(s.Address, $"%{filter.Address}%"));

            #endregion

            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntities, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion
            return filter.SetPaging(pager).SetSeller(allEntities);
        }

        public async Task<EditRequestSellerDTO> GetRequestSellerForEdit(long id, long currentUserId)
        {
            var seller = await _sellerRepository.GetEntityById(id);
            if (seller == null || seller.UserId != currentUserId) return null;
            return new EditRequestSellerDTO
            {
                Id = seller.Id,
                Address = seller.Address,
                Phone = seller.Phone,
                StoreName = seller.StoreName
            };
        }

        public async Task<EditRequestResult> EditRequsetSeller(EditRequestSellerDTO request, long currentId)
        {
            var seller = await _sellerRepository.GetEntityById(request.Id);
            if (seller == null || seller.UserId != currentId) return EditRequestResult.NotFound;
            seller.Phone = request.Phone;
            seller.StoreName = request.StoreName;
            seller.Address = request.Address;
            seller.SellerAcceptanceState = SellerAcceptanceState.UnderProgress;
            _sellerRepository.EditEntity(seller);
            await _sellerRepository.SaveChanges();
            return EditRequestResult.Success;
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
