using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dyplom_Rozhko_MVC.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult OrderList()
        {
            DyplomEntities db = new DyplomEntities();
            return View(db.Orders.ToList());
        }

        [HttpGet]
        public ActionResult OrderEdit(int id)
        {
            DyplomEntities db = new DyplomEntities();
            var orders = db.Orders.Find(id);
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OrderEdit(Orders order)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var existingOrder = db.Orders.Find(order.OrderDetailsId);
                existingOrder.Status = order.Status;
                existingOrder.IsCanceled = order.IsCanceled;
                existingOrder.City = order.City;
                existingOrder.DeliveryMethod = order.DeliveryMethod;
                existingOrder.DepartmentNumber = order.DepartmentNumber;
                existingOrder.Email = order.Email;
                existingOrder.Phone = order.Phone;
                existingOrder.PaymentMethod = order.PaymentMethod;
                existingOrder.PaymentStatus = order.PaymentStatus;

                db.Entry(existingOrder).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("OrderList", "Order");

            }
        }

        [HttpGet]
        public ActionResult OrderDelete(int id)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var orders = db.Orders.Find(id);
                if (orders != null)
                {
                    db.Orders.Remove(orders);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("OrderList");
        }
    }
}