using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            var categories = repository.Categories.Where(x=>x.UserId == userId).ToList();

            return View(categories);
        }

        [HttpGet]
        public ActionResult Edit(int categoryId)
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            Category category = repository.Categories.FirstOrDefault(p => p.Id == categoryId && p.UserId == userId);
            return View(category);
        }

        [HttpGet]
        public ActionResult Delete(int categoryId)
        {
            if (ModelState.IsValid)
            {
                Category deleteCategory = repository.DeleteCategory(categoryId);
            }
           
            return RedirectToAction("GetAllCategories");
        }

        [HttpGet]
        public ActionResult Create()
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            Category category = new Category { UserId = userId};

            return View("Edit", category);
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