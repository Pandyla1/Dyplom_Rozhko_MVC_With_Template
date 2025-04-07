using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dyplom_Rozhko_MVC.Models
{
	public class FileUpload
	{
        public IFormFile FormFile { get; set; }
    }
}