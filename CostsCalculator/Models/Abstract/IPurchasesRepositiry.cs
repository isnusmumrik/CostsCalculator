using System.Linq;


namespace CostsCalculator.Models.Abstract
{
    public interface IPurchasesRepositiry
    {
        IQueryable<User> Users { get; }
        IQueryable<Category> Categories { get; }
        IQueryable<Purchase> Purchases { get; }
        Category DeleteCategory(int categoryId);
        void SaveCategory(Category category);

        Purchase DeletePurchase(int purchaseId);
        void SavePurchase(Purchase purchase);

        void SaveUser(User user);
        void ModifyUser(User user);
    }
}
