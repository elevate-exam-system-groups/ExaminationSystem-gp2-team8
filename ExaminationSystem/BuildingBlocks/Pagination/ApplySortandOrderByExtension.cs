using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public static class ApplySortandOrderByExtension
    {
        public static IQueryable<T> ApplySortandOrderBy<T>(this IQueryable<T> query, string sortBy, string orderBy)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return query;
            }
            //sortby 

            //what i will sort by is a string and
            //i want to sort by it so i will get the property of the type T
            //that has the name of sortBy and then i will use it to sort the query

            //property
            //so i get the property of the type T that has the name of sortBy
            var sortByProperty = typeof(T).GetProperty(sortBy);
            if (sortByProperty != null)
            {
                if (orderBy?.ToLower() == "desc")
                {
                    //orderByDes(x=>x.name)
                    query = query.OrderByDescending(x => EF.Property<object>(x, sortByProperty.Name));
                }
                else
                {
                    query = query.OrderBy(x => EF.Property<object>(x, sortByProperty.Name));
                }
            }



            return query;

        }
    }
}
