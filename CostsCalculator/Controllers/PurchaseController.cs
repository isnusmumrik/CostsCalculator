﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CostsCalculator.Models;
using CostsCalculator.Models.Abstract;

namespace CostsCalculator.Controllers
{
    [Authorize]
    [HandleError(ExceptionType = typeof(System.IndexOutOfRangeException), View = "Error")]
    public class PurchaseController : Controller
    {
        private IPurchasesRepositiry repository;
        private int pageSize = 6;      

        public PurchaseController(IPurchasesRepositiry repo)
        {
            repository = repo;           
        }

        [HttpGet]
        public ActionResult Index()
        {           
            return View();
        }

        [HttpGet]
        public ActionResult FirstListPurchases()
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            DateTime dateFromFirst;

            if (repository.Purchases.Any(x=>x.UserId == userId))
                dateFromFirst =  repository.Purchases.Where(p=>p.UserId == userId).Min(x => x.Date);
            else
                dateFromFirst = DateTime.Now;

            return RedirectToAction("GetAllPurchases", new {dateFrom = dateFromFirst.Date, dateTo = DateTime.Now.Date, page = 1,category = 0});

        }

        [HttpGet]
        public ActionResult GetAllPurchases(DateTime dateFrom,DateTime dateTo, int page = 1, int category = 0 )
        {
            IEnumerable<Purchase> purchasesList;
            var categoriesList = repository.Categories.ToList();
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            if (category == 0)
                purchasesList = repository.Purchases.Include(x=>x.Category).Where(x => x.Date >= dateFrom && x.Date <= dateTo && x.UserId == userId).ToList();
            else
                purchasesList = repository.Purchases.Where(x => x.Date >= dateFrom && x.Date <= dateTo && x.CategoryId == category && x.UserId == userId).ToList();

            PurchasesCollection model = new PurchasesCollection
            {
                Purchases = purchasesList.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize),
                FromDate = dateFrom.Date,
                ToDate = dateTo.Date,
                PagingInfo = FillPagingInfo(page, purchasesList.Count())               
            };                           

            SelectList list = ReturnSelectList(categoriesList);
            ViewBag.Categories = list;

            return View(model);
        }

        [HttpGet]
        public ActionResult CalcAllCosts()
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            double allCosts;

            if (repository.Purchases.Any(x => x.UserId == userId))
                allCosts = repository.Purchases.Where(x => x.UserId == userId).Select(x => x.Amount * x.UnitCost).Sum();
            else
                allCosts = 0;

            return PartialView(allCosts);
        }

        [HttpGet]
        public ActionResult GetLatestDateOfCost()
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            DateTime latesTime;

            if (repository.Purchases.Any(x => x.UserId == userId))
            {
                var latestPurchase = repository.Purchases.Where(x => x.UserId == userId)
                    .Include(x => x.Category)
                    .Min(x => x.Date);
                latesTime = latestPurchase.Date;
            }
            else
            {
                latesTime = DateTime.Now;
            }

            return PartialView(latesTime);
        }

        #region MODIFICATION
        [HttpDelete]
        public ActionResult Delete(int purchaseId)
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            if (ModelState.IsValid)
            {
                repository.DeletePurchase(purchaseId);
            }

            DateTime dateFromFirst;
            if(repository.Purchases.Count(x=>x.UserId == userId) >0)
                dateFromFirst = repository.Purchases.Where(x=>x.UserId== userId).Min(x => x.Date);
            else
                dateFromFirst = DateTime.Now;
            return RedirectToAction("GetAllPurchases", new { dateFrom = dateFromFirst.Date, dateTo = DateTime.Now.Date, page = 1, category = 0 });
        }

        [HttpGet]
        public ActionResult Edit(int purchaseId)
        {
            Purchase purchase = repository.Purchases.FirstOrDefault(p => p.Id == purchaseId);          

            SelectList list = ReturnSelectList(purchase.CategoryId);
            ViewBag.Categories = list;

            return View(purchase);
        }

        [HttpPost]
        public ActionResult Edit(Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

                repository.SavePurchase(purchase);
                DateTime dateFromFirst = repository.Purchases.Where(x=>x.UserId == userId).Min(x => x.Date);

                return RedirectToAction("GetAllPurchases",new { dateFrom = dateFromFirst.Date, dateTo = DateTime.Now.Date, page = 1, category = 0 });
            }

            return View("Edit");
        }

        [HttpGet]
        public ViewResult Create()
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            Purchase purchase= new Purchase{UserId = userId,Date = DateTime.Now};

            SelectList list = ReturnSelectList(purchase.CategoryId);
            if (list == null || !list.Any())
                return View("EmptyCategory" );

            ViewBag.Categories = list;

            return View("Edit",purchase);
        }
        #endregion



        #region FILTRATION
        [HttpGet]
        public ActionResult FilterForPeriod(string period)
        {
            DateTime toDate = DateTime.Now;
            DateTime fromDate;
            switch (period)
            {
                case "week":
                {
                    fromDate = toDate.AddDays(-7);
                    break;
                }
                case "month":
                {
                    fromDate = toDate.AddMonths(-1);
                    break;
                }
                case "year":
                {
                    fromDate = toDate.AddYears(-1);
                    break;
                }
                default:
                {
                    fromDate = toDate.AddYears(-1);
                    break;
                }
            }

            return FilterByData(fromDate, toDate);
        }

        [HttpPost]
        public ActionResult FilterByData(DateTime FromDate, DateTime ToDate , int category = 0)
        {
            IEnumerable<Purchase> purchasesList;
            var categories = repository.Categories.ToList();
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            if (category == 0)
                purchasesList = repository.Purchases.Include(x=>x.Category).Where(x => x.Date >= FromDate && x.Date <= ToDate && x.UserId == userId).ToList();
            else
                purchasesList = repository.Purchases.Include(x => x.Category).Where(x => x.Date >= FromDate && x.Date <= ToDate && x.CategoryId == category && x.UserId==userId).ToList();


            SelectList list = ReturnSelectList(categories);
            ViewBag.Categories = list;

            //var latestPurchase = repository.Purchases.Min(x => x.Date);
            //PurchasesCollection purchases = new PurchasesCollection { Purchases = purchasesList, FromDate = latestPurchase.Date, ToDate = DateTime.Now.Date };
            int page = 1;
            PurchasesCollection model = new PurchasesCollection
            {
                Purchases = purchasesList.OrderBy(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize),
                FromDate = FromDate.Date,
                ToDate = ToDate.Date,
                PagingInfo = FillPagingInfo(page,purchasesList.Count())
            };

            return View("GetAllPurchases", model);
        }
        #endregion




        #region Local function

        private SelectList ReturnSelectList(int categoryId )
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;
            var categories = repository.Categories.Where(x=>x.UserId==userId).ToList();

            IEnumerable<SelectListItem> selectList = categories.Select(u => new SelectListItem
                { Text = u.Name, Value = u.Id.ToString(), Selected = u.Id == categoryId });

            SelectList list = new SelectList(selectList, "Value", "Text");

            return list;

        }


        private SelectList ReturnSelectList(List<Category> categoriesList)
        {
            int userId = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name).Id;

            IEnumerable<SelectListItem> selectList = categoriesList.Where(x=>x.UserId==userId).Select(u => new SelectListItem
                { Text = u.Name, Value = u.Id.ToString(), Selected = false });

            List<SelectListItem> dropDownList = new List<SelectListItem>
            {
                new SelectListItem {Text = "All", Value = "0", Selected = true}
            };
            dropDownList.AddRange(selectList.ToList());

            SelectList list = new SelectList(dropDownList, "Value", "Text");

            return list;
        }

        //Fill object for pagging
        private PagingInfo  FillPagingInfo(int page,int totalItems)
        {
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                ItemsPerPage = pageSize,
                TotalItems = totalItems
            };
    
            return pagingInfo;
        }
        #endregion
    }
}