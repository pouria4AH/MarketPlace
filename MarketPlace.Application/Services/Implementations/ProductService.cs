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
            var query = _productRepository.GetQuery()
                .Include(x => x.ProductSelectedCategories)
                .ThenInclude(x => x.ProductCategory)
                .AsQueryable();

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

            switch (filter.ProductOrderBy)
            {
                case FilterProductOrderBy.CreateDate_Des:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.CreateDate_Aec:
                    query = query.OrderBy(x => x.CreateDate);
                    break;
                case FilterProductOrderBy.Price_Des:
                    query = query.OrderByDescending(x => x.Price);
                    break;
                case FilterProductOrderBy.Price_Asc:
                    query = query.OrderBy(x => x.Price);
                    break;
            }

            #endregion

            #region filter

            if (!string.IsNullOrEmpty(filter.ProductTitle))
                query = query.Where(x => EF.Functions.Like(x.Title, $"%{filter.ProductTitle}%"));
            if (filter.SellerId != null && filter.SellerId != 0)
                query = query.Where(x => x.SellerId == filter.SellerId.Value);

            var expensivePrice = await query.OrderByDescending(x => x.Price).FirstOrDefaultAsync();
            filter.FilterMaxPrice = expensivePrice.Price;

            if (filter.SelectedMaxPrice == 0) filter.SelectedMaxPrice = expensivePrice.Price;

            query = query.Where(x => x.Price >= filter.SelectedMinPrice);
            query = query.Where(x => x.Price <= filter.SelectedMaxPrice);
            if (!string.IsNullOrEmpty(filter.Category))
            {
                query = query.Where(x =>
                    x.ProductSelectedCategories.Any(x => x.ProductCategory.UrlName == filter.Category));
            }
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

        public async Task<List<ProductGallery>> GetAllProductGalleryForSeller(long id, long sellerId)
        {
            return await _productGalleryRepository.GetQuery()
                .Include(x => x.Product)
                .Where(x => x.ProductId == id && x.Product.SellerId == sellerId).ToListAsync();
        }

        public async Task<CreateOurEditProductGalleryResult> CreateProductGallery(CreateOurEditProductGalleryDTO createOurEdit, long productId, long sellerId)
        {
            var product = await _productRepository.GetEntityById(productId);
            if (product == null) return CreateOurEditProductGalleryResult.ProductNotFound;
            if (product.SellerId != sellerId) return CreateOurEditProductGalleryResult.NotForUserProduct;
            if (createOurEdit.Image == null || !createOurEdit.Image.IsImage()) return CreateOurEditProductGalleryResult.ImageIsNull;

            var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(createOurEdit.Image.FileName);
            createOurEdit.Image.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100,
                PathExtension.ProductGalleryThumbServer);
            await _productGalleryRepository.AddEntity(new ProductGallery
            {
                ProductId = productId,
                ImageName = imageName,
                DisplayPriority = createOurEdit.DisplayPriority
            });
            await _productGalleryRepository.SaveChanges();
            return CreateOurEditProductGalleryResult.Success;
        }

        public async Task<CreateOurEditProductGalleryDTO> GetProductGalleryFourEdit(long galleryId, long sellerId)
        {
            var gallery = await _productGalleryRepository.GetQuery()
                .Include(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == galleryId && x.Product.SellerId == sellerId);
            if (gallery == null) return null;
            return new CreateOurEditProductGalleryDTO
            {
                ImageName = gallery.ImageName,
                DisplayPriority = gallery.DisplayPriority
            };
        }

        public async Task<CreateOurEditProductGalleryResult> EditProductGallery(long galleryId, long SellerId, CreateOurEditProductGalleryDTO gallery)
        {
            var mainGallery = await _productGalleryRepository.GetQuery()
                .Include(x => x.Product)
                .SingleOrDefaultAsync(x => x.Id == galleryId);
            if (mainGallery == null) return CreateOurEditProductGalleryResult.ProductNotFound;
            if (mainGallery.Product.SellerId != SellerId) return CreateOurEditProductGalleryResult.NotForUserProduct;
            if (gallery.Image != null && gallery.Image.IsImage())
            {
                var imageName = Guid.NewGuid().ToString("N") + Path.GetExtension(gallery.Image.FileName);
                gallery.Image.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100,
                    PathExtension.ProductGalleryThumbServer, mainGallery.ImageName);

                mainGallery.ImageName = imageName;
            }
            mainGallery.DisplayPriority = gallery.DisplayPriority;
            _productGalleryRepository.EditEntity(mainGallery);
            await _productGalleryRepository.SaveChanges();
            return CreateOurEditProductGalleryResult.Success;
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
