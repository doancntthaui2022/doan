using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace HoangNV.HotelBooking.Web.Helper
{
    internal static class ResourceGroupExtensions
    {
        internal static string ToJavascript(this IEnumerable<ResourceGroup> resources)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("var Resources = ");

            var uiObj = new Dictionary<string, string>();

            foreach (ResourceGroup fieldGroup in resources)
            {
                foreach (var entry in fieldGroup.Entries)
                {
                    uiObj[entry.Name] = entry.Value;
                }
            }

            sb.Append(JsonSerializer.Serialize(uiObj));
            sb.Append(";");

            return sb.ToString();
        }
    }
}
