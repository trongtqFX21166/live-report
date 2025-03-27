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
    [Route("v{version:apiVersion}/basemap-report")]
    [ApiController]
    public class BasemapReportV17Controller : ControllerBase
    {
        private readonly ITrafficReportService _trafficReportService;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryV17Controller> _logger;

        public BasemapReportV17Controller(ITrafficReportService trafficReportService,
            IMapper mapper,
            ILogger<CategoryV17Controller> logger)
        {
            _trafficReportService = trafficReportService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("submit")]
        [Authorize()]
        /// <summary>
        /// Direction = SameDirection / OppositeDirection
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<ActionResult<ApiResponse<BaseApiOutputCommon>>> Submit(SubmitBasemapReportModel model)
        {
            try
            {
                var request = _mapper.Map<SubmitBasemapReportRequest>(model);
                request.UserId = User.Identity.GetUserId();

                var response = await _trafficReportService.SubmitBasemapReportAsync(request);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(HttpContext.Request.Path, ex.ToString());
                return BadRequest(ResponseOutputCreaterCommon.InternalErrorResult(ex.Message));
            }
        }

        [HttpPost("edit")]
        [Authorize()]
        public async Task<ActionResult<ApiResponse<BaseApiOutputCommon>>> Edit(EditTrafficReportModel model)
        {
            try
            {
                var request = _mapper.Map<EditTrafficReportRequest>(model);

                var response = await _trafficReportService.EditTrafficReportAsync(request);

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
