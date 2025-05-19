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
            var currentUserId = User.Identity.GetUserId();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Wishlist = db.Wishlist
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Category = db.Category.ToList(),
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Shop(int id, string categoryName)
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserId = User.Identity.GetUserId();
            ViewBag.CategoryName = categoryName;
            var viewModel = new ConnectAllTables
            {
                Product = db.Product
                    .Where(item => item.CategoryId == id)
                    .Include(item => item.Category)
                    .ToList(),
                Wishlist = db.Wishlist
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Category = db.Category.ToList(),
            };
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Product(int id)
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserId = User.Identity.GetUserId();
            var product = db.Product.Find(id);

            var viewModel = new ConnectAllTables
            {
                Product = new List<Product> { product },
                Category = db.Category.ToList(),
                Wishlist = db.Wishlist
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList()
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
                Wishlist = db.Wishlist
                    .Where(item => item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList(),
                Category = db.Category.ToList(),
                Cart = db.Cart
                    .Where(item=> item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList()
            };

            var cartItems = db.Cart
                .Where(item => item.UserId == currentUserID)
                .Include(item => item.Product)
                .ToList();

            ViewBag.DisableButton = !cartItems.Any();

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
                    .ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList()
            };
            return View(viewModel);
        }


        [HttpGet]
        public ActionResult Contact()
        {
            DyplomEntities db = new DyplomEntities();
            var currentUserId = User.Identity.GetUserId();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Wishlist = db.Wishlist
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserId)
                    .Include(item => item.Product)
                    .ToList(),
                Contact = new List<Contact>
                {
                    new Contact
                    {

                    }
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            DyplomEntities db = new DyplomEntities();
            var viewModel = new ConnectAllTables
            {
                Product = db.Product.ToList(),
                Category = db.Category.ToList(),
                Cart = db.Cart.ToList(),
                Contact = new List<Contact> { contact }
            };
            if (ModelState.IsValid)
            {
                contact.CreatedDate = DateTime.Now;
                db.Contact.Add(contact);
                db.SaveChanges();
                return RedirectToAction("Contact");
            }
            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public ActionResult Order() 
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
                    .ToList(),
                Cart = db.Cart
                    .Where(item => item.UserId == currentUserID)
                    .Include(item => item.Product)
                    .ToList(),
                Orders = new List<Orders> 
                {
                    new Orders
                    {
                        
                    }
                }
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Order(Orders orders)
        {
            if (!ModelState.IsValid)
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
                    Orders = new List<Orders> { orders }
                };
                return View(viewModel);
            }

            DyplomEntities dbContext = new DyplomEntities();
            var currentUserIDSave = User.Identity.GetUserId();
            var cartItems = dbContext.Cart
                .Where(item => item.UserId == currentUserIDSave)
                .Include(item => item.Product)
                .ToList();

            foreach (var item in cartItems)
            {
                var newOrder = new Orders
                {
                    ProductId = item.ProductId,
                    Quantity = 1,
                    UserId = currentUserIDSave,
                    OrderDate = DateTime.Now,
                    City = orders.City,
                    DeliveryMethod = orders.DeliveryMethod,
                    DepartmentNumber = orders.DepartmentNumber,
                    Email = orders.Email,
                    Phone = orders.Phone,
                    PaymentMethod = orders.PaymentMethod,
                    Status = "Обробляється",
                    IsCanceled = false
                };

                dbContext.Orders.Add(newOrder);
                dbContext.Cart.Remove(item);
                dbContext.SaveChanges();
            }

            return RedirectToAction("Index", "User");
        }
    }
}