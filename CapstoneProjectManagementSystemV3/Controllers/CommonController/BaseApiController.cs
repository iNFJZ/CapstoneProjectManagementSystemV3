using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.CommonController
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected string GetConnectionString()
        {
            if (!Request.Headers.TryGetValue("X-Connection-String", out var connectionString))
            {
                return null;
            }
            return connectionString.ToString();
        }
    }
}
