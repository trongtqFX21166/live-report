using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities
{
    [Table("traffic-category")]
    public class TrafficCategory : Entity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("parent_id")]
        public Guid? ParentId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("icon_url")]
        public string IconUrl { get; set; }

        [Column("marker_image_url")]
        public string? MarkerImageUrl { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("is_display_on_map")]
        public bool IsDisplayOnMap { get; set; }

        [Column("duration_display_on_map")]
        public int? DurationDisplayOnMap { get; set; }

        [Column("duration_display_on_map_type")]
        public string? DurationDisplayOnMapType { get; set; }

        [Column("add_duration_to_display_on_map")]
        public int? AddDurationToDisplayOnMap { get; set; }

        [Column("add_duration_to_display_on_map_type")]
        public string? AddDurationToDisplayOnMapType { get; set; }

        [Column("subtract_duration_to_hide_on_map")]
        public int? SubtractDurationToHideOnMap { get; set; }

        [Column("subtract_duration_to_hide_on_map_type")]
        public string? SubtractDurationToHideOnMapType { get; set; }

        [Column("display_target_user")]
        public string? DisplayTargetUser { get; set; }

        [Column("minimization_hex_color")]
        public string? MinimizationHexColor { get; set; }

        [Column("order")]
        public int Order { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public virtual ICollection<LiveReport> LiveReports { get; set; }

        public virtual ICollection<BasemapReport> BaseMapReports { get; set; }
    }
}
