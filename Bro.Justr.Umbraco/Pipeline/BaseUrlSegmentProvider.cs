using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;
using Umbraco.Core.Strings;
using Bro.Justr.Umbraco.Models;
using Newtonsoft.Json;

namespace Bro.Justr.Umbraco.Pipeline
{
    /// <summary>
    /// Default implementation of IUrlSegmentProvider.
    /// </summary>
    public class BaseUrlSegmentProvider : IUrlSegmentProvider
    {
        private readonly IShortStringHelper _shortStringHelper;

        public BaseUrlSegmentProvider()
        {
            _shortStringHelper = new DefaultShortStringHelper().WithDefaultConfig();
        }

        /// <summary>
        /// Gets the default url segment for a specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns>The url segment.</returns>
        public string GetUrlSegment(IContentBase content)
        {
            string text = GetUrlSegmentSource(content);
            return _shortStringHelper.CleanStringForUrlSegment(text);
        }

        /// <summary>
        /// Gets the url segment for a specified content and culture.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>The url segment.</returns>
        public string GetUrlSegment(IContentBase content, CultureInfo culture)
        {
            string text = GetUrlSegmentSource(content, culture);
            return _shortStringHelper.CleanStringForUrlSegment(text, culture);
        }

        private static string GetUrlSegmentSource(IContentBase content, CultureInfo culture = null)
        {
            string source = null;
            
            if (content.HasProperty(Settings.Umbraco.UmbracoUrlNameProperty))
                source = (content.GetValue<string>(Settings.Umbraco.UmbracoUrlNameProperty) ?? string.Empty).Trim();
            
            if (string.IsNullOrWhiteSpace(source) && 
                content.HasProperty(Settings.Justr.PageUrlSegmentProperty))
            {
                var vortoValueString = content.GetValue<string>(Settings.Justr.PageUrlSegmentProperty);
                var vortoValue = vortoValueString != null ? JsonConvert.DeserializeObject<VortoValue>(vortoValueString) : null;
                if (vortoValue != null && vortoValue.Values.Any())
                {
                    culture = culture ?? Settings.Justr.DefaultCulture;

                    source = vortoValue.Values.Where(v => v.Key == culture.Name).Select(v => v.Value.ToString()).FirstOrDefault();

                    if (string.IsNullOrWhiteSpace(source))
                    {
                        source = vortoValue.Values.First().Value.ToString();
                    }
                }
            }
            
            if (string.IsNullOrWhiteSpace(source))
                source = content.Name;
            
            return source;
        }
    }
}
