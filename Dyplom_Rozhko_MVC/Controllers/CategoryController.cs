using Dyplom_Rozhko_MVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Dyplom_Rozhko_MVC.Controllers
{
    public class CategoryController : Controller
    {
        public ActionResult CategoryList()
        {
            DyplomEntities categories = new DyplomEntities();
            return View(categories.Category.ToList());
        }

        [HttpGet]
        public ActionResult CategoryCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryCreate(Category category, HttpPostedFileBase ImageFile)
        {
            DyplomEntities db = new DyplomEntities();
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    fileName = fileName.Replace(" ", "_");
                    string filePath = Path.Combine(Server.MapPath("~/Images/Category/"), fileName);

                    if (!Directory.Exists(Server.MapPath("~/Images/Category/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Images/Category/"));
                    }

                    ImageFile.SaveAs(filePath);
                    category.ImageUrl = "~/Images/Category/" + fileName;

                    if (!ImgExtentionValidation.IsValidExtension(ImageFile.FileName))
                    {
                        category.ImageUrl = "~/Images/No_image.png";
                    }

                }
                else
                {
                    category.ImageUrl = "~/Images/No_image.png";
                }

                category.CreatedDate = DateTime.Now;
                db.Category.Add(category);
                db.SaveChanges();
                return RedirectToAction("CategoryCreate");
            }

            return View(category);
        }

        [HttpGet]
        public ActionResult CategoryEdit(int id)
        {
            DyplomEntities db = new DyplomEntities();
            var category = db.Category.Find(id);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CategoryEdit(Category categories, HttpPostedFileBase ImageFile)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var existingCategory = db.Category.Find((object)categories.CategoryId);
                if (existingCategory == null)
                {
                    return HttpNotFound();
                }

                if (ModelState.IsValid)
                {
                    existingCategory.CategoryName = categories.CategoryName;
                    existingCategory.IsActive = categories.IsActive;
                    existingCategory.ImageUrl = categories.ImageUrl;
                    existingCategory.CreatedDate = DateTime.Now;

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(ImageFile.FileName);
                        fileName = fileName.Replace(" ", "_");
                        string filePath = Path.Combine(Server.MapPath("~/Images/Category/"), fileName);

                        if (!Directory.Exists(Server.MapPath("~/Images/Category/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Images/Category/"));
                        }

                        ImageFile.SaveAs(filePath);

                        if (ImgExtentionValidation.IsValidExtension(fileName))
                        {
                            existingCategory.ImageUrl = "~/Images/Category/" + fileName + "?v=" + DateTime.Now.Ticks;
                        }
                        else
                        {
                            existingCategory.ImageUrl = "~/Images/No_image.png";
                        }
                    }
                    else
                    {
                        existingCategory.ImageUrl = "~/Images/No_image.png";
                    }

                    db.Entry(existingCategory).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("CategoryList", "Category");
                }

                return View(categories);
            }
        }


        [HttpGet]
        public ActionResult CategoryDelete(int id)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var category = db.Category.Find(id);
                if (category != null)
                {
                    db.Category.Remove(category);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("CategoryList");
        }
    }
}