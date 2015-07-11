using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientDependency.Core;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;
using Bro.Justr.Umbraco.Extensions;
using Umbraco.Core.Models;

/* https://our.umbraco.org/documentation/Reference/Request-Pipeline/outbound-pipeline */

namespace Bro.Justr.Umbraco.Pipeline
{
    /// <summary>
    /// Umbraco Url Provider for product pages
    /// </summary>
    public class ProductUrlProvider : IUrlProvider
    {
        //private readonly IUrlProvider _urlProvider = new BaseUrlProvider();

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            //return _urlProvider.GetOtherUrls(umbracoContext, id, current);

            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == "Product" && content.Parent != null)
            {
                 return new List<string>()
                 {
                     GetUrl(umbracoContext, id, current, UrlProviderMode.Relative, Settings.Justr.SecondCulture.TwoLetterISOLanguageName)
                 };
            }
            return Enumerable.Empty<string>();
        }

        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            return GetUrl(umbracoContext, id, current, mode, GetLanguageTwoSymbolCode(umbracoContext.PublishedContentRequest));
        }

        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode, string languageCode)
        {
            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == "Product" && content.Parent != null)
            {
                if (mode == UrlProviderMode.Absolute)
                {
                    return GetAbsoluteUrl(content, languageCode, umbracoContext);
                }
                else
                {
                    return GetRelativeUrl(content, languageCode, umbracoContext);
                }
            }
            return null;
        }

        #region Private members

        /// <summary>
        /// Gets page relative URL from cache if exist, otherwise builds the URL and updates the cache
        /// </summary>
        /// <param name="content"></param>
        /// <param name="languageCode"></param>
        /// <param name="umbracoContext"></param>
        /// <returns>relative URL</returns>
        private string GetRelativeUrl(IPublishedContent content, string languageCode, UmbracoContext umbracoContext)
        {
            string cachedPageUrl = CacheHelper.GetPageUrl(content.Id, languageCode);
            if (!string.IsNullOrEmpty(cachedPageUrl))
            {
                return cachedPageUrl;
            }
            else
            {
                string relativeUrl = BuildRelativeUrl(content, languageCode, umbracoContext);
                CacheHelper.SetPageUrl(content.Id, languageCode, relativeUrl);
                return relativeUrl;
            }
        }

        private string BuildRelativeUrl(IPublishedContent content, string languageCode, UmbracoContext umbracoContext)
        {
            string rootNode = languageCode == Settings.Justr.SecondCulture.TwoLetterISOLanguageName ? "аренда-и-прокат" : "оренда-та-прокат";

            string urlSegment = content.UrlName;
            if (languageCode == Settings.Justr.SecondCulture.TwoLetterISOLanguageName)
            {
                var dbContent = umbracoContext.Application.Services.ContentService.GetById(content.Id);
                urlSegment = dbContent.GetUrlSegment(Settings.Justr.SecondCulture) ?? content.UrlName; //be safe
            }

            string relativeUrl = "/" + string.Join("/", new string[] { languageCode, rootNode, urlSegment }) + "/";
            return relativeUrl;
        }

        /// <summary>
        /// Gets page absolute URL from cache if exist, otherwise builds the URL and updates the cache
        /// </summary>
        /// <param name="content"></param>
        /// <param name="languageCode"></param>
        /// <param name="umbracoContext"></param>
        /// <returns>relative URL</returns>
        private string GetAbsoluteUrl(IPublishedContent content, string languageCode, UmbracoContext umbracoContext)
        {
            string cachedPageUrl = CacheHelper.GetPageUrl(content.Id, languageCode, true);
            if (!string.IsNullOrEmpty(cachedPageUrl))
            {
                return cachedPageUrl;
            }
            else
            {
                string relativeUrl = GetRelativeUrl(content, languageCode, umbracoContext);
                Uri absoluteUri = new Uri(relativeUrl, UriKind.Relative).MakeAbsoluteUri(umbracoContext.HttpContext);
                CacheHelper.SetPageUrl(content.Id, languageCode, absoluteUri.AbsoluteUri, true);
                return absoluteUri.AbsoluteUri;
            }
        }


        private string GetLanguageTwoSymbolCode(PublishedContentRequest publishedContentRequest)
        {
            if (publishedContentRequest != null && publishedContentRequest.Culture != null && publishedContentRequest.Culture.Name == Settings.Justr.SecondCulture.Name)
            {
                return Settings.Justr.SecondCulture.TwoLetterISOLanguageName;
            }
            return "ua";
        }

        #endregion
    }
}

//LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "msg", ex);