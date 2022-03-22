using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MarketPlace.DataLayer.Common;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        #region product
        Task<FilterProductDTO> FilterProducts(FilterProductDTO filter);
        Task<CreateProductResult> CreateProduct(CreateProductDTO product, long sellerId, IFormFile productImage);
        Task<bool> AcceptedSellerProduct(long id);
        Task<bool> RejectSellerProduct(RejectItemDTO reject);
        Task<EditProductDTO> GetProductForEdit(long productId);
        #endregion

        #region ProductCategories

        Task<List<ProductCategory>> GetAllProductCategoryBy(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategories();

        #endregion

    }
}
