using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Netcool.Core.AspNetCore.ModelBinders;

public class LocalDateTimeModelBinder : IModelBinder
{
    private readonly DateTimeStyles _supportedStyles;

    public LocalDateTimeModelBinder(DateTimeStyles supportedStyles)
    {
        _supportedStyles = supportedStyles;
    }

    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
        if (valueProviderResult == ValueProviderResult.None)
        {
            // no entry
            return Task.CompletedTask;
        }

        var modelState = bindingContext.ModelState;
        modelState.SetModelValue(modelName, valueProviderResult);

        var metadata = bindingContext.ModelMetadata;
        var type = metadata.UnderlyingOrModelType;
        try
        {
            var value = valueProviderResult.FirstValue;

            object model;
            if (string.IsNullOrWhiteSpace(value))
            {
                // Parse() method trims the value (with common DateTimeSyles) then throws if the result is empty.
                model = null;
            }
            else if (type == typeof(DateTime))
            {
                var dateTime = DateTime.Parse(value, valueProviderResult.Culture, _supportedStyles);
                if (dateTime.Kind == DateTimeKind.Unspecified)
                {
                    dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Local);
                }
                else if (dateTime.Kind == DateTimeKind.Utc)
                {
                    dateTime = dateTime.ToLocalTime();
                }

                model = dateTime;
            }
            else
            {
                throw new NotSupportedException();
            }

            // When converting value, a null model may indicate a failed conversion for an otherwise required
            // model (can't set a ValueType to null). This detects if a null model value is acceptable given the
            // current bindingContext. If not, an error is logged.
            if (model == null && !metadata.IsReferenceOrNullableType)
            {
                modelState.TryAddModelError(
                    modelName,
                    metadata.ModelBindingMessageProvider.ValueMustNotBeNullAccessor(
                        valueProviderResult.ToString()));
            }
            else
            {
                bindingContext.Result = ModelBindingResult.Success(model);
            }
        }
        catch (Exception exception)
        {
            // Conversion failed.
            modelState.TryAddModelError(modelName, exception, metadata);
        }

        return Task.CompletedTask;
    }
}
