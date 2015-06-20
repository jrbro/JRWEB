using System.Globalization;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;

namespace Bro.Justr.Umbraco.Pipeline
{
    /// <summary>
    /// Umbraco segment provider for Product pages (Document Type "Product")
    /// </summary>
    public class ProductSegmentProvider : IUrlSegmentProvider
    {
        private readonly IUrlSegmentProvider _provider = new DefaultUrlSegmentProvider();
        private const int PRODUCT_DOCUMENT_TYPE_ID = 2113;

        public string GetUrlSegment(IContentBase content, CultureInfo culture)
        {
            if (content.ContentTypeId != PRODUCT_DOCUMENT_TYPE_ID) return null;

            var segment = culture != null ? _provider.GetUrlSegment(content, culture) : _provider.GetUrlSegment(content);

            return string.Format("{0}-p{1}", segment, content.Id); //add product ID to the end of URL to avoid duplicated URLs and to get content by id in the Product Content Finder
        }

        public string GetUrlSegment(IContentBase content)
        {
            return GetUrlSegment(content, null);
        }
    }
}
