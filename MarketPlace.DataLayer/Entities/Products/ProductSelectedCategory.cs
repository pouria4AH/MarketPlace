using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.DataLayer.Entities.Common;

namespace MarketPlace.DataLayer.Entities.Products
{
    public class ProductSelectedCategory :BaseEntity
    {
        #region prop
        public long ProductId { get; set; }
        public long ProductCategoryId { get; set; }
        #endregion

        #region relation

        public Product Product { get; set; }
        public ProductCategory ProductCategory { get; set; }

        #endregion

    }
}
