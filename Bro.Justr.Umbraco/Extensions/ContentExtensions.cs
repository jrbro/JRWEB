using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;
using Umbraco.Web.Routing;

namespace Bro.Justr.Umbraco.Extensions
{
    public static class ContentExtensions
    {
        /// <summary>
        /// Gets the url providers.
        /// </summary>
        /// <remarks>This is so that unit tests that do not initialize the resolver do not
        /// fail and fall back to defaults. When running the whole Umbraco, CoreBootManager
        /// does initialise the resolver.</remarks>
        private static IEnumerable<IUrlProvider> UrlProviders
        {
            get
            {
                return UrlProviderResolver.HasCurrent
                           ? UrlProviderResolver.Current.Providers
                           : new IUrlProvider[] { new DefaultUrlProvider() };
            }
        }


        public static string GetUrl(this IContent content)
        {
            /*var url = UrlProviders.Select(p => p.GetUrl(content, culture)).First(u => u != null);
            url = url ?? new DefaultUrlProvider().GetUrlSegment(content, culture); // be safe
            return url;*/

            throw new NotImplementedException();
        }

        /*public static string UrlName(this IContent content)
        {
            //((IContentBase)content).get
            return new DefaultUrlSegmentProvider().GetUrlSegment(content).ToLower();
        }*/

        /********************************************** COPY PASTED FROM Umbraco.Core.Strings.ContentBaseExtensions ************************************************/

        /// <summary>
        /// Gets the url segment providers.
        /// </summary>
        /// <remarks>This is so that unit tests that do not initialize the resolver do not
        /// fail and fall back to defaults. When running the whole Umbraco, CoreBootManager
        /// does initialise the resolver.</remarks>
        private static IEnumerable<IUrlSegmentProvider> UrlSegmentProviders
        {
            get
            {
                return UrlSegmentProviderResolver.HasCurrent
                           ? UrlSegmentProviderResolver.Current.Providers
                           : new IUrlSegmentProvider[] { new DefaultUrlSegmentProvider() };
            }
        }

        /// <summary>
        /// Gets the default url segment for a specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The url segment.</returns>
        public static string GetUrlSegment(this IContentBase content)
        {
            var url = UrlSegmentProviders.Select(p => p.GetUrlSegment(content)).First(u => u != null);
            url = url ?? new DefaultUrlSegmentProvider().GetUrlSegment(content); // be safe
            return url;
        }

        /// <summary>
        /// Gets the url segment for a specified content and culture.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The url segment.</returns>
        public static string GetUrlSegment(this IContentBase content, CultureInfo culture)
        {
            var url = UrlSegmentProviders.Select(p => p.GetUrlSegment(content, culture)).First(u => u != null);
            url = url ?? new DefaultUrlSegmentProvider().GetUrlSegment(content, culture); // be safe
            return url;
        }
    }
}
