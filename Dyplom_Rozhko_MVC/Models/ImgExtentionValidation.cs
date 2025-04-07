using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Dyplom_Rozhko_MVC.Models
{
	public class ImgExtentionValidation
	{
        public static bool IsValidExtension(string fileName)
        {
            bool isValid = false;
            string[] fileExension = { ".jpg", ".png", ".jpeg" };

            foreach (string file in fileExension)
            {
                if (fileName.Contains(file))
                {
                    isValid = true;
                    break;
                }
            }
            return isValid;
        }
    }
}