namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public record PaginatedResult<T>(IReadOnlyList<T> Data, int TotalCount, int Page, int PerPage)
    {
        public int TotalPage => TotalPage <= 0 ? 0 :  (int)Math.Ceiling((double)TotalCount / PerPage); 

    }
}
