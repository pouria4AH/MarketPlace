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
        Task<EditProductResult> EditSellerProduct(EditProductDTO product, long userId, IFormFile productImage);
        Task RemoveAllProductSelectedCategories(long productId);
        Task RemoveAllProductSelectedColors(long productId);
        Task AddProductSelectedColors(long productId, List<CreateProductColorDTO> colors);
        Task AddProductSelectedCategories(long productId, List<long> selectedCategories);
        Task<ProductDetailDTO> GetProductDetailBy(long productId);
        #endregion

        #region ProductCategories

        Task<List<ProductCategory>> GetAllProductCategoryBy(long? parentId);
        Task<List<ProductCategory>> GetAllActiveProductCategories();

        #endregion

        #region product gallery

        Task<List<ProductGallery>> GetAllProductGallery(long id);
        Task<Product> GetProductBySellerOwnerId(long productId, long userId);
        Task<List<ProductGallery>> GetAllProductGalleryForSeller(long id, long sellerId);

        Task<CreateOurEditProductGalleryResult> CreateProductGallery(CreateOurEditProductGalleryDTO createOurEdit, long productId,
            long sellerId);

        Task<CreateOurEditProductGalleryDTO> GetProductGalleryFourEdit(long galleryId, long sellerId );
        Task<CreateOurEditProductGalleryResult> EditProductGallery(long galleryId, long SellerId, CreateOurEditProductGalleryDTO gallery);

        #endregion

        #region product feature

        Task CreateProductFeature(List<CreateProductFeatureDTO> features);
        Task RemoveAllProductFeature(long productId);

        #endregion
    }
}
