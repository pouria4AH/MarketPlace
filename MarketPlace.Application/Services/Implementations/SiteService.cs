using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.Entities.Site;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class SiteService : ISiteService
    {
        #region constaructor

        private readonly IGenericRepository<SiteSetting> _siteGenericRepository;
        private readonly IGenericRepository<Slider> _slisderRepository;
        private readonly IGenericRepository<SiteBanner> _siteBannerRepository;

        public SiteService(IGenericRepository<SiteSetting> siteGenericRepository, IGenericRepository<Slider> slisderRepository, IGenericRepository<SiteBanner> siteBannerRepository)
        {
            _siteGenericRepository = siteGenericRepository;
            _slisderRepository = slisderRepository;
            _siteBannerRepository = siteBannerRepository;
        }

        #endregion

        #region Site seitSettng

        public async Task<SiteSetting> GetDefaultSiteSetting()
        {
            return await _siteGenericRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(x => x.IsDefault && !x.IsDelete);
        }

        #endregion

        #region slider

        public async Task<List<Slider>> GetAllActiveSlide()
        {
            return await _slisderRepository.GetQuery().AsQueryable().Where(x => x.IsActive && !x.IsDelete).ToListAsync();
        }

        #endregion


        #region site banner

        public async Task<List<SiteBanner>> GetSiteBannerByPlacement(List<SiteBannerPlacement> placements)
        {
            return await _siteBannerRepository.GetQuery().AsQueryable()
                .Where(f => placements.Any(x => x == f.BannerPlacement)).ToListAsync();
        }

        #endregion
        #region dispose

        public async ValueTask DisposeAsync()
        {
            if (_siteGenericRepository != null) await _siteGenericRepository.DisposeAsync();
            if (_slisderRepository != null) await _slisderRepository.DisposeAsync();
            if (_siteBannerRepository != null) _siteBannerRepository.DisposeAsync();
        }


        #endregion
    }
}
