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

        [HttpGet]
        public ActionResult Shop(int id)
        {
            DyplomEntities db = new DyplomEntities();
            //var category = db.Category.Find(id);
            var viewModel = new ConnectAllTables
            {
                Product = db.Product
                    .Where(item => item.CategoryId == id)
                    .Include(item => item.Category)
                    .ToList(),
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
            ViewBag.Id = id;
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
        public ActionResult AddToCart(int productId, Cart cart, bool toWishlist)
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserId = User.Identity.GetUserId();

            cart.UserId = currentUserId;
            cart.ProductId = productId;
            cart.Quantity = 1;
            cart.CreatedDate = DateTime.Now;

            db.Cart.Add(cart);
            db.SaveChanges();
            if (toWishlist)
            {
                return RedirectToAction("Wishlist");
            }
            else
            {
                return RedirectToAction("Product", new { id = productId });
            }
        }

        public ActionResult DeleteFromCart(int cartId)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var cart = db.Cart.Find(cartId);
                if (cart != null)
                {
                    db.Cart.Remove(cart);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Cart");
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

        [Authorize]
        public ActionResult AddToWishlist(int productId, Wishlist wishlist)
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserId = User.Identity.GetUserId();

            wishlist.UserId = currentUserId;
            wishlist.ProductId = productId;
            wishlist.CreatedDate = DateTime.Now;

            db.Wishlist.Add(wishlist);
            db.SaveChanges();

            return RedirectToAction("Product", new { id = productId });
        }
        public ActionResult DeleteFromWishlist(int wishlistId)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var wishlist = db.Wishlist.Find(wishlistId);
                if (wishlist != null)
                {
                    db.Wishlist.Remove(wishlist);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Wishlist");
        }

        [Authorize]
        public ActionResult Wishlist()
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserID = User.Identity.GetUserId();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Wishlist = db.Wishlist
                    .Where(item => item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList()
            };
            return View(viewModel);
        }

        //[Authorize]
        //public ActionResult Checkout()
        //{
        //    DyplomEntities db = new DyplomEntities();
        //    var currentUserID = User.Identity.GetUserId();
        //    var viewModel = new ConnectAllTables
        //    {
        //        Product = db.Product.ToList(),
        //        Category = db.Category.ToList(),
        //        Cart = db.Cart
        //            .Where(item => item.UserId == currentUserID)
        //            .Include(item => item.Product)
        //            .ToList(),
        //        Payment = db.Payment.ToList()
        //    };
        //    return View(viewModel);
        //}

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

        [HttpGet]
        [Authorize]
        public ActionResult Order() 
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Order(Orders orders)
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserID = User.Identity.GetUserId();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList(),
                Orders = db.Orders
            };
            foreach (var item in viewModel.Cart)
            {
                orders.ProductId = item.ProductId;
                orders.Quantity = 1;
                orders.UserId = currentUserID;
                orders.OrderDate = DateTime.Now;
                db.Orders.Add(orders);
                db.Cart.Remove(item);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "User");
        }

        //[HttpPost]
        //public ActionResult OrderDataSave(Orders orders, string email, string phoneNumber, string city, string deliveryMethod, string branchNumber, string paymentMethod) //Add payment verify
        //{
        //    DyplomEntities db = new DyplomEntities();
        //    var currentUserID = User.Identity.GetUserId();
        //    var viewModel = new ConnectAllTables
        //    {
        //        Cart = db.Cart
        //            .Where(item => item.UserId == currentUserID)
        //            .Include(item => item.Product)
        //            .ToList(),
        //        Orders = db.Orders
        //        //Payment = db.Payment.ToList()
        //    };
        //    foreach (var item in viewModel.Cart)
        //    {
        //        //orders.City = ;
        //        db.Cart.Remove(item);
        //        db.SaveChanges();
        //    }
        //    return RedirectToAction("Cart");
        //}
    }
}