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

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategory, IGenericRepository<ProductSelectedCategory> productSelectedRepository, IGenericRepository<ProductColors> productColorRepository)
        {
            _productRepository = productRepository;
            _productCategory = productCategory;
            _productSelectedRepository = productSelectedRepository;
            _productColorRepository = productColorRepository;
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

                var productSelectedCategories = new List<ProductSelectedCategory>();
                if (product.SelectedCategories != null && product.SelectedCategories.Any())
                {
                    foreach (var categoryId in product.SelectedCategories)
                    {
                        productSelectedCategories.Add(new ProductSelectedCategory
                        {
                            ProductId = newProduct.Id,
                            ProductCategoryId = categoryId
                        });
                    }

                    await _productSelectedRepository.AddRangeEntities(productSelectedCategories);
                    await _productSelectedRepository.SaveChanges();
                }

                if (product.ProductColors != null && product.ProductColors.Any())
                {
                    var productSelectedColor = new List<ProductColors>();
                    foreach (var color in product.ProductColors)
                    {
                        productSelectedColor.Add(new ProductColors
                        {
                            ColorName = color.ColorName,
                            Price = color.Price,
                            ProductId = newProduct.Id
                        });
                    }

                    await _productColorRepository.AddRangeEntities(productSelectedColor);
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
                 Title = product.Title,
                 ProductColors = await _productColorRepository.GetQuery().AsQueryable()
                     .Where(x=>x.ProductId == productId && !x.IsDelete)
                     .Select(x=> new CreateProductColorDTO{ Price =x.Price, ColorName = x.ColorName}).ToListAsync(),
                 SelectedCategories = await _productSelectedRepository.GetQuery().AsQueryable()
                     .Where(x=>x.ProductId == productId).Select(x=>x.ProductCategoryId).ToListAsync()
            };
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
