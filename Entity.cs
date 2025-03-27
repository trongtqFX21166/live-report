using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities
{
    public abstract class Entity
    {
        [Column("created_at")]
        public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        [Column("created_by")]
        public string? CreatedBy { get; set; } = "";

        [Column("last_modified")]
        public long LastModified { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        [Column("last_modified_by")]
        public string? LastModifiedBy { get; set; } = "";
    }
}
