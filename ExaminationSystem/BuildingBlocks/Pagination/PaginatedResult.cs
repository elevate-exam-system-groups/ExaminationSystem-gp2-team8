
namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public record PaginatedResult<T>(IReadOnlyList<T> Data, int TotalCount, int Page, int PerPage)
    {
        //public int PerPage { get; set; }
        //public int Page { get; set; }
        //public IReadOnlyList<T> Data { get; set; }
        //public int TotalCount { get; set; }
        //public int TotalPage => TotalPage <= 0 ? 0 :  (int)Math.Ceiling((double)TotalCount / PerPage); 
        public int TotalPage => TotalCount <= 0 ? 0 :  (int)Math.Ceiling((double)TotalCount / PerPage); 

    }
}
