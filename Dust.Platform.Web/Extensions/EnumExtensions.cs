using System;
using System.ComponentModel;
using System.Reflection;

namespace Dust.Platform.Web.Extensions
{
    public static class EnumExtensions
    {
        public static bool TryGetDescription(this Enum enumValue, out string description)
        {
            var strValue = enumValue.ToString();
            var enumFailed = enumValue.GetType().GetField(strValue);
            if (!(enumFailed.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute descripAttr))
            {
                description = string.Empty;
                return false;
            }

            description = descripAttr.Description;
            return true;
        }

        public static string GetDescription(this Enum enumValue)
        {
            var strValue = enumValue.ToString();
            var enumFailed = enumValue.GetType().GetField(strValue);
            if (!(enumFailed.GetCustomAttribute(typeof(DescriptionAttribute)) is DescriptionAttribute descripAttr))
            {
                return string.Empty;
            }

            return descripAttr.Description;
        }
    }
}
