using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain.Utils;

namespace Onoicrm.DataContext.Utils;

public static class SearchUtils
{
    public static bool SearchInFields<TType>(IEnumerable<string> fields, TType source,string searchText)
    {
        var search = $"%{searchText.ToUpper()}%";
        var contain = false;
        foreach (var field in fields)
        {
            var value = (string?)source.GetDeepPropertyValue(field);
            if(string.IsNullOrEmpty(value)) continue;
            if (EF.Functions.Like(value.ToUpper(), search))
            {
                contain = true;
            }
        }

        return contain;
    }
}