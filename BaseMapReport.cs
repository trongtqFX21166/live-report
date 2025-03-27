using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities
{
    [Table("basemap-report")]
    public class BasemapReport : Entity
    {
        [Key]
        [Column("id")]
        public Guid Id { get; set; }

        [Column("submit_time")]
        public long SubmitTime { get; set; }

        [Column("lat")]
        public double Lat { get; set; }

        [Column("lng")]
        public double Lng { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("description")]
        public string? Description { get; set; }

        [Column("reason")]
        public string? Reason { get; set; }

        [Column("category_id")]
        public Guid CategoryId { get; set; }

        [Column("score")]
        public int Score { get; set; }

        [Column("user_id")]
        public int UserId { get; set; }

        [Column("user_lat")]
        public double UserLat { get; set; }

        [Column("user_lng")]
        public double UserLng { get; set; }

        [Column("user_heading")]
        public int UserHeading { get; set; }

        [Column("user_speed")]
        public int UserSpeed { get; set; }

        [Column("user_address")]
        public string? UserAddress { get; set; }

        [Column("user_map_ver")]
        public string? UserMapVer { get; set; }

        [Column("direction")]
        public string Direction { get; set; }

        [Column("gps")]
        public string? Gps { get; set; }

        [Column("vehicle_type")]
        public string? VehicleType { get; set; }

        [Column("street_view_image_url")]
        public string? StreetViewImageUrl { get; set; }

        [Column("score_result_id")]
        public Guid ScoreResultId { get; set; }

        [Column("filter_result_id")]
        public Guid FilterResultId { get; set; }

        [Column("status")]
        public string? Status { get; set; }

        [Column("number_of_view")]
        public int NumberOfView { get; set; }

        [Column("number_of_confirm")]
        public int NumberOfConfirm { get; set; }

        [Column("number_of_rejection")]
        public int NumberOfRejection { get; set; }

        [Column("road_link_id")]
        public string? RoadLinkId { get; set; }

        [Column("cell_id")]
        public string? CellId { get; set; }

        [Column("is_deleted")]
        public bool IsDeleted { get; set; }

        public virtual TrafficCategory TrafficCategory { get; set; }

        public virtual ICollection<BasemapReportImage> BasemapReportImages { get; set; }
    }
}
