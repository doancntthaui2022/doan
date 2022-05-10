using HoangNV.HotelBooking.Core.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HoangNV.HotelBooking.Web.Utils
{
    public class ViewCommonFunction
    {
        public string GetPrefix(IUrlHelper urlHelper)
        {
            return PrefixFunctions.GetPrefix(urlHelper.ActionContext.HttpContext.Request);
        }

        public string GetPrefixUrlByControllerAction(IUrlHelper urlHelper, string action, string controller, object param = null)
        {
            var url = urlHelper.Action(action, controller);
            if (param != null)
            {
                url = urlHelper.Action(action, controller, param);
            }
            var prefix = PrefixFunctions.GetPrefix(urlHelper.ActionContext.HttpContext.Request);
            if (!string.IsNullOrEmpty(prefix))
            {
                url = PrefixFunctions.CombineUri(prefix, url);
            }

            return url;
        }

        public string GetPrefixUrlByUrl(IUrlHelper urlHelper, string url)
        {
            var prefix = PrefixFunctions.GetPrefix(urlHelper.ActionContext.HttpContext.Request);
            if (!string.IsNullOrEmpty(prefix) && !url.Contains($"/{prefix}"))
            {
                url = PrefixFunctions.CombineUri(prefix, url);
            }

            return url;
        }

        public string GetPrefixUrlByStaticUrl(IUrlHelper urlHelper, string staticUrl)
        {
            var url = staticUrl;
            var prefix = PrefixFunctions.GetPrefix(urlHelper.ActionContext.HttpContext.Request);
            if (!string.IsNullOrEmpty(prefix) && !url.Contains($"/{prefix}"))
            {
                if (url.StartsWith("~"))
                {
                    url = url.Replace("~/", $"/{prefix}/");
                }
                else
                {
                    url = PrefixFunctions.CombineUri(prefix, url);
                }
            }

            url = url.Replace("~", "");

            return url;
        }

        public string GetPrefixUrlByRequest(HttpRequest request, string url)
        {
            var prefix = PrefixFunctions.GetPrefix(request);
            if (!string.IsNullOrEmpty(prefix) && !url.Contains($"/{prefix}"))
            {
                url = PrefixFunctions.CombineUri(prefix, url);
            }

            return url;
        }
    }
}
