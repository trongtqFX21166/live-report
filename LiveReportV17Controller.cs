using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietmapLive.BuildingBlocks.Extensions;
using VietmapLive.Common.Models;
using VietmapLive.TrafficReport.Api.Models.Model;
using VietmapLive.TrafficReport.Api.Models.Request;
using VietmapLive.TrafficReport.Api.Services;

namespace VietmapLive.TrafficReport.Api.Controllers
{
    [ApiVersion("17")]
    [Route("v{version:apiVersion}/live-report")]
    [ApiController]
    public class LiveReportV17Controller : ControllerBase
    {
        private readonly ITrafficReportService _trafficReportService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryV17Controller> _logger;

        public LiveReportV17Controller(ITrafficReportService trafficReportService,
            IMapper mapper,
            ILogger<CategoryV17Controller> logger)
        {
            _trafficReportService = trafficReportService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("submit")]
        [Authorize()]
        public async Task<ActionResult<ApiResponse<BaseApiOutputCommon>>> Submit(SubmitLiveReportModel model)
        {
            try
            {
                var request = _mapper.Map<SubmitLiveReportRequest>(model);
                request.UserId = User.Identity.GetUserId();

                var response = await _trafficReportService.SubmitLiveReportAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(HttpContext.Request.Path, ex.ToString());
                return BadRequest(ResponseOutputCreaterCommon.InternalErrorResult(ex.Message));
            }
        }
    }
}
