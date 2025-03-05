using Infrastructure.Entities.Common.ApiResult;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.CommonController
{
    [Route("api/[controller]")]
    [ApiController]
    public class Error404Controller : BaseApiController
    {
        [HttpGet("not-found")]
        public IActionResult NotFoundPage()
        {
            return NotFound(new ApiSuccessResult<string>("Page not found"));
        }
    }
}
