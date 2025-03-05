using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.StudentService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Infrastructure.Services.PrivateService.SupportService;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CapstoneProjectManagementSystemV3.Controllers.DevHeadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevHeadEditSupervisorController : BaseApiController
    {
        private readonly ISupportService _supportService;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly INotificationService _notificationService;
        private readonly IStaffService _staffService;
        private readonly ISupervisorService _supervisorService;
        private readonly ILogger<DevHeadViewNewsController> _logger;
        public DevHeadEditSupervisorController(ISupportService supportService,
            IStudentService studentService,
            IUserService userService,
            IDataRetrievalService dataRetrievalService,
            INotificationService notificationService,
            IStaffService staffService,
            ISupervisorService supervisorService,
            ILogger<DevHeadViewNewsController> logger)
        {
            _supportService = supportService;
            _studentService = studentService;
            _userService = userService;
            _dataRetrievalService = dataRetrievalService;
            _notificationService = notificationService;
            _staffService = staffService;
            _supervisorService = supervisorService;
            _logger = logger;
        }
        [HttpPost("editProfileSupervisor")]
        public async Task<ActionResult> EditProfileSupervisor([FromBody] SupervisorDto supervisor)
        {
            try
            {
                _logger.LogInformation("Edit profile of Supervisor");

                // Kiểm tra thông tin người dùng từ session
                var user = _dataRetrievalService.GetData<User>("sessionAccount");
                if (user == null)
                {
                    _logger.LogWarning("User session is null.");
                    return Unauthorized("User is not logged in.");
                }

                // Kiểm tra vai trò của người dùng
                if (user.Role.RoleId != 4)
                {
                    supervisor.User.UserID = user.UserId;
                }
                supervisor.SupervisorID = supervisor.User.UserID;

                // Kiểm tra số điện thoại hợp lệ
                var regexPhoneNumber = "^(0?)(3[2-9]|5[6|8|9]|7[0|6-9]|8[0-6|8|9]|9[0-4|6-9])[0-9]{7}$";
                var match = Regex.Match(supervisor.PhoneNumber, regexPhoneNumber);

                if (!match.Success || string.IsNullOrEmpty(supervisor.PhoneNumber))
                {
                    _logger.LogWarning("Invalid phone number.");
                    return BadRequest("Invalid phone number.");
                }

                // Cập nhật thông tin supervisor
                _supervisorService.UpdateInforProfileOfSupervisor(supervisor);

                _logger.LogInformation("Supervisor profile updated successfully.");
                return Ok("Profile updated successfully.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error occurred while editing supervisor profile.");
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
