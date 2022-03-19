using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs.Paging;

namespace MarketPlace.DataLayer.DTOs.Product
{
    public class FilterProductDTO  : BasePaging
    {
        #region prop

        public string ProductTitle { get; set; }
        public long? SellerId { get; set; }
        public FilterProductState FilterProductState { get; set; }
        public List<Entities.Products.Product> Products { get; set; }
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
        UnderProcess,
        Accept,
        Reject,
        Active,
        NotActive,
        All
    }
}
