﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Paging;
using MarketPlace.DataLayer.DTOs.Product;
using MarketPlace.DataLayer.Entities.Products;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region ctor

        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductCategory> _productCategory;
        private readonly IGenericRepository<ProductSelectedCategory> _productSelectedRepository;

        public ProductService(IGenericRepository<Product> productRepository, IGenericRepository<ProductCategory> productCategory, IGenericRepository<ProductSelectedCategory> productSelectedRepository)
        {
            _productRepository = productRepository;
            _productCategory = productCategory;
            _productSelectedRepository = productSelectedRepository;
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

        public async Task<CreateProductState> CreateProduct(CreateProductDTO product, string imageName, long sellerId)
        {
            var newProduct = new Product
            {
                Title = product.Title,
                Description = product.Description,
                ShortDescription = product.ShortDescription,
                Price = product.Price,
                IsActive = product.IsActive,
                ImageName = imageName,
                SellerId = sellerId
            };
            await _productRepository.AddEntity(newProduct);
            await _productRepository.SaveChanges();
            return CreateProductState.Success;
        }
        #endregion
        #region ProductCategories

        public async Task<List<ProductCategory>> GetAllProductCategoryBy(long? parentId)
        {
            if (parentId == null || parentId == 0)
            {
                return await _productCategory.GetQuery().AsQueryable()
                    .Where(x=>!x.IsDelete && x.IsActive && x.ParentId == null)
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
        }
        #endregion

    }
}