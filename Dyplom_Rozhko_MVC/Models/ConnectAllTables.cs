using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Dyplom_Rozhko_MVC.Models
{
	public class ConnectAllTables
	{
        public IEnumerable<Dyplom_Rozhko_MVC.Category> Category { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Product> Product { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Cart> Cart { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Contact> Contact { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Orders> Orders { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Payment> Payment { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Review> Review { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Roles> Roles { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.SubCategory> SubCategory { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Users> Users { get; set; }
        public IEnumerable<Dyplom_Rozhko_MVC.Wishlist> Wishlist { get; set; }

        public LoginViewModel LoginViewModel { get; set; }
    }
}