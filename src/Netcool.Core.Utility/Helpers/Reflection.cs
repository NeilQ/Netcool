﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Netcool.Core;

public static class Reflection
{
    public static List<EnumItem> GetEnumItems(Type enumType)
    {
        var pairs = new List<EnumItem>();
        var values = Enum.GetValues(enumType);
        foreach (var value in values)
        {
            var name = Enum.GetName(enumType, value);
            var field = enumType.GetField(name!);
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

    public static string GetEnumDescription(Type type, int value)
    {
        var name = Enum.GetName(type, value);
        if (name == null) return null;
        var field = type.GetField(name);
        if (field == null) return null;
        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        return attr == null ? field.Name : attr.Description;
    }

    public static string GetEnumDescription<T>(T value) where T : Enum
    {
        var type = typeof(T);
        var name = Enum.GetName(type, value);
        if (name == null) return null;
        var field = type.GetField(name);
        if (field == null) return null;
        var attr = field.GetCustomAttribute<DescriptionAttribute>();
        return attr == null ? field.Name : attr.Description;
    }

    public static string GetDescription<T>()
    {
        return GetDescription(Common.GetType<T>());
    }

    public static string GetDescription<T>(string memberName)
    {
        return GetDescription(Common.GetType<T>(), memberName);
    }

    public static string GetDescription(Type type, string memberName)
    {
        if (type == null)
            return string.Empty;
        if (string.IsNullOrWhiteSpace(memberName))
            return string.Empty;
        return GetDescription(type.GetTypeInfo().GetMember(memberName).FirstOrDefault());
    }

    public static string GetDescription(MemberInfo member)
    {
        if (member == null)
            return string.Empty;
        return member.GetCustomAttribute<DescriptionAttribute>() is { } attribute
            ? attribute.Description
            : member.Name;
    }

    public static string GetDisplayName<T>()
    {
        return GetDisplayName(Common.GetType<T>());
    }

    public static string GetDisplayName(MemberInfo member)
    {
        if (member == null)
            return string.Empty;
        if (member.GetCustomAttribute<DisplayAttribute>() is { } displayAttribute)
            return displayAttribute.Name;
        if (member.GetCustomAttribute<DisplayNameAttribute>() is { } displayNameAttribute)
            return displayNameAttribute.DisplayName;
        return string.Empty;
    }

    public static string GetDisplayNameOrDescription<T>()
    {
        return GetDisplayNameOrDescription(Common.GetType<T>());
    }

    public static string GetDisplayNameOrDescription(MemberInfo member)
    {
        var result = GetDisplayName(member);
        return string.IsNullOrWhiteSpace(result) ? GetDescription(member) : result;
    }

    public static List<Type> FindTypes<TFind>(params Assembly[] assemblies)
    {
        var findType = typeof(TFind);
        return FindTypes(findType, assemblies);
    }

    public static List<Type> FindTypes(Type findType, params Assembly[] assemblies)
    {
        var result = new List<Type>();
        foreach (var assembly in assemblies)
            result.AddRange(GetTypes(findType, assembly));
        return result.Distinct().ToList();
    }

    private static List<Type> GetTypes(Type findType, Assembly assembly)
    {
        var result = new List<Type>();
        if (assembly == null)
            return result;
        Type[] types;
        try
        {
            types = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException)
        {
            return result;
        }

        foreach (var type in types)
            AddType(result, findType, type);
        return result;
    }

    private static void AddType(List<Type> result, Type findType, Type type)
    {
        if (type.IsInterface || type.IsAbstract)
            return;
        if (findType.IsAssignableFrom(type) == false && MatchGeneric(findType, type) == false)
            return;
        result.Add(type);
    }

    private static bool MatchGeneric(Type findType, Type type)
    {
        if (findType.IsGenericTypeDefinition == false)
            return false;
        var definition = findType.GetGenericTypeDefinition();
        foreach (var implementedInterface in type.FindInterfaces((filter, criteria) => true, null))
        {
            if (implementedInterface.IsGenericType == false)
                continue;
            return definition.IsAssignableFrom(implementedInterface.GetGenericTypeDefinition());
        }

        return false;
    }

    public static Assembly GetAssembly(string assemblyName)
    {
        return Assembly.Load(new AssemblyName(assemblyName));
    }

    public static List<Assembly> GetAssemblies(string directoryPath)
    {
        return Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories).ToList()
            .Where(t => t.EndsWith(".exe") || t.EndsWith(".dll"))
            .Select(path => Assembly.Load(new AssemblyName(path))).ToList();
    }
}
