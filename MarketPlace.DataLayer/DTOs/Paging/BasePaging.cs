using System;

namespace MarketPlace.DataLayer.DTOs.Paging
{
    public class BasePaging
    {
        public BasePaging()
        {
            PageId = 1;
            TakeEntities = 10;
            HowManyShowPageAfterAndBefore = 3;
        }
        public int PageId { get; set; }
        public int PageCount { get; set; }
        public int StartPage { get; set; }
        public int EndPage { get; set; }
        public int AllEntitiesCount { get; set; }
        public int TakeEntities { get; set; }
        public int SkipEntities { get; set; }
        public int HowManyShowPageAfterAndBefore { get; set; }

        public int GetLastPage()
        {
            return (int)Math.Ceiling(AllEntitiesCount / (double)TakeEntities);
        }
        public string GetCurrentPagingState()
        {
            var startPage = 1;
            var endPage = AllEntitiesCount;
            if (EndPage > 1)
            {
                startPage = (PageId - 1) * (TakeEntities + 1);
                endPage = PageId * TakeEntities > AllEntitiesCount ? AllEntitiesCount : PageId * TakeEntities;
            }

            return $"نمایش {startPage}-{endPage} از {AllEntitiesCount}";
        }
        public BasePaging GetCurrentPaging()
        {
            return this;
        }
    }
}
