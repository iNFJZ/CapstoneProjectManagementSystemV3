using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using CapstoneProjectManagementSystemV3.Controllers.StudentController;
using Infrastructure.Entities;
using Infrastructure.Repositories.Supervisor_GroupIdeaReporitory;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.StudentService;
using Infrastructure.Services.PrivateService.SupervisorGroupIdeaService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Infrastructure.Services.PrivateService.SupportService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.DevHeadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevHeadListIdeaSupervisorController : BaseApiController
    {
        private readonly ISupportService _supportService;
        private readonly IStudentService _studentService;
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IStaffService _staffService;
        private readonly ISupervisorService _supervisorService;
        private readonly ISupervisorGroupIdeaService _supervisorGroupIdeaService;
        private readonly ILogger<DevHeadListIdeaSupervisorController> _logger;
        public DevHeadListIdeaSupervisorController(ISupportService supportService,
            IStudentService studentService,
            ISessionExtensionService sessionExtensionService,
            IUserService userService,
            INotificationService notificationService,
            IStaffService staffService,
            ISupervisorService supervisorService,
            ISupervisorGroupIdeaService supervisorGroupIdeaService,
            ILogger<DevHeadListIdeaSupervisorController> logger)
        {
            _supportService = supportService;
            _studentService = studentService;
            _sessionExtensionService = sessionExtensionService;
            _userService = userService;
            _notificationService = notificationService;
            _staffService = staffService;
            _supervisorService = supervisorService;
            _supervisorGroupIdeaService = supervisorGroupIdeaService;
            _logger = logger;
        }

        [HttpGet("/ViewListIdeasSupervisor")]
        public async Task<IActionResult> ViewListIdeasSupervisor(int page)
        {
            try
            {
                _logger.LogInformation("View list idea of other supervisor");

                User user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");

                if (user == null)
                {
                    _logger.LogWarning("User not found in session.");
                    return Unauthorized(new { message = "User not authorized or session expired." });
                }

                var supervisorId = user.UserId;
                int totalPage = 0;
                List<GroupIdeaOfSupervisorDto> listGroupIdea;

                // Fetching the data with paging
                (totalPage, page, listGroupIdea) = (await _supervisorGroupIdeaService.getGroupIdeaOfSupervisorWithPaging(page, supervisorId)).ResultObj;

                // Prepare the response object
                var response = new
                {
                    TotalPage = totalPage,
                    PageIndex = page,
                    ListIdeaSupervisor = listGroupIdea
                };

                return Ok(response); // Return JSON response with 200 OK status
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View list idea of other supervisor error");
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }
        [HttpGet("/detailIdeaSupervisor/{id}")]
        public async Task<ActionResult> GetDetailIdeaSupervisor(int id)
        {
            try
            {
                _logger.LogInformation("View detail of other supervisors' idea");

                // Lấy thông tin người dùng từ session
                User user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                if (user == null)
                {
                    _logger.LogWarning("User not found in session.");
                    return Unauthorized("User is not logged in.");
                }

                // Lấy chi tiết nhóm ý tưởng supervisor
                GroupIdeaOfSupervisorDto groupIdeaDetail = (await _supervisorGroupIdeaService.GetGroupIdeaOfSupervisorByGroupIdeaId(id)).ResultObj;
                if (groupIdeaDetail == null)
                {
                    _logger.LogWarning($"Group Idea with ID {id} not found.");
                    return NotFound($"Group Idea with ID {id} not found.");
                }

                // Lấy danh sách chuyên ngành của nhóm ý tưởng
                List<GroupIdeaOfSupervisorProfessionDto> listProSpe = (await _supervisorGroupIdeaService.getAllProfessionSpecialyByGroupIdeaID(id)).ResultObj;

                var result = new
                {
                    groupIdeaDetail,
                    listProSpe
                };

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while viewing detail of idea supervisor.");
                return StatusCode(500, new { Message = "An error occurred while processing your request." });
            }
        }
    }
}