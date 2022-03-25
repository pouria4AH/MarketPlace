using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.DTOs.Paging;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class FilterProductDTO : BasePaging
    {
        #region prop

        public string ProductTitle { get; set; }
        public long? SellerId { get; set; }
        public int FilterMinPrice { get; set; } 
        public int FilterMaxPrice { get; set; } 
        public int SelectedMinPrice { get; set; } 
        public int SelectedMaxPrice { get; set; }
        public int PriceStep { get; set; } = 1000;
        public FilterProductState FilterProductState { get; set; }
        public List<Entities.Products.Product> Products { get; set; }
        public List<long> ProductCategorySelected { get; set; }
        #endregion

        #region methods

        public FilterProductDTO SetProduct(List<Entities.Products.Product> products)
        {
            this.Products = products;
            return this;
        }

        public FilterProductDTO SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.TakeEntities = paging.TakeEntities;
            this.SkipEntities = paging.SkipEntities;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.PageCount = paging.PageCount;
            return this;
        }

        #endregion
    }

    public enum FilterProductState
    {
        [Display(Name = "در حال برسی")]
        UnderProcess,
        [Display(Name = "تایید شده")]
        Accept,
        [Display(Name = "رد شده")]
        Reject,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیر فعال")]
        NotActive,
        [Display(Name = "همه")]
        All
    }
}






