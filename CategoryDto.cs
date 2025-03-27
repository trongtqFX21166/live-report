namespace VietmapLive.TrafficReport.Api.Models.Dtos
{
    public class CategoryDto
    {
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public string IconUrl { get; set; }

        public string Type { get; set; }

        public string MinimizationHexColor { get; set; }
    }
}
