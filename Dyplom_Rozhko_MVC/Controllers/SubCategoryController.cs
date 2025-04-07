using Dyplom_Rozhko_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dyplom_Rozhko_MVC.Controllers
{
    public class SubCategoryController : Controller
    {
        public ActionResult SubCategoryList()
        {
            DyplomEntities subCategories = new DyplomEntities();
            return View(subCategories.SubCategory.ToList());
        }

        [HttpGet]
        public ActionResult SubCategoryCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SubCategoryCreate(SubCategory subCategory)
        {
            DyplomEntities db = new DyplomEntities();
            subCategory.CreatedDate = DateTime.Now;
            db.SubCategory.Add(subCategory);
            db.SaveChanges();
            return RedirectToAction("SubCategoryCreate");
        }

        [HttpGet]
        public ActionResult SubCategoryEdit(int id)
        {
            DyplomEntities db = new DyplomEntities();
            var subCategory = db.SubCategory.Find(id);
            return View(subCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubCategoryEdit(SubCategory subCategories)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var existingSubCategory = db.SubCategory.Find(subCategories.SubCategoryId);
                existingSubCategory.SubCategoryName = subCategories.SubCategoryName;
                existingSubCategory.CategoryId = subCategories.CategoryId;
                existingSubCategory.IsActive = subCategories.IsActive;
                existingSubCategory.CreatedDate = DateTime.Now;

                db.Entry(existingSubCategory).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("SubCategoryList", "SubCategory");

            }
        }

        [HttpGet]
        public ActionResult SubCategoryDelete(int id)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var subCategory = db.SubCategory.Find(id);
                if (subCategory != null)
                {
                    db.SubCategory.Remove(subCategory);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SubCategoryList");
        }
    }
}