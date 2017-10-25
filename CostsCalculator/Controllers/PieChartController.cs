using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using CostsCalculator.Models;
using CostsCalculator.Models.Abstract;

namespace CostsCalculator.Controllers
{
    [Authorize]
    public class PieChartController : Controller
    {
        private IPurchasesRepositiry repository;


        public PieChartController(IPurchasesRepositiry r)
        {
            repository = r;           
        }

        public ActionResult GetDataCategory(string fromDate,string toDate,string categoryId)
        {
            User currUser = repository.Users.Where(x => x.Name == User.Identity.Name).FirstOrDefault();

            if (currUser != null)
            {
                DateTime dateFrom;
                DateTime dateTo;
                int idCategory;
                int userId =currUser .Id;
                DateTime.TryParse(fromDate, out dateFrom);
                DateTime.TryParse(toDate, out dateTo);
                int.TryParse(categoryId, out idCategory);


                var listCategories = repository.Categories.ToList();
                var purchases = repository.Purchases.Where(x => x.UserId == userId).Include(p => p.Category).ToList();

                List<CostsDataPoint> costsData;
                if (idCategory == 0)
                {
                    var dataCategories = listCategories.Select(c => new CostsDataPoint
                    {
                        Name = c.Name,
                        Value = purchases.Where(p => p.CategoryId == c.Id && p.Date >= dateFrom && p.Date <= dateTo)
                            .Sum(x => x.UnitCost * x.Amount),
                        Color = c.ColorForDiagram
                    });
                    costsData = dataCategories.ToList();
                }
                else
                {
                    var dataCategories = listCategories.Select(c => new CostsDataPoint
                    {
                        Name = c.Name,
                        Value = purchases.Where(p => p.CategoryId == c.Id && p.CategoryId == idCategory &&
                                                     p.Date >= dateFrom && p.Date <= dateTo)
                            .Sum(x => x.UnitCost * x.Amount),
                        Color = c.ColorForDiagram
                    });
                    costsData = dataCategories.ToList();
                }

                var json = Json(new {Categories = costsData}, JsonRequestBehavior.AllowGet);

                return json;
            }

            return RedirectToAction("Login", "Account");
        }

    }
}