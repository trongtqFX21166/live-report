using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities
{
    [Table("traffic-category-audit-log")]
    public class TrafficCategoryAuditLog : Entity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("action")]
        public string Action { get; set; }

        [Column("modify_details")]
        public List<string>? ModifyDetails { get; set; }
    }
}
