namespace Core.DataAccess.EntityFramework
{
    public sealed class SpecificationConfiguration
    {
        public bool EnableTracking { get; set; } = true;
        public bool IgnoreQueryFilters { get; set; } = false;
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 20;
    }
}