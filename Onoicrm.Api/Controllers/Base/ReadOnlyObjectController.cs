using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Onoicrm.Api.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Onoicrm.Domain.Entities;
using Onoicrm.Domain.Models;
using Onoicrm.Domain.Utils;
using Onoicrm.DataContext;

namespace Onoicrm.Api.Controllers.Base;

[ApiController]
[Route("[controller]")]
public abstract class ReadOnlyObjectController<TEntity> : BaseController where TEntity : Entity, new()
{
    
    protected ReadOnlyObjectController(IConfiguration configuration, ApplicationDataContext dataContext, UserManager<IdentityUser> userManager) : base(configuration,dataContext, userManager)
    {
    }
    protected virtual IQueryable<TEntity> FilterPredicate(Filter filter, IQueryable<TEntity> entities)
    {
        return entities;
    }

    protected virtual async Task<TEntity> GetModel(Expression<Func<TEntity, bool>> expression)
    {
        var model = await Context.Set<TEntity>().SingleOrDefaultAsync(expression);
        return model;
    }

    [HttpGet]
    public virtual async Task<IActionResult> GetList(int pageIndex=1, int pageSize=20, string orderFieldName="Id", string orderFieldDirection="ASC", string filter = "", string fields="") => await ExecuteRequest(async () =>
    {
        var query = Context.Set<TEntity>()
            .AsNoTracking()
            .Filter(filter, FilterPredicate)
            .Sort(orderFieldName, orderFieldDirection);
            
        var items = await query
            .Paginate(pageIndex,pageSize)
            .Project(fields)
            .ToDynamicListAsync();
            
        var count = query.Count();
            
        return new ListResponse(items,count);
    });

    [HttpGet("{id:long}")]
    public virtual async Task<IActionResult> GetById(long id) => await ExecuteRequest(async () =>
    {
        var model = await GetModel(m => m.Id == id);
        return model;
    });

    
}