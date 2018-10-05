using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace ShoppingCartApp.Presentation
{
    public static class StringFormatting
    {
        #region Satic Methods
        /// <summary>
        /// Formats decimals into US Currency string
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public static string FormattedPrice(decimal price)
        {
            string formattedPrice = String.Format(CultureInfo.GetCultureInfo(1033), "{0:C}", price);

            return formattedPrice;
        } 
        #endregion
    }
}