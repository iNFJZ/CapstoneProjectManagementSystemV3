using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3UI.Controllers.Admin_Controller
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class CreateUserController : Controller
    {
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly ILogger<CreateUserController> _logger;
        private readonly IConfiguration _configuration;
        public CreateUserController(ISessionExtensionService sessionExtensionService
     , IConfiguration configuration
                         , ILogger<CreateUserController> logger)
        {
            _sessionExtensionService = sessionExtensionService;
            _configuration = configuration;
            _logger = logger;
        }
        public IActionResult Index()
        {
            try
            {
            //    _logger.LogInformation("View Create User Screen");
            //    User user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");

            //    ViewBag.sessionRole = user.Role.RoleId;
                return View("Views/Admin_View/CreateUser/Index.cshtml");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View Create User Screen error");
                return View("/Views/Common_View/404NotFound.cshtml");
            }
        }
    }
}
