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