namespace VietmapLive.TrafficReport.Core.Data
{
    public class SearchTrafficReportRequest
    {
        public string SearchText { get; set; }

        public string? ReportType { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDisplayOnMap { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }
}
