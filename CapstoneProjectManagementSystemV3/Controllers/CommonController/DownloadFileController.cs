using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Services.PrivateService.NewsService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.CommonController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadFileController : BaseApiController
    {
        private readonly INewsService _newsService;
        private readonly ILogger<DownloadFileController> _logger;
        public DownloadFileController(INewsService newsService, 
            ILogger<DownloadFileController> logger)
        {
            _newsService = newsService;
            _logger = logger;
        }
        [HttpGet("download-file/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                _logger.LogInformation("Download File");
                var file = await _newsService.DownloadFile(id);

                if (file == null)
                {
                    return NotFound(new ApiSuccessResult<string>("File not found"));
                }

                return Ok(new ApiSuccessResult<dynamic>(file));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Download File error");
                return BadRequest(new ApiSuccessResult<string>("Error downloading file"));
            }
        }

    }
}
