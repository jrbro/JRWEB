using Bro.Justr.Umbraco.Pipeline;
using umbraco.cms.businesslogic.web;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using Umbraco.Core.Strings;
using Umbraco.Web.Routing;
using System.Linq;
using Bro.Justr.Umbraco.Extensions;
using System.Globalization;

namespace Bro.Justr.Umbraco
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            LogHelper.Info(this.GetType(), "JR: Registering ProductUrlProvider");
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, ProductUrlProvider>();

            LogHelper.Info(this.GetType(), "JR: Registering BaseUrlSegmentProvider");
            UrlSegmentProviderResolver.Current.InsertTypeBefore<DefaultUrlSegmentProvider, BaseUrlSegmentProvider>();

            LogHelper.Info(this.GetType(), "JR: Registering ProductUrlSegmentProvider");
            UrlSegmentProviderResolver.Current.InsertTypeBefore<BaseUrlSegmentProvider, ProductUrlSegmentProvider>();

            LogHelper.Info(this.GetType(), "JR: Registering ProductContentFinder");
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, ProductContentFinder>();

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }

        protected override void ApplicationStarted(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            ContentService.Publishing += ContentService_Publishing;
            //ContentService.Saving += ContentService_Saving;

           

            base.ApplicationStarted(umbracoApplication, applicationContext);
        }

        void ContentService_Saving(IContentService sender, global::Umbraco.Core.Events.SaveEventArgs<IContent> e)
        {
            
        }

        void ContentService_Publishing(global::Umbraco.Core.Publishing.IPublishingStrategy sender, global::Umbraco.Core.Events.PublishEventArgs<IContent> e)
        {
            foreach (var content in e.PublishedEntities)
            {
                if (content.HasProperty(Settings.Justr.PageUrlSegmentProperty) && content.HasProperty(Settings.Umbraco.UmracoUrlAliasProperty))
                {
                    string originalAlias = (string)content.GetValue(Settings.Umbraco.UmracoUrlAliasProperty) ?? string.Empty;
                    var originalAliases = originalAlias.Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries).Select(i => i.Trim()).ToList();

                    CultureInfo russianCulture = new CultureInfo("ru-RU");
                    string russianUrlSegment = ((IContentBase)content).GetUrlSegment(russianCulture);

                    if (!string.IsNullOrWhiteSpace(russianUrlSegment))
                    {
                        #warning TODO generate page URL for russian version of the site
                        string russianVersionPageUrl = ("/ru/" + russianUrlSegment).Trim().ToLower().TrimEnd("/");

                        if (!originalAliases.Contains(russianVersionPageUrl))
                        {
                            originalAliases.Insert(0, russianVersionPageUrl); //should be the first item in the list
                        }

                        content.SetValue(Settings.Umbraco.UmracoUrlAliasProperty, string.Join(",", originalAliases.ToArray()));
                    }
                }
            }
        }
    }
}
