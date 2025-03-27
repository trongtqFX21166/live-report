namespace VietmapLive.TrafficReport.Core.Data
{
    public class TrafficCategoryAuditLogDto
    {
        public int UserId { get; set; }
        public string Action { get; set; }
        public List<string>? ModifyDetails { get; set; }
        public string CategoryName { get; set; }
        public long CreatedAt { get; set; }
    }
}
