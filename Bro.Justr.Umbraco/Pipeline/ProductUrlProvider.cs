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
            return GetUrl(umbracoContext, id, current, mode,
                GetLanguageTwoSymbolCode(umbracoContext.PublishedContentRequest));
        }

        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode, string languageCode)
        {
            #warning TODO implement chaching of url if the performance is low

            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == "Product" && content.Parent != null)
            {
                /*var cachedUrls = (Dictionary<int, Dictionary<string, string>>)umbracoContext.HttpContext.Cache["CachedURLs"];
                if (cachedUrls != null)
                {
                    if (cachedUrls.ContainsKey(id))
                    {
                        var cacheUrlItem = cachedUrls[id];
                        //return the url for the current languahe
                        return true;
                    }
                    else {
                        //add to the cache
                    }
                }*/

                //string languageCode = GetLanguageTwoSymbolCode(umbracoContext.PublishedContentRequest);
                
                string rootNode = languageCode == Settings.Justr.SecondCulture.TwoLetterISOLanguageName ? "прокат-аренда" : "прокат-oренда";

                string urlSegment = content.UrlName;
                if (languageCode == Settings.Justr.SecondCulture.TwoLetterISOLanguageName)
                {
                    #warning TODO optimize that - use cache!
                    var dbContent = umbracoContext.Application.Services.ContentService.GetById(id);
                    urlSegment = dbContent.GetUrlSegment(Settings.Justr.SecondCulture) ?? content.UrlName; //be safe
                }

                string relativeUrl = "/" + string.Join("/", new string[] { languageCode, rootNode, urlSegment }) + "/";

                if (mode == UrlProviderMode.Absolute)
                {
                    Uri absoluteUri = new Uri(relativeUrl, UriKind.Relative).MakeAbsoluteUri(umbracoContext.HttpContext);
                    return absoluteUri.AbsoluteUri;
                }

                return relativeUrl;
            }

            return null;

            /* https://our.umbraco.org/documentation/Reference/Request-Pipeline/outbound-pipeline
             It's tricky to implement your own provider, it is advised use override the default provider. If implementing a custom Url Provider, consider following things:
                * cache things,
                * be sure to know how to handle schema's (http vs https) and hostnames
                * inbound might require rewriting
            */
        }

        private string GetLanguageTwoSymbolCode(PublishedContentRequest publishedContentRequest)
        {
            if (publishedContentRequest != null && publishedContentRequest.Culture != null && publishedContentRequest.Culture.Name == Settings.Justr.SecondCulture.Name)
            {
                return Settings.Justr.SecondCulture.TwoLetterISOLanguageName;
            }
            return "ua";
        }
    }
}

//LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "msg", ex);