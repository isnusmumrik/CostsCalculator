using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using CostsCalculator.Models.Abstract;
using CostsCalculator.Models.ServieceModels;

namespace CostsCalculator.Models.Concrete
{
    public class PurchasesRepository:IPurchasesRepositiry
    {
        private PurchasesContext context = new PurchasesContext();
        public IQueryable<User> Users => context.Users;
        public IQueryable<Category> Categories => context.Categories;

        public IQueryable<Purchase>Purchases => context.Purchases;

        #region Category
        public Category DeleteCategory(int categoryId)
        {
            Category dbEntryCategory = context.Categories.Find(categoryId);

            if (dbEntryCategory != null)
            {                
                context.Categories.Remove(dbEntryCategory);
                context.SaveChanges();
            }
            return dbEntryCategory;
        }

        public void SaveCategory(Category category)
        {           
            if (category.Id == 0)
            {
                context.Categories.Add(category);
            }
            else
            {
                Category dbEntryCategory = context.Categories.Find(category.Id);
                if (dbEntryCategory != null)
                {
                    context.Categories.Attach(dbEntryCategory);
                    dbEntryCategory.Name = category.Name;
                    dbEntryCategory.ColorForDiagram = category.ColorForDiagram;
                }
            }
            context.SaveChanges();
        }
        #endregion

        #region Purchase
        public Purchase DeletePurchase(int purchaseId)
        {
            Purchase dbEntryPurchase = context.Purchases.Find(purchaseId);       

            if (dbEntryPurchase != null)
            {
                context.Purchases.Remove(dbEntryPurchase);
                context.SaveChanges();
            }
            return dbEntryPurchase;
        }

        public void SavePurchase(Purchase purchase)
        {
            if (purchase.Id == 0)
            {
                context.Purchases.Add(purchase);
            }
            else
            {
                Purchase dbEnterPurchase = context.Purchases.Find(purchase.Id);
                if (dbEnterPurchase != null)
                {
                    context.Purchases.Attach(dbEnterPurchase);
                    dbEnterPurchase.CategoryId = purchase.CategoryId;
                    dbEnterPurchase.Amount = purchase.Amount;
                    dbEnterPurchase.Date = purchase.Date;
                    dbEnterPurchase.Name = purchase.Name;
                    dbEnterPurchase.Unit = purchase.Unit;
                    dbEnterPurchase.UnitCost = purchase.UnitCost;
                }
            }
            context.SaveChanges();
        }
        #endregion

        #region User

        public void SaveUser(User user)
        {
            context.Users.Add(user);
            context.SaveChanges();
        }

        public void ModifyUser(User user)
        {
            User dbEnryUser = context.Users.Find(user.Id);
            if (dbEnryUser != null)
            {
                context.Users.Attach(dbEnryUser);
                dbEnryUser.Password = user.Password;
                context.SaveChanges();
            }
        }

        #endregion
    }
}