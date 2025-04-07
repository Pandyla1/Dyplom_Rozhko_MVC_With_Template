using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dyplom_Rozhko_MVC.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryImageUrl { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}