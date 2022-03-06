using System.Linq;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Entities.Contact;
using MarketPlace.DataLayer.Entities.Site;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.DataLayer.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        #region site

        public DbSet<SiteSetting> SiteSettings { get; set; }

        #endregion
        #region account

        public DbSet<User> Users { get; set; }

        #endregion
        #region contact
        public DbSet<ContactUs> ContactUs { get; set; }
        #endregion
        #region config
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(x => x.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Cascade;
            }
            base.OnModelCreating(modelBuilder);
        }
    }
    #endregion
}
