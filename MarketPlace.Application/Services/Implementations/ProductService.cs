using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Extensions;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.Application.Utils;
using MarketPlace.DataLayer.Common;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region ctor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategory;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedRepository;
        private readonly IGenericRepository<ProductColors> _productColorRepository;
        private readonly IGenericRepository<ProductGallery> _productGalleryRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategory, IGenericRepository<ProductSelectedCategory> productSelectedRepository, IGenericRepository<ProductColors> productColorRepository, IGenericRepository<ProductGallery> productGalleryRepository)
        {
            _productRepository = productRepository;
            _productCategory = productCategory;
            _productSelectedRepository = productSelectedRepository;
            _productColorRepository = productColorRepository;
            _productGalleryRepository = productGalleryRepository;
        }

        #endregion
        #region product

        public async Task<FilterProductDTO> FilterProducts(FilterProductDTO filter)
        {
            var query = _productRepository.GetQuery().AsQueryable();

            #region state

            switch (filter.FilterProductState)
            {
                case FilterProductState.All:
                    query = query.Where(x => !x.IsDelete);
                    break;
                case FilterProductState.Active:
                    query = query.Where(x => x.IsActive && x.ProductAcceptanceState == ProductAcceptanceState.Accept && !x.IsDelete);
                    break;
                case FilterProductState.NotActive:
                    query = query.Where(x => !x.IsActive && x.ProductAcceptanceState == ProductAcceptanceState.Accept && !x.IsDelete);
                    break;
                case FilterProductState.Reject:
                    query = query.Where(x => x.ProductAcceptanceState == ProductAcceptanceState.Reject && !x.IsDelete);
                    break;
                case FilterProductState.Accept:
                    query = query.Where(x => x.ProductAcceptanceState == ProductAcceptanceState.Accept && !x.IsDelete);
                    break;
                case FilterProductState.UnderProcess:
                    query = query.Where(x => x.ProductAcceptanceState == ProductAcceptanceState.UnderProcess && !x.IsDelete);
                    break;
            }

            #endregion

            #region filter

            if (!string.IsNullOrEmpty(filter.ProductTitle))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));
            if (filter.SellerId != null && filter.SellerId != 0)
                query = query.Where(x => x.SellerId == filter.SellerId.Value);

            #endregion
            #region paging
            var pager = Pager.Build(filter.PageId, await query.CountAsync(), filter.TakeEntities, filter.HowManyShowPageAfterAndBefore);
            var allEntities = await query.Paging(pager).ToListAsync();
            #endregion

            return filter.SetProduct(allEntities).SetPaging(pager);
        }

        public async Task<CreateProductResult> CreateProduct(CreateProductDTO product, long sellerId, IFormFile productImage)
        {
            if (productImage == null) return CreateProductResult.IsNotImage;
            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
            var res = productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer, 150, 150,
                PathExtension.ProductThumbServer);
            if (res)
            {
                var newProduct = new Product
                {
                    Title = product.Title,
                    Description = product.Description,
                    ShortDescription = product.ShortDescription,
                    Price = product.Price,
                    IsActive = product.IsActive,
                    SellerId = sellerId,
                    ImageName = imageName,
                    ProductAcceptanceState = ProductAcceptanceState.UnderProcess
                };
                await _productRepository.AddEntity(newProduct);
                await _productRepository.SaveChanges();


                if (product.SelectedCategories != null && product.SelectedCategories.Any())
                {
                    await AddProductSelectedCategories(newProduct.Id, product.SelectedCategories);
                    await _productSelectedRepository.SaveChanges();
                }

                if (product.ProductColors != null && product.ProductColors.Any())
                {
                    await AddProductSelectedColors(newProduct.Id, product.ProductColors);
                    await _productColorRepository.SaveChanges();
                }

                return CreateProductResult.Success;
            }

            return CreateProductResult.Error;
        }

        public async Task<bool> AcceptedSellerProduct(long id)
        {
            var product = await _productRepository.GetEntityById(id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Accept;
                product.ProductAcceptOrRejectDescription = $" این محصول در تاریخ {DateTime.Now.ToShamsi()} تایید شد";
                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> RejectSellerProduct(RejectItemDTO reject)
        {
            var product = await _productRepository.GetEntityById(reject.Id);
            if (product != null)
            {
                product.ProductAcceptanceState = ProductAcceptanceState.Reject;
                product.ProductAcceptOrRejectDescription = reject.RejectDescription;
                _productRepository.EditEntity(product);
                await _productRepository.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<EditProductDTO> GetProductForEdit(long productId)
        {
            var product = await _productRepository.GetEntityById(productId);
            if (product == null) return null;
            return new EditProductDTO
            {
                Id = productId,
                Price = product.Price,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                IsActive = product.IsActive,
                ImageName = product.ImageName,
                Title = product.Title,
                ProductColors = await _productColorRepository.GetQuery().AsQueryable()
                     .Where(x => x.ProductId == productId && !x.IsDelete)
                     .Select(x => new CreateProductColorDTO { Price = x.Price, ColorName = x.ColorName }).ToListAsync(),
                SelectedCategories = await _productSelectedRepository.GetQuery().AsQueryable()
                     .Where(x => x.ProductId == productId).Select(x => x.ProductCategoryId).ToListAsync()
            };
        }

        public async Task<EditProductResult> EditSellerProduct(EditProductDTO product, long userId,
            IFormFile productImage)
        {
            var mainProduct = await _productRepository.GetQuery().AsQueryable()
                .Include(x => x.Seller)
                .SingleOrDefaultAsync(x => x.Id == product.Id);
            if (mainProduct == null) return EditProductResult.NotFound;
            if (mainProduct.Seller.UserId != userId) return EditProductResult.NotForUser;

            mainProduct.Title = product.Title;
            mainProduct.ShortDescription = product.ShortDescription;
            mainProduct.Description = product.Description;
            mainProduct.IsActive = product.IsActive;
            mainProduct.Price = product.Price;
            mainProduct.ProductAcceptanceState = ProductAcceptanceState.UnderProcess;
            await RemoveAllProductSelectedCategories(product.Id);

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(productImage.FileName);
            var res = productImage.AddImageToServer(imageName, PathExtension.ProductOriginServer, 150, 150,
                PathExtension.ProductThumbServer, mainProduct.ImageName);
            if (res)
            {
                mainProduct.ImageName = imageName;
            }

            await RemoveAllProductSelectedCategories(product.Id);
            await AddProductSelectedCategories(product.Id, product.SelectedCategories);
            await _productSelectedRepository.SaveChanges();
            await RemoveAllProductSelectedColors(product.Id);
            await AddProductSelectedColors(product.Id, product.ProductColors);
            await _productColorRepository.SaveChanges();

            return EditProductResult.Success;
        }

        public async Task RemoveAllProductSelectedCategories(long productId)
        {
            _productSelectedRepository.DeletePermanentEntities(await _productSelectedRepository.GetQuery().AsQueryable().Where(s => s.ProductId == productId).ToListAsync());
        }

        public async Task RemoveAllProductSelectedColors(long productId)
        {
            _productColorRepository.DeletePermanentEntities(await _productColorRepository.GetQuery().AsQueryable().Where(s => s.ProductId == productId).ToListAsync());
        }

        public async Task AddProductSelectedColors(long productId, List<CreateProductColorDTO> colors)
        {
            var productSelecteadColor = new List<ProductColors>();
            foreach (var color in colors)
            {
                if (productSelecteadColor.All(x => x.ColorName != color.ColorName))
                {
                    productSelecteadColor.Add(new ProductColors
                    {
                        ColorName = color.ColorName,
                        Price = color.Price,
                        ProductId = productId
                    });
                }

            }
            await _productColorRepository.AddRangeEntities(productSelecteadColor);

        }

        public async Task AddProductSelectedCategories(long productId, List<long> selectedCategories)
        {
            var productSelectedCategories = new List<ProductSelectedCategory>();
            foreach (var categoryId in selectedCategories)
            {
                productSelectedCategories.Add(new ProductSelectedCategory
                {
                    ProductId = productId,
                    ProductCategoryId = categoryId
                });
            }

            await _productSelectedRepository.AddRangeEntities(productSelectedCategories);
        }

        #endregion
        #region product gallery

        public async Task<List<ProductGallery>> GetAllProductGallery(long id)
        {
            return await _productGalleryRepository.GetQuery().AsQueryable()
                .Where(x => x.ProductId == id).ToListAsync();
        }

        public async Task<Product> GetProductBySellerOwnerId(long productId, long userId)
        {
            return await _productRepository.GetQuery()
                .Include(x => x.Seller)
                .SingleOrDefaultAsync(x => x.Seller.UserId == userId && x.Id == productId);
        }

        public async Task<List<ProductGallery>> GetAllProductGalleryForSeller(long id, long userId)
        {
            return await _productGalleryRepository.GetQuery()
                .Include(x => x.Product)
                .Where(x => x.ProductId == id && x.Product.Seller.UserId == userId).ToListAsync();
        }

        #endregion
        #region ProductCategories

        public async Task<List<ProductCategory>> GetAllProductCategoryBy(long? parentId)
        {
            if (parentId == null || parentId == 0)
            {
                return await _productCategory.GetQuery().AsQueryable()
                    .Where(x => !x.IsDelete && x.IsActive && x.ParentId == null)
                    .ToListAsync();
            }

            return await _productCategory.GetQuery().AsQueryable()
                .Where(x => !x.IsDelete && x.IsActive && x.ParentId == parentId).ToListAsync();
        }

        public async Task<List<ProductCategory>> GetAllActiveProductCategories()
        {
            return await _productCategory.GetQuery().AsQueryable()
                .Where(x => !x.IsDelete && x.IsActive).ToListAsync();
        }

        #endregion
        #region dispose
        public async ValueTask DisposeAsync()
        {
            await _productSelectedRepository.DisposeAsync();
            await _productRepository.DisposeAsync();
            await _productCategory.DisposeAsync();
            await _productColorRepository.DisposeAsync();
        }
        #endregion

    }
}
