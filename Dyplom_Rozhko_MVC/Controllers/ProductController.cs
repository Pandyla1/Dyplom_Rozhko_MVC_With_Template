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
    public class ProductController : Controller
    {
        public ActionResult ProductList()
        {
            DyplomEntities products = new DyplomEntities();
            return View(products.Product.ToList());
        }

        [HttpGet]
        public ActionResult ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductCreate(Product product, HttpPostedFileBase ImageFile)
        {
            DyplomEntities db = new DyplomEntities();
            if (ModelState.IsValid)
            {
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(ImageFile.FileName);
                    fileName = fileName.Replace(" ", "_");
                    string filePath = Path.Combine(Server.MapPath("~/Images/Product/"), fileName);

                    if (!Directory.Exists(Server.MapPath("~/Images/Product/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Images/Product/"));
                    }

                    ImageFile.SaveAs(filePath);
                    product.ImageUrl = "~/Images/Product/" + fileName;

                    if (!ImgExtentionValidation.IsValidExtension(ImageFile.FileName))
                    {
                        product.ImageUrl = "~/Images/No_image.png";
                    }

                }
                else
                {
                    product.ImageUrl = "~/Images/No_image.png";
                }

                product.CreatedDate = DateTime.Now;
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("ProductCreate");
            }

            return View(product);
        }

        [HttpGet]
        public ActionResult ProductEdit(int id)
        {
            DyplomEntities db = new DyplomEntities();
            var product = db.Product.Find(id);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProductEdit(Product product, HttpPostedFileBase ImageFile)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var existingProduct = db.Product.Find((object)product.ProductId);
                if (existingProduct == null)
                {
                    return HttpNotFound();
                }

                if (ModelState.IsValid)
                {
                    existingProduct.ProductName = product.ProductName;
                    existingProduct.ShortDescription = product.ShortDescription;
                    existingProduct.Price = product.Price;
                    existingProduct.Quantity = product.Quantity;
                    existingProduct.Size = product.Size;
                    existingProduct.Color = product.Color;
                    existingProduct.CompanyName = product.CompanyName;
                    existingProduct.CategoryId = product.CategoryId;
                    existingProduct.SubCategoryId = product.SubCategoryId;
                    existingProduct.Sold = product.Sold;
                    existingProduct.IsActive = product.IsActive;
                    existingProduct.CreatedDate = DateTime.Now;
                    existingProduct.ImageUrl = product.ImageUrl;

                    if (ImageFile != null && ImageFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(ImageFile.FileName);
                        fileName = fileName.Replace(" ", "_");
                        string filePath = Path.Combine(Server.MapPath("~/Images/Product/"), fileName);

                        if (!Directory.Exists(Server.MapPath("~/Images/Product/")))
                        {
                            Directory.CreateDirectory(Server.MapPath("~/Images/Product/"));
                        }

                        ImageFile.SaveAs(filePath);

                        if (ImgExtentionValidation.IsValidExtension(fileName))
                        {
                            existingProduct.ImageUrl = "~/Images/Product/" + fileName + "?v=" + DateTime.Now.Ticks;
                        }
                        else
                        {
                            existingProduct.ImageUrl = "~/Images/No_image.png";
                        }
                    }
                    else
                    {
                        existingProduct.ImageUrl = "~/Images/No_image.png";
                    }

                    db.Entry(existingProduct).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("ProductList", "Product");
                }

                return View(product);
            }
        }

        [HttpGet]
        public ActionResult ProductDelete(int id)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var product = db.Product.Find(id);
                if (product != null)
                {
                    db.Product.Remove(product);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ProductList");
        }
    }
}