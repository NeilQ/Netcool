using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Netcool.Api.Domain.EfCore;
using Netcool.Core;

namespace Netcool.Api.Controllers;

[Produces("application/json")]
[Route("enums")]
public class EnumsController : ControllerBase
{
    // GET
    [HttpGet("items")]
    [ProducesResponseType(typeof(Dictionary<string, IEnumerable<NameValue<int>>>), 200)]
    public IActionResult Get()
    {
        var attrDic = new Dictionary<string, List<NameValue<int>>>();

        var enumTypeInfos = Assembly.GetAssembly(typeof(NetcoolDbContext))?.DefinedTypes
            .Where(t => t.IsEnum).ToList();
        if (enumTypeInfos != null)
        {
            foreach (var typeInfo in enumTypeInfos)
            {
                attrDic.Add(typeInfo.Name.ToLower(), GetEnumNameValuePairs(typeInfo.AsType()));
            }
        }

        return Ok(attrDic);
    }

    [HttpGet]
    [ProducesResponseType(typeof(Dictionary<string, IEnumerable<NameValue<int>>>), 200)]
    public IActionResult GetSingle(string name)
    {
        if (string.IsNullOrEmpty(name)) return Ok(null);
        name = name.ToLower();

        var attrDic = new Dictionary<string, List<NameValue<int>>>();

        var enumTypeInfos = Assembly.GetAssembly(typeof(NetcoolDbContext))?.DefinedTypes
            .Where(t => t.IsEnum).ToList();
        if (enumTypeInfos == null) return Ok(null);
        foreach (var typeInfo in enumTypeInfos)
        {
            if (name == typeInfo.Name.ToLower())
            {
                attrDic.Add(typeInfo.Name.ToLower(), GetEnumNameValuePairs(typeInfo.AsType()));
            }
        }

        return Ok(attrDic);
    }

    public static List<NameValue<int>> GetEnumNameValuePairs(Type enumType)
    {
        var pairs = new List<NameValue<int>>();
        var values = Enum.GetValues(enumType);
        foreach (var value in values)
        {
            var name = Enum.GetName(enumType, value);
            if (string.IsNullOrEmpty(name)) continue;
            var field = enumType.GetField(name);
            if (field == null) continue;
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            pairs.Add(new NameValue<int>
            {
                Name = attr?.Description ?? name,
                Value = Convert.ToInt32(value)
            });
        }

        return pairs;
    }
}