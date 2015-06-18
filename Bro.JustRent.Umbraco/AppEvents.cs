using Bro.JustRent.Umbraco.Pipeline;
using Umbraco.Core;
using Umbraco.Core.Logging;
using Umbraco.Core.Strings;
using Umbraco.Web.Routing;

namespace Bro.JustRent.Umbraco
{
    public class RegisterEvents : ApplicationEventHandler
    {
        protected override void ApplicationStarting(UmbracoApplicationBase umbracoApplication, ApplicationContext applicationContext)
        {
            LogHelper.Info(this.GetType(), "JR: Registering MyUrlProvider");
            UrlProviderResolver.Current.InsertTypeBefore<DefaultUrlProvider, ProductUrlProvider>();

            LogHelper.Info(this.GetType(), "JR: Registering MyUrlSegmentProvider");
            UrlSegmentProviderResolver.Current.InsertTypeBefore<DefaultUrlSegmentProvider, ProductSegmentProvider>();

            LogHelper.Info(this.GetType(), "JR: Registering ProductContentFinder");
            ContentFinderResolver.Current.InsertTypeBefore<ContentFinderByNiceUrl, ProductContentFinder>();

            base.ApplicationStarting(umbracoApplication, applicationContext);
        }
    }
}
