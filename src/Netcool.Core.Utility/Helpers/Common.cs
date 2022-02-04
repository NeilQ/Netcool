using System.Runtime.InteropServices;

namespace Netcool.Core;

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

    public static bool IsLinux => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

    public static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    public static bool IsOsx => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);

    public static string System => IsWindows ? "Windows" : IsLinux ? "Linux" : IsOsx ? "OSX" : string.Empty;
}
