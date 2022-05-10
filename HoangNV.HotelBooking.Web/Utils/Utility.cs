using HoangNV.HotelBooking.Web.Localization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace HoangNV.HotelBooking.Web.Utils
{
    public class Utility
    {
        public static int NormalizePage(int start, int length)
        {
            var page = start / NormalizeSize(length) + 1;
            return page > 1 ? page : 1;
        }

        public static int NormalizeSize(int size)
        {
            return size > 0 ? size : int.MaxValue;
        }
        public static Dictionary<int, string> GetEnumDisplay<T>(IStringLocalizer<SharedResource> _localizer) where T : Enum
        {
            var rs = new Dictionary<int, string>();
            foreach (T value in Enum.GetValues(typeof(T)).Cast<T>())
            {
                rs[(int)(object)value] = GetEnumDisplay<T>(value, _localizer);
            }
            return rs;
        }

        public static string GetEnumDisplay<T>(T value, IStringLocalizer<SharedResource> _localizer) where T : Enum
        {
            var m = value.GetType().GetMember(value.ToString()).FirstOrDefault();
            if (m == null)
            {
                return "";
            }

            var display = m.GetCustomAttribute<DisplayAttribute>()?.Name;
            return string.IsNullOrEmpty(display) ? "" : _localizer[display];
        }
    }
}
