namespace ExaminationSystem.BuildingBlocks.Pagination
{
    public class PaginationParams
    {
        private int _page = 1;
        private int _perPage = 10;

        public int Page
        {
            get => _page;
            set => _page = value < 1 ? 1 : value;
        }

        public int PerPage
        {
            get => _perPage;
            set => _perPage = value < 1 ? 10 : (value > 100 ? 100 : value);
        }

        public string? SortBy { get; set; }

        public string Order
        {
            get => _order;
            set => _order = value?.ToLower() == "desc" ? "desc" : "asc";
        }
        private string _order = "asc";
    }
}
