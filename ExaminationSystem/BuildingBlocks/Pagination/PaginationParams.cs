namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public class PaginationParams
    {
        public int Page { get; set; } = 1;
        public int PerPage { get; set; } = 20;
        public string? SortBy { get; set; } 
        public string? Order { get; set; } = "asc";
    }
}
