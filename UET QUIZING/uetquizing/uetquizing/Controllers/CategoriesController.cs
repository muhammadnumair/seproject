using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using uetquizing.Models;

namespace uetquizing.Controllers
{
    public class CategoriesController : Controller
    {
        uetquizingEntities db = new uetquizingEntities();
        // GET: Categories
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            var userId = User.Identity.GetUserId();
            var categories = db.categories.Where(x => x.teacher_id == userId).ToList();
            return View(categories);
        }

        // CATEGORIES
        public ActionResult Add()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add(CategoryViewModel collection)
        {
            if (collection.categoryName != "")
            {
                category c = new category();

                c.category_name = collection.categoryName;
                c.teacher_id = User.Identity.GetUserId();
                c.created_on = DateTime.Now;
                db.categories.Add(c);
                int result = db.SaveChanges();
                if (result > 0)
                {
                    TempData["Success"] = "Record Added Successfully";
                }
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            try
            {
                var category = db.categories.Where(x => x.category_id == id).SingleOrDefault();
                var result = db.categories.Remove(category);
                db.SaveChanges();
                if (result != null)
                {
                    TempData["Success"] = "Record Deleted Successfully";
                }
                else
                {
                    TempData["Error"] = "Something Went Wrong, Try Again";
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int? id)
        {
            try
            {
                var cat = db.categories.Where(x => x.category_id == id).SingleOrDefault();
                CategoryViewModel category = new CategoryViewModel();
                category.categoryId = cat.category_id;
                category.categoryName = cat.category_name;
                ViewBag.Type = "Edit";
                return View("Add", category);
            }
            catch (Exception e)
            {
                TempData["Error"] = "Please Enter Valid Id";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id, CategoryViewModel collection)
        {
            try
            {
                var category = db.categories.Where(x => x.category_id == id).SingleOrDefault();
                category.category_name = collection.categoryName;
                var result = db.SaveChanges();
                if (result > 0)
                {
                    TempData["Success"] = "Record Updated Successfully";
                }
                else
                {
                    TempData["Error"] = "Something Went Wrong, Try Again";
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "Something Went Wrong, Try Again";
            }
            return RedirectToAction("Index");
        }
    }
}