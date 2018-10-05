using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.IO;

// Custom Validation Attribute to validate that file upload is an image.

namespace ShoppingCartApp.Presentation
{
    public class ImageExtensionAttribute : ValidationAttribute
    {
        #region Public Methods
        /// <summary>
        /// Returns true if file extension is of allowable format, else false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }
            var fileName = Path.GetExtension(value.ToString());
            if (fileName == ".png" || fileName == ".jpg" || fileName == ".jpeg")
            {
                return true;
            }
            else
            {
                return false;
            }
        } 
        #endregion
    }
}