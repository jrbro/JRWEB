using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bro.Justr.Umbraco.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Gets two symbol language code. Used to get 'ua' for uk-UA culture instead of 'uk'.
        /// </summary>
        /// <param name="cultureInfo">culture info</param>
        /// <returns>'ua' for uk-UA, 'ru' for ru-RU etc</returns>
        public static string GetTwoLetterLangCode(this CultureInfo cultureInfo)
        {
            return cultureInfo.Name.Split(new char[] { '-' })[1].ToLower();
        }
    }
}
