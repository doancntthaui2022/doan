using Microsoft.AspNetCore.Http;

namespace HoangNV.HotelBooking.Core.Utils
{
    public static class PrefixFunctions
    {
        public static string GetPrefix(HttpRequest request)
        {
            if (!string.IsNullOrEmpty(request.Headers["prefix"]))
            {
                return request.Headers["prefix"];
            }
            return string.Empty;
        }

        public static string GetPrefixUrlByUrl(HttpRequest request, string url)
        {
            var prefix = GetPrefix(request);
            if (!string.IsNullOrEmpty(prefix) && !url.StartsWith($"/{prefix}"))
            {
                url = CombineUri(prefix, url);
            }

            return url;
        }

        public static string CombineUri(string prefix, string url)
        {
            prefix = prefix.Trim('/');
            url = url.TrimStart('/');
            return $"/{prefix}/{url}";
        }
    }
}
