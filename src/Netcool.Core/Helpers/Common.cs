using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Netcool.Core
{
    public static class Common
    {
        public static Type GetType<T>()
        {
            return GetType(typeof(T));
        }

        public static Type GetType(Type type)
        {
            return Nullable.GetUnderlyingType(type) ?? type;
        }

        public static T SafeValue<T>(this T? value) where T : struct
        {
            return value ?? default(T);
        }

        public static string SafeString(this object input)
        {
            return input?.ToString()?.Trim() ?? string.Empty;
        }

        public static bool TryGetEnumDescription<T>(T value, out string description) where T : struct
        {
            description = string.Empty;
            if (!(value is Enum)) return false;
            var type = typeof(T);
            var name = Enum.GetName(type, value);
            if (name == null) return false;
            var field = type.GetField(name);
            var attr = field?.GetCustomAttribute<DescriptionAttribute>();
            if (attr == null) return false;
            description = attr.Description;
            return true;
        }

        public static string GetEnumDescription<T>(T value) where T : struct
        {
            return TryGetEnumDescription(value, out var description) ? description : null;
        }


        public static List<EnumItem> GetEnumItems(Type enumType)
        {
            var pairs = new List<EnumItem>();
            var values = Enum.GetValues(enumType);
            foreach (var value in values)
            {
                var name = Enum.GetName(enumType, value);
                var field = enumType.GetField(name);
                var attr = field?.GetCustomAttribute<DescriptionAttribute>();
                if (attr != null)
                {
                    pairs.Add(new EnumItem
                    {
                        Name = name,
                        Value = Convert.ToInt32(value),
                        Description = attr.Description
                    });
                }
            }

            return pairs;
        }

        public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static bool IsOsx => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

        public static string System => IsWindows ? "Windows" : IsLinux ? "Linux" : IsOsx ? "OSX" : string.Empty;
    }
}
