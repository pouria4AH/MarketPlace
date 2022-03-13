using System.Linq;

namespace MarketPlace.DataLayer.DTOs.Paging
{
    public static class PagingExtension
    {
        public static IQueryable<T> paging<T>(this IQueryable<T> query, BasePaging paging)
        {
            return query.Skip(paging.SkipEntities).Take(paging.TakeEntities);
        }
    }
}
