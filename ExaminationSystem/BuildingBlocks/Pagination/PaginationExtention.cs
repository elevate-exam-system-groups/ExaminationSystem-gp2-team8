using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public static class PaginationExtention
    {
      
        public static async Task<PaginatedResult<T>> ApplyPagination<T>(this IQueryable<T> query,int PageIndex,int PageSize) where T : class 
        {
            //get count
            var count= await query.CountAsync();
            //Apply pagination
            var pageIndex = Math.Max(1, PageIndex);
            var pageSize = Math.Max(1, PageSize);

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            //return paginated result


            return new PaginatedResult<T>(items, count, PageIndex, PageSize);
           
        }
    }
}
