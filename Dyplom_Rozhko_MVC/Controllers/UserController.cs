using Dyplom_Rozhko_MVC.Models;
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

        //[HttpPost]
        ////[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddToCart(int productId)
        //{
        //    DyplomEntities db = new DyplomEntities();
        //    var product = db.Product.Find(productId);

        //    if (product == null)
        //        return HttpNotFound();

        //    // Отримуємо поточний кошик із сесії
        //    List<Product> cart = Session["Cart"] as List<Product> ?? new List<Product>();

        //    var existingItem = cart.FirstOrDefault(p => p.ProductId == product.ProductId);
        //    if (existingItem != null)
        //    {
        //        existingItem.Quantity += 1;
        //    }
        //    else
        //    {
        //        cart.Add(new Product
        //        {
        //            ProductId = product.ProductId,
        //            ProductName = product.ProductName,
        //            ImageUrl = product.ImageUrl,
        //            Price = product.Price,
        //            Quantity = 1
        //        });
        //    }

        //    // Оновлюємо сесію
        //    Session["Cart"] = cart;

        //    // Повернення назад
        //    return RedirectToAction("Product", new { id = productId });
        //}

        [Authorize]
        public ActionResult Cart()
        {
            DyplomEntities db = new DyplomEntities();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Cart = db.Cart.ToList()
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