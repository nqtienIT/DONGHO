using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CuaHangDongHo.Helpers
{
    public static class GetEnum
    {
        public static string GetEnumDisplayName(this Enum enumType)
        {
            return enumType.GetType()
                        .GetMember(enumType.ToString())
                        .First()
                        .GetCustomAttribute<DisplayAttribute>()
                        .GetName();
        }
    }
}