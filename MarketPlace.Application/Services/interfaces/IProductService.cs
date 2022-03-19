using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region product
        Task<FilterProductDTO> FilterProducts(FilterProductDTO filter);
        #endregion

        #region ProductCategories

        Task<List<ProductCategory>> GetAllProductCategoryBy(long? parentId);

        #endregion

    }
}
