namespace VietmapLive.TrafficReport.Api.Models.Request
{
    public class SubmitLiveReportRequest
    {
        public int UserId { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? Address { get; set; }
        public double UserLat { get; set; }
        public double UserLng { get; set; }
        public int UserHeading { get; set; }
        public int UserSpeed { get; set; }
        public string? UserAddress { get; set; }
        public long SubmitTime { get; set; }
        public Guid CategoryId { get; set; }
        public string? CellId { get; set; }

        public List<string>? Images { get; set; }
    }
}
