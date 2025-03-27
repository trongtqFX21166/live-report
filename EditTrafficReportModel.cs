using System.ComponentModel.DataAnnotations;

namespace VietmapLive.TrafficReport.Api.Models.Model
{
    public class EditTrafficReportModel
    {
        [Required]
        public Guid ReportId { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        [Required]
        public string Direction { get; set; }
        public List<string>? Images { get; set; }
    }
}
