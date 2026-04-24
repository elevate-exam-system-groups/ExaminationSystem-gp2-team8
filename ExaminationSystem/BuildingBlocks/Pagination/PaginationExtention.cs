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
            var items =await query.Skip((PageIndex-1) * PageSize).Take(PageSize).ToListAsync();

            //return paginated result


            return new PaginatedResult<T>(items, count, PageIndex, PageSize);
           
        }
    }
}
