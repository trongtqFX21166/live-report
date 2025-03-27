using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace VietmapLive.TrafficReport.Core.Entities
{
    [Table("live-report-image")]
    public class LiveReportImage : Entity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("report_id")]
        public Guid ReportId { get; set; }

        [Column("image_url")]
        public string ImageUrl { get; set; }

        [Column("order")]
        public int Order { get; set; }

        public virtual LiveReport LiveReport { get; set; }
    }
}
