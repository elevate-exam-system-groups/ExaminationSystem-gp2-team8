using ExaminationSystem.BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.API.Extensions
{
    public static class Pagination
    {
      
        public static async Task<PaginatedResult<T>> ApplyPagination<T>(this IQueryable<T> query,int PageIndex,int PageSize) where T : class 
        {
            //get count
            var count= query.Count();
            //Apply pagination
            var items =await query.Skip((PageIndex-1) * PageSize).Take(PageSize).ToListAsync();

            //return paginated result

            return new PaginatedResult<T>(items, count, PageIndex, PageSize);
           
        }
    }
}
