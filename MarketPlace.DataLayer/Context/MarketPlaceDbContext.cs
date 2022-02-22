﻿using MarketPlace.DataLayer.Entities.Account;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.DataLayer.Context
{
    public class MarketPlaceDbContext : DbContext
    {
        #region dbsets

        public DbSet<User> Users { get; set; }

        #endregion
    }
}
