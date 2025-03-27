namespace VietmapLive.TrafficReport.Core.Data
{
    public class SearchTrafficCategorySnapshotsRequest
    {
        public string? SearchText { get; set; }
        public long? FromTime { get; set; }
        public long? ToTime { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
