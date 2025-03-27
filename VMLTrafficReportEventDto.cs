namespace VietmapLive.TrafficReport.Api.Models.Dtos
{
    public class VMLTrafficReportEventDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid CategoryId { get; set; }
        public long SubmitTime { get; set; }
        public long CreatedAt { get; set; }
        public string Event { get; set; }
        public string EventName { get; set; }
    }
}
