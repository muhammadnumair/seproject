using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Web.Mvc;
using uetquizing.Models;

namespace uetquizing.Controllers
{
    public class DashboardController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: Dashboard
        public ActionResult Index()
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            var role = user.userRole;
            if (role == "Student")
            {
                return RedirectToAction("Index", "User");
            }
            // END VALIDATIONS

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }
    }
}