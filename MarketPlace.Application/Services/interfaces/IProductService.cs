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
        Task<CreateProductState> CreateProduct(CreateProductDTO product, string imageName, long sellerId);
        #endregion

        #region ProductCategories

        Task<List<ProductCategory>> GetAllProductCategoryBy(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategories();

        #endregion

    }
}
