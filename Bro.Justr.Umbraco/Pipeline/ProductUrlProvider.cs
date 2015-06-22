using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClientDependency.Core;
using Umbraco.Core;
using Umbraco.Web;
using Umbraco.Web.Routing;

namespace Bro.Justr.Umbraco.Pipeline
{
    /// <summary>
    /// Umbraco Url Provider for product pages
    /// </summary>
    public class ProductUrlProvider : IUrlProvider
    {
        private readonly IUrlProvider _urlProvider = new DefaultUrlProvider();

        public IEnumerable<string> GetOtherUrls(UmbracoContext umbracoContext, int id, Uri current)
        {
            var content = umbracoContext.ContentCache.GetById(id);
            if (content != null && content.DocumentTypeAlias == "Product" && content.Parent != null)
            {
                //return Enumerable.Empty<string>();
            }
            return _urlProvider.GetOtherUrls(umbracoContext, id, current);
        }

        public string GetUrl(UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
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

                //TODO implement chaching of url if the performance is low

                string languageCode = GetLanguageTwoSymbolCode(umbracoContext.PublishedContentRequest);
                
                string rootNode = languageCode == "ru" ? "прокат-аренда" : "прокат-oренда";
                
                string relativeUrl = "/" + string.Join("/", new string[] { languageCode, rootNode, content.UrlName} ) + "/";

                Uri absoluteUri = new Uri(relativeUrl, UriKind.Relative).MakeAbsoluteUri(umbracoContext.HttpContext);

                if (mode == UrlProviderMode.Absolute)
                {
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
            if (publishedContentRequest != null && publishedContentRequest.Culture != null && publishedContentRequest.Culture.Name == "ru-RU")
            {
                return "ru";
            }
            return "ua";
        }
    }
}

//LogHelper.Error(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType, "msg", ex);