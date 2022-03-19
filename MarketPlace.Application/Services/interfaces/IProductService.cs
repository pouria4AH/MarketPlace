using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Product;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region product
        Task<FilterProductDTO> FilterProducts(FilterProductDTO filter);
        #endregion

    }
}
