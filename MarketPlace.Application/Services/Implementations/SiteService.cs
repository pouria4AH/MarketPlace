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

        public SiteService(IGenericRepository<SiteSetting> siteGenericRepository)
        {
            _siteGenericRepository = siteGenericRepository;
        }

        #endregion

        #region Site seitSettng

        public async Task<SiteSetting> GetDefaultSiteSetting()
        {
            return await _siteGenericRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(x => x.IsDefault && !x.IsDelete);
        }

        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _siteGenericRepository.DisposeAsync();
        }


        #endregion
    }
}
