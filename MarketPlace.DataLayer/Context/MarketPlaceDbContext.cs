using System.Linq;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Contact;
using MarketPlace.DataLayer.Entities.Products;
using MarketPlace.DataLayer.Entities.Site;
using MarketPlace.DataLayer.Entities.Store;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.DataLayer.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        #region site

        public DbSet<SiteSetting> SiteSettings { get; set; }

        #endregion
        #region slider

        public DbSet<Slider> Sliders { get; set; }

        #endregion
        #region account

        public DbSet<User> Users { get; set; }

        #endregion
        #region contact
        public DbSet<ContactUs> ContactUs { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketMessage> TicketMessages { get; set; }
        #endregion
        #region Stor
        public DbSet<Seller> Sellers { get; set; }
        #endregion
        #region site banner
        public DbSet<SiteBanner> SiteBanners { get; set; }
        #endregion
        #region Product

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }
        #endregion
        #region config
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
    #endregion
}
