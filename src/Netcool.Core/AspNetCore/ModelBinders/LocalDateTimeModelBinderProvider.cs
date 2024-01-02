using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Netcool.Core.AspNetCore.ModelBinders;

public class LocalDateTimeModelBinderProvider : IModelBinderProvider
{
    internal const DateTimeStyles SupportedStyles = DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AdjustToUniversal;

    /// <inheritdoc />
    public IModelBinder GetBinder(ModelBinderProviderContext context)
    {
        if (context == null)
            throw new ArgumentNullException(nameof(context));
        return context.Metadata.UnderlyingOrModelType == typeof(DateTime)
            ? new LocalDateTimeModelBinder(SupportedStyles)
            : (IModelBinder)null;
    }
}
