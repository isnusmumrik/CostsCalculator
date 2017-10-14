using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CostsCalculator.Models
{
    public class PurchasesContext: DbContext
    {
        public PurchasesContext() :base ("DbConnection")
        { }

        public static PurchasesContext Create()
        {
            return new PurchasesContext();
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

    }

}