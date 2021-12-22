using System;
using System.Text;

namespace Netcool.Core;

public static class StringExtensions
{
    public static string ToSnakeCase(this string s)
    {
        if (string.IsNullOrWhiteSpace(s))
        {
            return s;
        }

        s = s.Trim();

        var length = s.Length;
        var addedByLower = false;
        var stringBuilder = new StringBuilder();

        for (var i = 0; i < length; i++)
        {
            var currentChar = s[i];

            if (char.IsWhiteSpace(currentChar))
            {
                continue;
            }

            if (currentChar.Equals('_'))
            {
                stringBuilder.Append('_');
                continue;
            }

            bool isLastChar = i + 1 == length,
                isFirstChar = i == 0,
                nextIsUpper = false,
                nextIsLower = false;

            if (!isLastChar)
            {
                nextIsUpper = char.IsUpper(s[i + 1]);
                nextIsLower = !nextIsUpper && !s[i + 1].Equals('_');
            }

            if (!char.IsUpper(currentChar))
            {
                stringBuilder.Append(char.ToLowerInvariant(currentChar));

                if (nextIsUpper)
                {
                    stringBuilder.Append('_');
                    addedByLower = true;
                }

                continue;
            }

            if (nextIsLower && !addedByLower && !isFirstChar)
            {
                stringBuilder.Append('_');
            }

            addedByLower = false;

            stringBuilder.Append(char.ToLowerInvariant(currentChar));
        }

        return stringBuilder.ToString();
    }

    public static bool IsValidUrl(this string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;
        return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }
}
