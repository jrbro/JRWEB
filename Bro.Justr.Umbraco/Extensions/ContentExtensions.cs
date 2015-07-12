using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Our.Umbraco.Vorto.Extensions;

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


        public static string GetUrl(this IContent content, CultureInfo culture)
        {
            if (content.Id > 0)
            {
                return GetLocalizedUrlFromOtherUrls(content.Id, culture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Gets localized relative URL for the content (used in templates to build links)
        /// </summary>
        /// <param name="content"></param>
        /// <param name="culture"></param>
        /// <returns>localized relative URL</returns>
        public static string GetUrl(this IPublishedContent content, CultureInfo culture)
        {
            if (content.Id > 0)
            {
                if (culture.Equals(Settings.Justr.DefaultCulture))
                {
                    return content.Url;
                }
                else
                {
                    string pageUrl = CacheHelper.GetPageUrl(content.Id, culture.GetTwoLetterLangCode());
                    if (!string.IsNullOrWhiteSpace(pageUrl))
                    {
                        return pageUrl;
                    }
                    else
                    {
                        return GetLocalizedUrlFromOtherUrls(content.Id, culture);
                    }
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Get localized page name (if exists), otherwise reqular name
        /// </summary>
        /// <param name="content">content</param>
        /// <returns>page name</returns>
        public static object GetPageName(this IPublishedContent content)
        {
            return content.GetVortoValue("pageName") ?? content.Name;
        }

        private static string GetLocalizedUrlFromOtherUrls(int contentId, CultureInfo culture)
        {
            string lang = culture.GetTwoLetterLangCode();
            foreach (var urlProvider in UrlProviders)
            {
                var urls = urlProvider.GetOtherUrls(UmbracoContext.Current, contentId, UmbracoContext.Current.HttpContext.Request.Url);

                string url = urls != null ? urls.FirstOrDefault(u => u.StartsWith("/" + lang)) : null; //get first url which starts with /lang

                if (!string.IsNullOrWhiteSpace(url))
                    return url; //return first not empty url
            }
            return string.Empty;
        }


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

        /// <summary>
        /// Gets the url segment for a specified content and culture.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The url segment.</returns>
        public static string GetUrlSegment(this IContent content, CultureInfo culture)
        {
            return GetUrlSegment((IContentBase)content, culture);
        }
    }
}
