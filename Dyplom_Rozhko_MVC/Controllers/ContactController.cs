using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dyplom_Rozhko_MVC.Controllers
{
    public class ContactController : Controller
    {
        public ActionResult ContactList()
        {
            DyplomEntities db = new DyplomEntities();
            return View(db.Contact.ToList());
        }

        [HttpGet]
        public ActionResult ContactDelete(int id)
        {
            using (DyplomEntities db = new DyplomEntities())
            {
                var contact = db.Contact.Find(id);
                if (contact != null)
                {
                    db.Contact.Remove(contact);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("ContactList");
        }
    }
}