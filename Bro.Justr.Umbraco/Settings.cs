using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bro.Justr.Umbraco
{
    public static class Settings
    {
        /// <summary>
        /// Umbraco settings
        /// </summary>
        public static class Umbraco
        {
            /// <summary>
            /// Property alias for the Content's Url name.
            /// </summary>
            public static string UmbracoUrlNameProperty = "umbracoUrlName";

            /// <summary>
            /// Property alias for the Content's Url alias.
            /// </summary>
            public static string UmracoUrlAliasProperty = "umbracoUrlAlias";
        }
        
        /// <summary>
        /// Justr project settings
        /// </summary>
        public static class Justr {
            
            /// <summary>
            /// Property alias for the Content's Url name for multilingual setup (Vorto package).
            /// </summary>
            public static string PageUrlSegmentProperty = "pageName";//vortoPageUrlSegment

            /// <summary>
            /// ID of Documnent Type "Product"
            /// </summary>
            public static int ProductDocumentTypeID = 2113;

            /// <summary>
            /// Default culture of the site
            /// </summary>
            public static CultureInfo DefaultCulture = new CultureInfo("uk-UA");

            /// <summary>
            /// Second (alternate) culture of the site
            /// </summary>
            public static CultureInfo SecondCulture = new CultureInfo("ru-RU");

        }

        
    }
}
