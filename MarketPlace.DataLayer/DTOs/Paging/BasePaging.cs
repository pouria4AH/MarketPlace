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
    }
}
