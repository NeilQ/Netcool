﻿using System.Globalization;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc.ModelBinding;
 using Netcool.Core.Extensions;

 namespace Acartons.Parking.Core.Api.ValueProviders
{
    public class SnakeCaseQueryValueProvider : QueryStringValueProvider
    {
        public SnakeCaseQueryValueProvider(
            BindingSource bindingSource,
            IQueryCollection values,
            CultureInfo culture)
            : base(bindingSource, values, culture)
        {
        }

        public override bool ContainsPrefix(string prefix)
        {
            return base.ContainsPrefix(prefix.ToSnakeCase());
        }

        public override ValueProviderResult GetValue(string key)
        {
            return base.GetValue(key.ToSnakeCase());
        }

    }
}