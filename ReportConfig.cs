using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities
{
    [Table("report-config")]
    public class ReportConfig : Entity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("code")]
        public string Code { get; set; }

        [Column("config_data")]
        public string ConfigData { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
    }
}
