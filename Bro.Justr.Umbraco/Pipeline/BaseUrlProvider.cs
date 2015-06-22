using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web.Routing;
using Bro.Justr.Umbraco.Extensions;
using Umbraco.Core.Models;

namespace Bro.Justr.Umbraco.Pipeline
{
    public class BaseUrlProvider : IUrlProvider
    {
        DefaultUrlProvider _urlProvider = new DefaultUrlProvider();

        public IEnumerable<string> GetOtherUrls(global::Umbraco.Web.UmbracoContext umbracoContext, int id, Uri current)
        {
            string russianUrl = BuildFullyLocalizedUrl(umbracoContext, id, new CultureInfo("ru-Ru"));

            var otherUrls = _urlProvider.GetOtherUrls(umbracoContext, id, current).ToList();
            otherUrls.Insert(0, russianUrl);

            return otherUrls;
        }

        public string GetUrl(global::Umbraco.Web.UmbracoContext umbracoContext, int id, Uri current, UrlProviderMode mode)
        {
            return _urlProvider.GetUrl(umbracoContext, id, current, mode);
        }


        private string BuildFullyLocalizedUrl(global::Umbraco.Web.UmbracoContext umbracoContext, int id, CultureInfo cultureInfo)
        {
            IList<string> segments = new List<string>();
            var content = umbracoContext.Application.Services.ContentService.GetById(id);
            GetLocalizedUrlSegments(content, cultureInfo, segments);
            return "/" + string.Join("/", segments.Reverse().ToArray()).ToLower();
        }

        private void GetLocalizedUrlSegments(IContent content, CultureInfo cultureInfo, IList<string> segments)
        {
            string segment = content.GetUrlSegment(cultureInfo);
            segments.Add(segment);
            var parent = content.Parent();
            if (parent != null)
            {
                GetLocalizedUrlSegments(parent, cultureInfo, segments);
            }
        }
    }
}
