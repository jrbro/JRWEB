using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Web;
using System.Web.Caching;
using System.Web;

namespace Bro.Justr.Umbraco
{
    public class CacheHelper
    {
        public static string GetPageUrl(int contentId, string languageCode, bool isAbsoluteUrl = false)
        {
            string key = isAbsoluteUrl ? Settings.Justr.Cache.PageAbsoluteUrlsCacheKey : Settings.Justr.Cache.PageRelativeUrlsCacheKey; 
            var cachedUrls = (Dictionary<int, Dictionary<string, string>>)HttpContext.Current.Cache[key];
            if (cachedUrls != null)
            {
                if (cachedUrls.ContainsKey(contentId))
                {
                    if (cachedUrls[contentId].ContainsKey(languageCode))
                    {
                        string cachedUrlItem = cachedUrls[contentId][languageCode];
                        return cachedUrlItem;
                    }
                }
            }
            return null;
        }

        public static void SetPageUrl(int contentId, string langCode, string pageUrl, bool isAbsoluteUrl = false)
        {
            string key = isAbsoluteUrl ? Settings.Justr.Cache.PageAbsoluteUrlsCacheKey : Settings.Justr.Cache.PageRelativeUrlsCacheKey; 
            var cachedUrls = (Dictionary<int, Dictionary<string, string>>)HttpContext.Current.Cache[key];
                                
            if (cachedUrls == null)
            {
                cachedUrls = new Dictionary<int, Dictionary<string, string>>();
                HttpContext.Current.Cache.Insert(key, cachedUrls);
            }

            if (cachedUrls.ContainsKey(contentId))
            {
                var item = cachedUrls[contentId];
                if (item.ContainsKey(langCode))
                {
                    item[langCode] = pageUrl;
                }
                else
                {
                    item.Add(langCode, pageUrl);
                }
            }
            else
            {
                var dic = new Dictionary<string, string>();
                dic.Add(langCode, pageUrl);
                cachedUrls.Add(contentId, dic);
            }
        }

        public static void ClearPageUrlsCache(int contentId, bool isAbsoluteUrl = false)
        {
            string key = isAbsoluteUrl ? Settings.Justr.Cache.PageAbsoluteUrlsCacheKey : Settings.Justr.Cache.PageRelativeUrlsCacheKey;
            var cachedUrls = (Dictionary<int, Dictionary<string, string>>)HttpContext.Current.Cache[key];
            if (cachedUrls != null)
            {
                cachedUrls.Remove(contentId);
            }
        }
    }
}
