using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.ChangeFinalGroupRequestService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageChangeFinalGroupController : BaseApiController
    {
        private readonly ISemesterService _semesterService;
        private readonly IProfessionService _professionService;
        private readonly IChangeFinalGroupRequestService _changeFinalGroupRequestService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ILogger<ManageChangeFinalGroupController> _logger;
        public ManageChangeFinalGroupController(ISemesterService semesterService,
            IProfessionService professionService,
            IChangeFinalGroupRequestService changeFinalGroupRequestService,
            INotificationService notificationService,
            IUserService userService,
            IMailService mailService,
            ILogger<ManageChangeFinalGroupController> logger)
        {
            _semesterService = semesterService;
            _professionService = professionService;
            _changeFinalGroupRequestService = changeFinalGroupRequestService;
            _notificationService = notificationService;
            _userService = userService;
            _mailService = mailService;
            _logger = logger;
        }
        [HttpGet("/get-professions")]
        public async Task<IActionResult> GetProfessions()
        {
            try
            {
                _logger.LogInformation("Fetching profession list");
                var semester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (semester == null)
                    return NotFound("No active semester found");

                var professionList = (await _professionService.getAllProfession(semester.SemesterID));
                return professionList != null ? Ok(professionList) : NotFound("No professions found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching profession list");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("/get-change-requests")]
        public async Task<IActionResult> GetChangeRequests(int status, string searchText, string pagingType, int recordNumber)
        {
            try
            {
                _logger.LogInformation("Fetching change final group requests");
                var semester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (semester == null) return NotFound("No active semester found");

                int numberOfRecordsPerPage = 5;
                int startNum = (recordNumber != 0) ? recordNumber : 1;

                if (pagingType == "previous") startNum -= numberOfRecordsPerPage;
                else if (pagingType == "next") startNum += numberOfRecordsPerPage;

                int countResult = (await _changeFinalGroupRequestService.CountRecordChangeFinalGroupBySearchText(searchText, status, semester.SemesterID)).ResultObj;
                var listChangeFinalGroupRequest = (await _changeFinalGroupRequestService.GetListChangeFinalGroupRequestBySearchText(searchText, status, semester.SemesterID, startNum - 1, numberOfRecordsPerPage)).ResultObj;

                return Ok(new { numberOfRecordsPerPage, startNum, countResult, listChangeFinalGroupRequest });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching change requests");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("/accept-change-request")]
        public async Task<IActionResult> AcceptChangeRequest([FromBody] int changeFinalGroupRequestID)
        {
            try
            {
                _logger.LogInformation("Accepting change request");
                var request = (await _changeFinalGroupRequestService.GetInforOfStudentExchangeFinalGroup(changeFinalGroupRequestID)).ResultObj;
                if (request == null) return NotFound("Request not found");

                if ((await _changeFinalGroupRequestService.UpdateGroupForStudentByChangeFinalGroupRequest(request)).ResultObj == true)
                {
                    string notificationContent = "Your request to change groups has been approved";
                    string attachedLink = "/ChangeFinalGroup/Index";
                    _notificationService.InsertDataNotification(request.FromStudent.StudentId, notificationContent, attachedLink);
                    _notificationService.InsertDataNotification(request.ToStudent.StudentId, notificationContent, attachedLink);

                    string subject = "Accepted change final group request";
                    string body = "<h2>Capstone Project Registration System</h2><p>Hello!</p><p>Your group change request has been approved.</p><p>Regards, Capstone Project Registration System</p>";
                    string receiverEmail = (await _userService.GetUserByID(request.FromStudent.StudentId)).ResultObj.FptEmail + "," + (await _userService.GetUserByID(request.ToStudent.StudentId)).ResultObj.FptEmail;
                    _mailService.SendMailNotification(receiverEmail, null, subject, body);

                    return Ok("Request accepted");
                }
                return BadRequest("Failed to update request");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error accepting change request");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("/reject-change-request")]
        public async Task<IActionResult> RejectChangeRequest(int ChangeFinalGroupRequestID, string StaffComment)
        {
            try
            {
                _logger.LogInformation("Rejecting change request");
                if (ChangeFinalGroupRequestID == 0 || string.IsNullOrEmpty(StaffComment))
                    return BadRequest("Invalid request data");

                var request = (await _changeFinalGroupRequestService.GetInforOfStudentExchangeFinalGroup(ChangeFinalGroupRequestID)).ResultObj;
                if (request == null) return NotFound("Request not found");

                if ((await _changeFinalGroupRequestService.UpdateStatusOfStaffByChangeFinalGroupRequestId(ChangeFinalGroupRequestID, StaffComment)).ResultObj == true)
                {
                    string notificationContent = "Your request to change groups has been rejected";
                    string attachedLink = "/ChangeFinalGroup/Index";
                    _notificationService.InsertDataNotification(request.FromStudent.StudentId, notificationContent, attachedLink);
                    _notificationService.InsertDataNotification(request.ToStudent.StudentId, notificationContent, attachedLink);

                    string subject = "Rejected change final group request";
                    string body = "<h2>Capstone Project Registration System</h2><p>Hello!</p><p>Your group change request has been rejected.</p><p>Regards, Capstone Project Registration System</p>";
                    string receiverEmail = (await _userService.GetUserByID(request.FromStudent.StudentId)).ResultObj.FptEmail + "," + (await _userService.GetUserByID(request.ToStudent.StudentId)).ResultObj.FptEmail;
                    _mailService.SendMailNotification(receiverEmail, null, subject, body);

                    return Ok("Request rejected");
                }
                return BadRequest("Failed to update request");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting change request");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}