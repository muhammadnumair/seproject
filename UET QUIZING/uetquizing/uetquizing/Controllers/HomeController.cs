using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace uetquizing.Controllers
{
    public class HomeController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();

        [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
        public ActionResult Index()
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            if (user != null)
            {
                var role = user.userRole;
                if (role == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                // END VALIDATIONS
            }
            return View();
        }

        [HttpPost]
        public ActionResult OpenQuiz()
        {
            // FOR VALIDATION
            var userid = User.Identity.GetUserId();
            var user = db.AspNetUsers.Where(x => x.Id == userid).SingleOrDefault();
            if (user != null)
            {
                var role = user.userRole;
                if (role == "Teacher")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                // END VALIDATIONS
            }

            var quiz_code = Request["quiz_code"];
            return RedirectToAction("Quiz/"+quiz_code, "User");
        }
    }
}