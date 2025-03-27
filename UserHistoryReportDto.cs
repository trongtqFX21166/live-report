using System.ComponentModel.DataAnnotations;

namespace VietmapLive.TrafficReport.Core.Data
{
    public class UserHistoryReportDto
    {
        public Guid ReportId { get; set; }

        public Guid CategoryId { get; set; }

        public Guid? ParentId { get; set; }

        public string IconUrl { get; set; }

        public string Name { get; set; }

        public long SubmitTime { get; set; }

        public string Type { get; set; }

        public string? Status { get; set; }

        public string MarkerImageUrl { get; set; }

        public int NumberOfView { get; set; }

        public int NumberOfConfirm { get; set; }

        public int ActivityScore { get; set; }

        public string? StreetViewImageUrl { get; set; }

        public List<string>? Images { get; set; }

        public string? Direction { get; set; }

        public double Lat { get; set; }
        public double Lng { get; set; }
        public string? Address { get; set; }

        public double UserLat { get; set; }
        public double UserLng { get; set; }
        public string? UserAddress { get; set; }
    }
}
