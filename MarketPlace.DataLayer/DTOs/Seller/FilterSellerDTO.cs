using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MarketPlace.DataLayer.DTOs.Paging;

namespace MarketPlace.DataLayer.DTOs.Seller
{
    public class FilterSellerDTO : BasePaging
    {
        public long? UserId { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public List<Entities.Store.Seller> Sellers { get; set; }
        public FilterSellerState State { get; set; }
        #region methods

        public FilterSellerDTO SetSeller(List<Entities.Store.Seller> sellers)
        {
            this.Sellers = sellers;
            return this;
        }

        public FilterSellerDTO SetPaging(BasePaging paging)
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
    public enum FilterSellerState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "در حال برسی")]
        UnderProgress,
        [Display(Name = "قبول شده")]
        Accepted,
        [Display(Name = "رد شده")]
        Rejected
    }
}
