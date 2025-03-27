using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VietmapLive.BuildingBlocks.CommonResponse;
using VietmapLive.Common.Models;
using VietmapLive.TrafficReport.Api.Models.Dtos;
using VietmapLive.TrafficReport.Api.Services;

namespace VietmapLive.TrafficReport.Api.Controllers
{
    [ApiVersion("17")]
    [Route("v{version:apiVersion}/Category")]
    [ApiController]
    public class CategoryV17Controller : ControllerBase
    {
        private readonly ITrafficReportService _trafficReportService;
        private readonly ILogger<CategoryV17Controller> _logger;

        public CategoryV17Controller(ITrafficReportService trafficReportService,
            ILogger<CategoryV17Controller> logger)
        {
            _trafficReportService = trafficReportService;
            _logger = logger;
        }

        [HttpGet("get-all")]
        [Authorize()]
        public async Task<ActionResult<ApiResponse<ItemsResponse<CategoryDto>>>> GetAllCategories()
        {
            try
            {
                var response = await _trafficReportService.GetAllCategoriesAsync();

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
