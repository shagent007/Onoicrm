using System.Linq.Dynamic.Core;

namespace Onoicrm.Api.Utils;

public static class LinqUtils
{
    public static IQueryable Project(this IQueryable<object> items, string fields)
    {
        if (string.IsNullOrEmpty(fields) || string.IsNullOrWhiteSpace(fields))
        {
            return items;
        }
        var config = new ParsingConfig { AllowNewToEvaluateAnyType = true };

        return items.Select(config,fields);
    }
}