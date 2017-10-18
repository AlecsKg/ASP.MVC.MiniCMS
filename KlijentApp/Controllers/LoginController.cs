using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using KlijentApp.Models;

namespace KlijentApp.Controllers
   
{
    

    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login( SystemUser user)
        {
            
           if (ModelState.IsValid)
                {
                    using (ModelEF db = new ModelEF())
                    {
                        var obj = db.SystemUsers.FirstOrDefault(a => a.UserName.Equals(user.UserName) && a.Password.Equals(user.Password));
                        if (obj != null)
                        {
                            Session["UserID"] = obj.UserId.ToString();
                            Session["UserName"] = obj.UserName;
                            obj.LastLogin = DateTime.Now;
                            db.SaveChanges();
                        return RedirectToAction("Index", "Home");
                        }
                        TempData["msg"] = Prenosna.Poruka(PrevodSrb.Login_podaci_su_netačni_);

                    }
                }
                return View(user);
            }
      
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session["UserName"] = "";
            Session["UserId"] = "";
            return RedirectToAction("Index", "Home");
        }
    }
}