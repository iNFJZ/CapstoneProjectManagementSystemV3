using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.FinalGroupService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.ChangeTopicRequestService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.StudentService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageChangeTopicController : BaseApiController
    {
        private readonly ISemesterService _semesterService;
        private readonly IChangeTopicRequestService _changeTopicRequestService;
        private readonly IFinalGroupService _finalGroupService;
        private readonly IProfessionService _professionService;
        private readonly IStudentService _studentService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IMailService _mailService;
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IStaffService _staffService;
        private readonly ILogger<ManageChangeTopicController> _logger;
        public ManageChangeTopicController(ISemesterService semesterService,
            IChangeTopicRequestService changeTopicRequestService,
            IFinalGroupService finalGroupService,
            IProfessionService professionService,
            IStudentService studentService,
            INotificationService notificationService,
            IUserService userService,
            IMailService mailService,
            ISessionExtensionService sessionExtensionService,
            IStaffService staffService,
            ILogger<ManageChangeTopicController> logger)
        {
            _semesterService = semesterService;
            _changeTopicRequestService = changeTopicRequestService;
            _finalGroupService = finalGroupService;
            _professionService = professionService;
            _studentService = studentService;
            _notificationService = notificationService;
            _userService = userService;
            _mailService = mailService;
            _sessionExtensionService = sessionExtensionService;
            _staffService = staffService;
            _logger = logger;
        }
        [HttpGet("/index")]
        public async Task<IActionResult> GetProfessionList()
        {
            try
            {
                _logger.LogInformation("View list profession");
                var currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (currentSemester != null)
                {
                    int semesterId = currentSemester.SemesterID;
                    var professionList = (await _professionService.getAllProfession(semesterId)).ResultObj;

                    if (professionList == null || !professionList.Any())
                    {
                        return NotFound(new { message = "No professions found. Please set up a major." });
                    }

                    var user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                    return Ok(new { UserID = user?.UserId, professionList });
                }
                return BadRequest(new { message = "Current semester not found." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving profession list");
                return StatusCode(500, new { message = "Internal server error." });
            }
        }
        [HttpGet("/list")]
        public async Task<IActionResult> GetChangeTopicRequests(int status = 3, string searchText = "", string pagingType = "none", int recordNumber = 0)
        {
            try
            {
                _logger.LogInformation("Fetching list of change topic requests");
                var semester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (semester == null) return NotFound("No active semester found");

                int numberOfRecordsPerPage = 5;
                int startNum = (recordNumber != 0) ? recordNumber : 1;

                if (pagingType.Equals("previous")) startNum -= numberOfRecordsPerPage;
                else if (pagingType.Equals("next")) startNum += numberOfRecordsPerPage;

                int countResult = (await _changeTopicRequestService.CountRecordChangeTopicRequestsBySearchText(searchText, status, semester.SemesterID)).ResultObj;
                var changeTopicRequests = (await _changeTopicRequestService.GetChangeTopicRequestsBySearchText(searchText, status, semester.SemesterID, startNum - 1, numberOfRecordsPerPage)).ResultObj;

                return Ok(new { numberOfRecordsPerPage, startNum, countResult, changeTopicRequests });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching change topic requests");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("detail/{requestId}")]
        public async Task<IActionResult> GetChangeTopicRequestDetail(int requestId)
        {
            try
            {
                _logger.LogInformation("Fetching details for request ID {RequestId}", requestId);
                var requestDetail = (await _changeTopicRequestService.GetDetailChangeTopicRequestsByRequestId(requestId)).ResultObj;
                if (requestDetail == null) return NotFound("Request not found");

                return Ok(requestDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching change topic request detail");
                return StatusCode(500, "Internal server error");
            }
        }
        [HttpPost]
        [Route("api/change-topic/accept")]
        public async Task<IActionResult> AcceptChangeTopicRequest([FromBody] int changeTopicRequestId)
        {
            try
            {
                _logger.LogInformation("Accept change topic request");
                if (changeTopicRequestId == 0) return BadRequest("Invalid request ID");

                if ((await _changeTopicRequestService.UpdateStatusOfChangeTopicRequest(changeTopicRequestId, 4, "")).ResultObj == false)
                    return BadRequest("Failed to update request status");

                var newTopic = (await _changeTopicRequestService.GetNewTopicByChangeTopicRequestId(changeTopicRequestId)).ResultObj;
                if ((await _finalGroupService.UpdateNewTopicForFinalGroup(newTopic)).ResultObj == false)
                {
                    _changeTopicRequestService.UpdateStatusOfChangeTopicRequest(changeTopicRequestId, 0, "");
                    return BadRequest("Failed to update topic for final group");
                }

                var detail = (await _changeTopicRequestService.GetDetailChangeTopicRequestsByRequestId(changeTopicRequestId)).ResultObj;
                var students = (await _studentService.GetListStudentIdByFinalGroupId(newTopic.FinalGroup.FinalGroupId)).ResultObj;
                string attachedLinkForStudent = "/MyGroup/Index";
                string contentNotificationForStudent = "Your group's changing topic request has been approved";
                string listEmailMemberInGroup = string.Join(",", students.Select(async s => (await _userService.GetUserByID(s.StudentId)).ResultObj.FptEmail));

                students.ForEach(student => _notificationService.InsertDataNotification(student.StudentId, contentNotificationForStudent, attachedLinkForStudent));

                string subject = "Accepted change topic request of final group";
                string body = "<h2 style='color:black'>Capstone Project Registration System</h2>" +
              "<p style='color:black'>Hello!</p>" +
              "<p style='color:black'>Your group's changing topic request has been approved.</p>" +
              "<p style='color:black'>Regards,</p>" +
              "<p style='color:black'>Capstone Project Registration System</p>" +
              $"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/MyGroup/Index'>Your Group</a>";

                _mailService.SendMailNotification(listEmailMemberInGroup, detail.EmailSuperVisor, subject, body);
                return Ok("Request approved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Accept change topic request error");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        [Route("api/change-topic/reject")]
        public async Task<IActionResult> RejectChangeTopicRequest(int ChangeTopicRequestId, string CommentReject)
        {
            try
            {
                _logger.LogInformation("Reject change topic request");
                if (ChangeTopicRequestId == 0 || string.IsNullOrEmpty(CommentReject))
                    return BadRequest("Invalid input data");

                if ((await _changeTopicRequestService.UpdateStatusOfChangeTopicRequest(ChangeTopicRequestId, -1, CommentReject)).ResultObj == false)
                    return BadRequest("Failed to update request status");

                var newTopic = (await _changeTopicRequestService.GetNewTopicByChangeTopicRequestId(ChangeTopicRequestId)).ResultObj;
                var detail = (await _changeTopicRequestService.GetDetailChangeTopicRequestsByRequestId(ChangeTopicRequestId)).ResultObj;
                var students = (await _studentService.GetListStudentIdByFinalGroupId(newTopic.FinalGroup.FinalGroupId)).ResultObj;
                string attachedLinkForStudent = "/ChangeTopic/Index";
                string contentNotificationForStudent = "Your group's changing topic request has been rejected";
                string listEmailMemberInGroup = string.Join(",", students.Select(async s => (await _userService.GetUserByID(s.StudentId)).ResultObj.FptEmail));

                students.ForEach(student => _notificationService.InsertDataNotification(student.StudentId, contentNotificationForStudent, attachedLinkForStudent));

                string subject = "Rejected change topic request of final group";
                string body = "<h2 style='color:black'>Capstone Project Registration System</h2>" +
              "<p style='color:black'>Hello!</p>" +
              "<p style='color:black'>Your group's changing topic request has been approved.</p>" +
              "<p style='color:black'>Regards,</p>" +
              "<p style='color:black'>Capstone Project Registration System</p>" +
              $"<a href='{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}/MyGroup/Index'>Your Group</a>";

                _mailService.SendMailNotification(listEmailMemberInGroup + "," + detail.EmailSuperVisor, null, subject, body);
                return Ok("Request rejected successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Reject change topic request error");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}