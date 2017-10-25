using System.Linq;
using System.Web.Mvc;
using CostsCalculator.Models;
using CostsCalculator.Models.Abstract;

namespace CostsCalculator.Controllers
{
    [Authorize]
    public class CategoryController : Controller
    {
        private IPurchasesRepositiry repository;

        public CategoryController(IPurchasesRepositiry r)
        {
            repository = r;
        }
        [HttpGet]
        public ActionResult GetAllCategories()
        {
            User currUser = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name);
            if(currUser != null)
            {
                int userId =currUser .Id;

                var categories = repository.Categories.Where(x => x.UserId == userId).ToList();

                return View(categories);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Edit(int categoryId)
        {
            User currUser = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name);
            if(currUser != null)
            {
                int userId =currUser .Id;

                Category category = repository.Categories.FirstOrDefault(p => p.Id == categoryId && p.UserId == userId);
                return View(category);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public ActionResult Delete(int categoryId)
        {
            if (ModelState.IsValid)
            {
                repository.DeleteCategory(categoryId);
            }
           
            return RedirectToAction("GetAllCategories");
        }

        [HttpGet]
        public ActionResult Create()
        {
            User currUser = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

            if (currUser != null)
            {
                int userId = currUser.Id;

                Category category = new Category {UserId = userId};

                return View("Edit", category);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {

                repository.SaveCategory(category);
                return RedirectToAction("GetAllCategories");
            }

            return View("Edit");
        }
    }
}