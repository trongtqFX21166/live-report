namespace VietmapLive.TrafficReport.Api.Models.Request
{
    public class EditTrafficReportRequest
    {
        public Guid ReportId { get; set; }
        public Guid CategoryId { get; set; }
        public string Direction { get; set; }
        public List<string>? Images { get; set; }
    }
}
