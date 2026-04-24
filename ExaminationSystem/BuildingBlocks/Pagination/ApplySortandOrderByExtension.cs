using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public static class ApplySortandOrderByExtension
    {
        public static IQueryable<T> ApplySortandOrderBy<T>(this IQueryable<T> query, string sortBy, string orderBy)
        {
            if (string.IsNullOrWhiteSpace(sortBy))
                return query;

            var property = typeof(T).GetProperty(
                sortBy,
                System.Reflection.BindingFlags.IgnoreCase |
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance);

            if (property == null)
                throw new ArgumentException($"Property '{sortBy}' not found on type '{typeof(T).Name}'");

            return orderBy?.ToLower() == "desc"
                ? query.OrderByDescending(x => EF.Property<object>(x, sortBy))
                : query.OrderBy(x => EF.Property<object>(x, sortBy));

        }
    }
}
