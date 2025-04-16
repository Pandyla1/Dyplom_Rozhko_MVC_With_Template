using Dyplom_Rozhko_MVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dyplom_Rozhko_MVC.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            DyplomEntities db = new DyplomEntities();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
            };
            return View(viewModel);
        }

        public ActionResult Shop()
        {
            DyplomEntities db = new DyplomEntities();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Product(int id)
        {
            DyplomEntities db = new DyplomEntities();
            var product = db.Product.Find(id);
            var viewModel = new ConnectAllTables
            {
                Product = new List<Product> { product },
                Category = db.Category.ToList(),
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Product(Product product)
        {
            DyplomEntities db = new DyplomEntities();
            var existingProduct = db.Product.Find((object)product.ProductId);

            if (ModelState.IsValid)
            {
                existingProduct.ProductName = product.ProductName;
                existingProduct.ShortDescription = product.ShortDescription;
                existingProduct.Price = product.Price;
                existingProduct.Quantity = product.Quantity;
                existingProduct.Size = product.Size;
                existingProduct.Color = product.Color;
                existingProduct.CompanyName = product.CompanyName;
                existingProduct.IsActive = product.IsActive;
                existingProduct.ImageUrl = product.ImageUrl;

                db.Entry(existingProduct).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Product", "User");
            }
            return View(product);
        }

        [Authorize]
        public ActionResult Cart()
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserID = User.Identity.GetUserId();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Cart = db.Cart
                    .Where(item=> item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList()
            };
            return View(viewModel);
        }

        public ActionResult Checkout()
        {
            DyplomEntities db = new DyplomEntities();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Cart = db.Cart.ToList(),
                Payment = db.Payment.ToList()
            };
            return View(viewModel);
        }

        public ActionResult Contact()
        {
            DyplomEntities db = new DyplomEntities();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Contact = db.Contact.ToList()
            };
            return View(viewModel);
        }
    }
}