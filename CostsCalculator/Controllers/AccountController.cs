using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using CostsCalculator.Models;
using CostsCalculator.Models.Abstract;
using CostsCalculator.Models.ServieceModels;
//Test local
namespace CostsCalculator.Controllers
{
    public class AccountController : Controller
    {
        private IPurchasesRepositiry repository;

        public AccountController(IPurchasesRepositiry repo)
        {
            repository = repo;
        }

        // GET: Account
        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LogModel model)
        {
            if (ModelState.IsValid)
            {
                User user = repository.Users.FirstOrDefault(x => x.Name == model.UserName && x.Password == model.Password);
           
                if (user != null)
                {
                     if(User.Identity.IsAuthenticated)
                        FormsAuthentication.SignOut();

                    FormsAuthentication.SetAuthCookie(model.UserName,true);
                    
                    return RedirectToAction("Index", "Purchase");
                }
                ModelState.AddModelError("", "Incorrect username or password");                
            }
            return View();
        }

        [HttpGet]
        public ViewResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User modelUser)
        {
            User user = repository.Users.FirstOrDefault(x => x.Name == modelUser.Name);

            if (user == null)
            {
                repository.SaveUser(modelUser); 

                user = repository.Users.FirstOrDefault(
                    x => x.Name == modelUser.Name && x.Password == modelUser.Password && x.Email == modelUser.Email);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(modelUser.Name, true);
                    return RedirectToAction("Index", "Purchase");
                }
            }
            else
            {
                ModelState.AddModelError("", $"Username {modelUser.Name} already exists");
            }

            return View();
        }

        [HttpGet]
        public ActionResult ModificationUserData()
        {
            User currUser = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name);

            if (currUser != null)
            {
                int userId = currUser.Id;
                UserData modifUser = new UserData();
                modifUser.Id = userId;

                return View(modifUser);
            }

            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ModificationUserData(UserData userData)
        {
            if (ModelState.IsValid)
            {
                User user = new User {Id = userData.Id, Password = userData.NewPassword};
                repository.ModifyUser(user);

                return RedirectToAction("Index", "Purchase");
            }
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult CheckPassword(string OldPassword)
        {
            User userModified = repository.Users.FirstOrDefault(x => x.Name == User.Identity.Name);
            if (userModified != null)
            {
                if (userModified.Password == OldPassword)
                    return Json(true, JsonRequestBehavior.AllowGet);

                return Json("Your old password is incorrect");
            }
            return RedirectToAction("Login","Account");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}
