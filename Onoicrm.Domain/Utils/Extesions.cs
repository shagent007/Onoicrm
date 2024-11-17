using System.Globalization;
using System.Text.RegularExpressions;
using Onoicrm.Domain.Models;

namespace Onoicrm.Domain.Utils;

public static class Extensions
{
    public static string ToPascalCase(this string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
            return value;
        return string.Concat(value[..1].ToUpper(), value.AsSpan(1));
    }

    public static string MergeTimeSlots(IList<string> timeSlots)
    {
        return "";
    }

    public static DateTimeOffset TryParseDateTimeOffset(this string input)
    {
        try
        {
            var culture = CultureInfo.CreateSpecificCulture("ru-RU");
            var style = DateTimeStyles.None;
            var format = "dd.MM.yyyy HH:mm:ss";
            var success = DateTimeOffset.TryParseExact(input + " 00:00:00", format, culture, style, out var result);
            if (!success)
            {
                throw new Exception("Ошибка при парсинга");
            }

            return new DateTimeOffset(result.Year, result.Month, result.Day, 0, 0, 0, TimeSpan.Zero);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    
    public static DateTimeOffset TryParseDateTimeOffset(this Filter filter)
    {
        return filter.Value.GetString()!.TryParseDateTimeOffset();
    }
    
    public static DateTime ParseDate(this string dateString)
    {
        const string dateFormat = "dd.MM.yyyy";

        if (DateTime.TryParseExact(dateString, dateFormat, null, DateTimeStyles.None, out var result))
        {
            return result;
        }

        throw new ArgumentException("Невозможно распознать дату.");
    }
    
    public static object? GetDeepPropertyValue(this object? source, string path)
    {
        var pp = path.Split('.');
        if (source == null) return source;
        var t = source.GetType();
        foreach (var prop in pp)
        {
            var propInfo = t.GetProperty(prop);
            if (propInfo == null)
                throw new ArgumentException($"Properties path is not correct: {path}");
            
            source = propInfo.GetValue(source, null);
            t = propInfo.PropertyType;
        }

        return source;
    }

    public static string ReplaceTemplate<Type>(this string str, Type model)
    {
        var result = str;
        var regex = new Regex(@"(?<!\{)\{([a-zA-Z]+).*?\}(?!})");
        var matches = regex.Matches(str);
        if (matches.Count == 0) return result;
        foreach (Match match in matches)
        {
            var template = match.ToString();
            var fieldName = template.Substring(1, template.Length - 2);
            var fieldValue = model.GetDeepPropertyValue(fieldName);
            if(fieldValue == null)
            {
                fieldValue = "";
            }
            fieldValue = fieldValue.GetType().Name != nameof(String) ? fieldValue.ToString() : fieldValue;
            result = result.Replace(template, (string)fieldValue);
        }

        return result;
    }

    public static bool IsBase64String(this string base64)
    {
        var buffer = new Span<byte>(new byte[base64.Length]);
        return Convert.TryFromBase64String(base64, buffer, out var bytesParsed);
    }
    
    public static string ClearPhoneNumber(this string phoneNumber)
    {
        var formattedNumber = phoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");

        if (phoneNumber.StartsWith("+"))
        {
            formattedNumber = "+" + formattedNumber;
        }

        return formattedNumber;
    }
    
    
}