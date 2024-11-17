using Microsoft.EntityFrameworkCore;

namespace Onoicrm.DataContext.Utils;

public static class Extensions
{
    public static bool LikeTo(this string source, string value)
    {
        return EF.Functions.Like(source.ToUpper(), $"%{value.ToUpper()}%");
    }

    

}