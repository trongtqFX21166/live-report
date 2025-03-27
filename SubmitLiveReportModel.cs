using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Api.Models.Model
{
    public class SubmitLiveReportModel
    {
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude is not valid")]
        public double Lat { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude is not valid")]
        public double Lng { get; set; }
        public string? Address { get; set; }
        [Required]
        [Range(-90, 90, ErrorMessage = "Latitude is not valid")]
        public double UserLat { get; set; }
        [Required]
        [Range(-180, 180, ErrorMessage = "Longitude is not valid")]
        public double UserLng { get; set; }
        [Required]
        public int UserHeading { get; set; }
        [Required]
        public int UserSpeed { get; set; }
        public string? UserAddress { get; set; }
        [Required]
        public long SubmitTime { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
        [MaxLength(5)]
        public List<string>? Images { get; set; }
        public string? CellId { get; set; }
    }
}
