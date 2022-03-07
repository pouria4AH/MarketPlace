using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.DataLayer.Entities.Site;

namespace MarketPlace.Application.Services.interfaces
{
    public interface ISiteService : IAsyncDisposable
    {
        #region site setting

         Task<SiteSetting> GetDefaultSiteSetting();

        #endregion

        #region slider

        Task<List<Slider>> GetAllActiveSlide();

        #endregion

        #region site banner

        Task<List<SiteBanner>> GetSiteBannerByPlacement(List<SiteBannerPlacement> placements);

        #endregion
    }
}
