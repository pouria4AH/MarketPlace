using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.DataLayer.DTOs.Paging
{
    public class Pager
    {
        public static BasePaging Build(int pageId, int allEntitiesCount, int take, int howManyPageAfterAndBefore)
        {
            var pageCount = Convert.ToInt32(Math.Ceiling(allEntitiesCount / (double)take));
            return new BasePaging
            {
                PageId = pageId,
                AllEntitiesCount = allEntitiesCount,
                TakeEntities = take,
                HowManyShowPageAfterAndBefore = howManyPageAfterAndBefore,
                SkipEntities = (pageCount - 1) * take,
                StartPage = pageId - howManyPageAfterAndBefore <= 0 ? 1 : pageId - howManyPageAfterAndBefore,
                EndPage = pageId + howManyPageAfterAndBefore > pageCount ? pageCount : pageId + howManyPageAfterAndBefore,
                PageCount = pageCount
            };
        }
    }
}
