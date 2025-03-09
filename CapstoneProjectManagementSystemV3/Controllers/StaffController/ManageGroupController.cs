using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities;
using Infrastructure.Entities.Common;
using Infrastructure.Services.CommonServices.FinalGroupService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.ChangeFinalGroupRequestService;
using Infrastructure.Services.PrivateService.ChangeTopicRequestService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.RegisteredService;
using Infrastructure.Services.PrivateService.SpecialtyService;
using Infrastructure.Services.PrivateService.StudentService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageGroupController : BaseApiController
    {
        private readonly IProfessionService _professionService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IFinalGroupService _finalGroupService;
        private readonly IFinalGroupDisplayFormService _finalGroupDisplayFormService;
        private readonly ISemesterService _semesterService;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        private readonly IChangeFinalGroupRequestService _changeFinalGroupRequestService;
        private readonly IChangeTopicRequestService _changeTopicRequestService;
        private readonly INotificationService _notificationService;
        private readonly IMailService _mailService;
        private readonly IRegisteredService _registeredGroupService;
        private readonly ILogger<ManageGroupController> _logger;
        public ManageGroupController(IProfessionService professionService,
            ISpecialtyService specialtyService,
            IFinalGroupService finalGroupService,
            IFinalGroupDisplayFormService finalGroupDisplayFormService,
            ISemesterService semesterService,
            IStudentService studentService,
            IUserService userService,
            IChangeFinalGroupRequestService changeFinalGroupRequestService,
            IChangeTopicRequestService changeTopicRequestService,
            INotificationService notificationService,
            IMailService mailService,
            IRegisteredService registeredGroupService,
            ILogger<ManageGroupController> logger)
        {
            _professionService = professionService;
            _specialtyService = specialtyService;
            _finalGroupService = finalGroupService;
            _finalGroupDisplayFormService = finalGroupDisplayFormService;
            _semesterService = semesterService;
            _studentService = studentService;
            _userService = userService;
            _changeFinalGroupRequestService = changeFinalGroupRequestService;
            _changeTopicRequestService = changeTopicRequestService;
            _notificationService = notificationService;
            _mailService = mailService;
            _registeredGroupService = registeredGroupService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfessionList()
        {
            try
            {
                _logger.LogInformation("Fetching profession list");

                var currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (currentSemester != null)
                {
                    int semesterId = currentSemester.SemesterID;
                    List<ProfessionDto> professionList = (await _professionService.getAllProfession(semesterId)).ResultObj;

                    if (professionList == null)
                    {
                        return BadRequest(new { message = "No profession data found. Redirecting to setup major." });
                    }
                    var response = new
                    {
                        showGroupName = currentSemester.ShowGroupName,
                        professionList = professionList,
                        semesterList = _semesterService.GetAllSemester()
                    };

                    return Ok(response);
                }
                else
                {
                    return BadRequest(new { message = "No active semester found. Redirecting to semester management." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching profession list");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> GetFinalGroups(string ProfessionId, string SpecialtyId, string searchText)
        {
            try
            {
                _logger.LogInformation("Fetching final groups");

                var currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (currentSemester == null)
                {
                    return BadRequest(new { message = "No active semester found." });
                }

                int semesterId = currentSemester.SemesterID;
                int professionId = Convert.ToInt32(ProfessionId);
                int specialtyId = Convert.ToInt32(SpecialtyId);

                // Danh sách nhóm đã đủ thành viên
                List<FinalGroupDto> fullMemberFinalGroupList = (await _finalGroupService.GetFullMemberFinalGroupSearchList(
                    semesterId, professionId, specialtyId, searchText, 0, int.MaxValue)).ResultObj;
                List<FinalGroupDisplayForm> fullMemberFinalGroupDisplayFormList = (await _finalGroupDisplayFormService.ConvertFromFinalList(fullMemberFinalGroupList)).ResultObj;

                // Danh sách nhóm chưa đủ thành viên
                List<FinalGroupDto> lackOfMemberFinalGroupList = (await _finalGroupService.GetLackOfMemberFinalGroupSearchList(
                    semesterId, professionId, specialtyId, searchText, 0, int.MaxValue)).ResultObj;
                List<FinalGroupDisplayForm> lackOfMemberFinalGroupDisplayFormList =
                    (await _finalGroupDisplayFormService.ConvertFromFinalList(lackOfMemberFinalGroupList)).ResultObj;

                // Danh sách sinh viên chưa có nhóm
                List<StudentDto> studentList = (await _studentService.GetStudentSearchList(semesterId, professionId, specialtyId, 0, int.MaxValue)).ResultObj;

                if (studentList != null)
                {
                    foreach (var stu in studentList)
                    {
                        stu.Specialty.SpecialtyFullName =
                           (await _specialtyService.getSpecialtyById(stu.Specialty.SpecialtyID)).ResultObj.SpecialtyFullName;
                    }
                }

                var response = new
                {
                    showGroupName = currentSemester.ShowGroupName,
                    fullMemberFinalGroups = fullMemberFinalGroupDisplayFormList,
                    lackOfMemberFinalGroups = lackOfMemberFinalGroupDisplayFormList,
                    studentsWithoutGroup = studentList,
                    professionList = _professionService.getAllProfession(semesterId),
                    specialtyList = professionId != 0 ? (await _specialtyService.getSpecialtiesByProfessionId(professionId, semesterId)).ResultObj : new List<SpecialtyDto>(),
                    semesterList = _semesterService.GetAllSemester()
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching manager group data");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
    }
}