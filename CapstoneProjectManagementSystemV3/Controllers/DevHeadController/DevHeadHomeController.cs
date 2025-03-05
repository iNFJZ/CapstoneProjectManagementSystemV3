using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.DevHeadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevHeadHomeController : BaseApiController
    {
        private readonly ILogger<DevHeadHomeController> _logger;

        public DevHeadHomeController(ILogger<DevHeadHomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet("home")]
        public ActionResult HomePage()
        {
            try
            {
                _logger.LogInformation("View home page of supervisor leader");
                // Trả về một thông báo hoặc dữ liệu JSON thay vì View
                var data = new { Message = "Welcome to the home page of supervisor leader." };
                return Ok(data);  // Trả về dữ liệu JSON với mã HTTP 200
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View home page of supervisor leader error");
                // Nếu có lỗi, trả về mã lỗi 500 và thông báo lỗi
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}
