using AutoMapper;
using VietmapLive.TrafficReport.Api.Models.Dtos;
using VietmapLive.TrafficReport.Api.Models.Model;
using VietmapLive.TrafficReport.Api.Models.Request;
using VietmapLive.TrafficReport.Core.Entities;

namespace VietmapLive.TrafficReport.Api
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<SubmitBasemapReportRequest, BasemapReport>();
            CreateMap<SubmitLiveReportRequest, LiveReport>();
            CreateMap<SubmitLiveReportModel, SubmitLiveReportRequest>();
            CreateMap<SubmitBasemapReportModel, SubmitBasemapReportRequest>();
            CreateMap<EditTrafficReportModel, EditTrafficReportRequest>();
            CreateMap<TrafficCategory, CategoryDto>();
        }
    }
}
