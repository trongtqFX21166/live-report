using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.BuildingBlocks.Extensions;
using VietmapLive.Common.Models;
using VietmapLive.TrafficReport.Api.Services;
using VietmapLive.TrafficReport.Core.Data;

namespace VietmapLive.TrafficReport.Api.Controllers
{
    [ApiVersion("17")]
    [Route("v{version:apiVersion}/user-report")]
    [ApiController]
    public class UserReportV17Controller : ControllerBase
    {
        private readonly ITrafficReportService _trafficReportService;
        private readonly ILogger<CategoryV17Controller> _logger;

        public UserReportV17Controller(ITrafficReportService trafficReportService,
            ILogger<CategoryV17Controller> logger)
        {
            _trafficReportService = trafficReportService;
            _logger = logger;
        }

        [HttpGet("get-histories")]
        [Authorize()]
        public async Task<ActionResult<ApiResponse<PaginatedResult<UserHistoryReportDto>>>> GetHistories(int pageIndex, int pageSize)
        {
            try
            {
                var response = await _trafficReportService.GetUserHistoryReportsAsync(pageIndex, pageSize, User.Identity.GetUserId());

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(HttpContext.Request.Path, ex.ToString());
                return BadRequest(ResponseOutputCreaterCommon.InternalErrorResult(ex.Message));
            }
        }

        [HttpGet("get-total-reports")]
        [Authorize()]
        public async Task<ActionResult<int>> GetTotalReports(long? fromTime, long? toTime)
        {
            try
            {
                var response = await _trafficReportService.GetTotalReportsAsync(User.Identity.GetUserId(), fromTime, toTime);

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
