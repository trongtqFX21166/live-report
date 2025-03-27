using System.ComponentModel.DataAnnotations.Schema;

namespace VietmapLive.TrafficReport.Core.Entities.Vml
{
    [Table("user")]
    public partial class User
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("accountid")]
        public long? AccountId { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("phone")]
        public string Phone { get; set; }

        [Column("email")]
        public string Email { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("passwordsalt")]
        public string PasswordSalt { get; set; }

        [Column("fullname")]
        public string FullName { get; set; }

        [Column("remark")]
        public string Remark { get; set; }

        [Column("lockeddate")]
        public long? LockedDate { get; set; }

        [Column("islocked")]
        public bool IsLocked { get; set; }

        [Column("vehicleid")]
        public long? VehicleId { get; set; }

        [Column("grouproles")]
        public long?[] GroupRoles { get; set; }

        [Column("inactive")]
        public bool InActive { get; set; }

        [Column("createddate")]
        public long? CreatedDate { get; set; }

        [Column("lastmodified")]
        public long? LastModified { get; set; }

        [Column("facebook")]
        public string FaceBook { get; set; }

        [Column("google")]
        public string Google { get; set; }

        [Column("apple")]
        public string Apple { get; set; }

        [Column("zalo")]
        public string Zalo { get; set; }

        [Column("address")]
        public string Address { get; set; }

        [Column("referralid")]
        public long? ReferralId { get; set; }

        [Column("waypoint")]
        public string Waypoint { get; set; }

        [Column("avatar")]
        public string Avatar { get; set; }

        [Column("icon_id")]
        public int? IconId { get; set; }

        [Column("setting")]
        public string Setting { get; set; }

        [Column("vetc_balance")]
        public long VetcBalance { get; set; }

        [Column("issync")]
        public bool? IsSync { get; set; }

        [Column("is_verified")]
        public bool IsVerified { get; set; }
    }
}
