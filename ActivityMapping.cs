using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities
{
    [Table("activity-mapping")]
    public class ActivityMapping : Entity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("activity_report_code")]
        public string ActivityReportCode { get; set; }

        [Column("activity_report_name")]
        public string ActivityReportName { get; set; }
    }
}
