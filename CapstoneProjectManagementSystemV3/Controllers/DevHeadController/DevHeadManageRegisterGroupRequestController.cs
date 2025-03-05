using Infrastructure.Entities.Dto.RegisteredDto;
using Infrastructure.Services.CommonServices.FinalGroupService;
using Infrastructure.Services.CommonServices.GroupIdeaService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.ConfigurationService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.SpecialtyService;
using Infrastructure.Services.PrivateService.StaffService;
using Infrastructure.Services.PrivateService.StudentService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.PrivateService.RegisteredService;
using Infrastructure.Services.PrivateService.StudentGroupIdeaService;
using Infrastructure.Services.PrivateService.Student_FavoriteGroupIdeaService;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.PrivateService.SupervisorGroupIdeaService;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Entities;
using Infrastructure.ViewModel.SupervisorViewModel;
using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using System.Transactions;

namespace CapstoneProjectManagementSystemV3.Controllers.DevHeadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevHeadManageRegisterGroupRequestController : BaseApiController
    {
        private readonly IRegisteredService _registeredGroupService;
        private readonly ISemesterService _semesterService;
        private readonly IGroupIdeaService _groupIdeaService;
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ISupervisorService _supervisorService;
        private readonly IStudentGroupIdeaService _studentGroupIdeaService;
        private readonly ISpecialtyService _specialtyService;
        private readonly IFinalGroupService _finalGroupService;
        private readonly IStudentFavoriteGroupIdeaService _studentFavoriteGroupIdeaService;
        private readonly IMailService _mailService;
        private readonly INotificationService _notificationService;
        private readonly RealTimeHub _realTimeHub;
        private readonly IStaffService _staffService;
        //private readonly ISessionExtensionService sessionExtensionService;
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly IConfigurationService _configurationService;
        private readonly IProfessionService _professionService;
        private readonly ILogger<DevHeadManageRegisterGroupRequestController> _logger;
        private readonly ISupervisorGroupIdeaService _supervisorGroupIdeaService;
        public DevHeadManageRegisterGroupRequestController(IRegisteredService registeredGroupService, 
            ISemesterService semesterService, 
            IGroupIdeaService groupIdeaService, 
            IUserService userService, 
            IStudentService studentService, 
            ISupervisorService supervisorService, 
            IStudentGroupIdeaService studentGroupIdeaService, 
            ISpecialtyService specialtyService, 
            IFinalGroupService finalGroupService, 
            IStudentFavoriteGroupIdeaService studentFavoriteGroupIdeaService, 
            IMailService mailService, INotificationService notificationService, 
            RealTimeHub realTimeHub, IStaffService staffService, 
            IDataRetrievalService dataRetrievalService, 
            IConfigurationService configurationService, 
            IProfessionService professionService, 
            ILogger<DevHeadManageRegisterGroupRequestController> logger, 
            ISupervisorGroupIdeaService supervisorGroupIdeaService)
        {
            _registeredGroupService = registeredGroupService;
            _semesterService = semesterService;
            _groupIdeaService = groupIdeaService;
            _userService = userService;
            _studentService = studentService;
            _supervisorService = supervisorService;
            _studentGroupIdeaService = studentGroupIdeaService;
            _specialtyService = specialtyService;
            _finalGroupService = finalGroupService;
            _studentFavoriteGroupIdeaService = studentFavoriteGroupIdeaService;
            _mailService = mailService;
            _notificationService = notificationService;
            _realTimeHub = realTimeHub;
            _staffService = staffService;
            _dataRetrievalService = dataRetrievalService;
            _configurationService = configurationService;
            _professionService = professionService;
            _logger = logger;
            _supervisorGroupIdeaService = supervisorGroupIdeaService;
        }
        [HttpGet("Index")]
        public async Task<IActionResult> Index(string supervisorEmails = "", int page = 1, int fetchRow = 5,string professions = "", int filterIsAssigned = -1)
        {
            try
            {
                _logger.LogInformation("View manage register group request page");

                Semester currentSemester =(await _semesterService.GetCurrentSemester()).ResultObj;
                bool isBeforeDeadline = DateTime.Now.Date <= currentSemester.DeadlineRegisterGroup;
                User user = _dataRetrievalService.GetData<User>("sessionAccount");

                List<RegisteredGroupRequest> registeredGroupRequests;
                int totalPage = 0;
                int currentPage = 0;
                int[] professionsIds;

                List<Profession> devheadProfessions =(await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(user.UserId, true)).ResultObj;
                if (string.IsNullOrEmpty(professions))
                {
                    professionsIds = devheadProfessions.Select(p => p.ProfessionId).ToArray();
                }
                else
                {
                    professionsIds = professions.Split(",").Select(p => Convert.ToInt32(p)).ToArray();
                }

                if (professionsIds.Length == 0)
                {
                    return BadRequest(new { message = "No valid professions provided." });
                }

                (currentPage, totalPage, registeredGroupRequests) = _registeredGroupService.GetListRegisteredGroupRequests(
                    supervisorEmails, page, fetchRow, professionsIds, isBeforeDeadline, filterIsAssigned);

                var response = new
                {
                    TotalPage = totalPage,
                    CurrentPage = currentPage + 1,
                    RegisteredGroupRequests = registeredGroupRequests,
                    UserID = user.UserId,
                    ProfessionIds = professionsIds,
                    DevheadProfessions = devheadProfessions,
                    FilterIsAssigned = filterIsAssigned
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "View manage register group request page error");
                return StatusCode(500, new { error = "Internal Server Error", message = "An unexpected error occurred." });
            }
        }
        [HttpGet("GetRegisterGroupRequests")]
        public async Task<IActionResult> GetRegisterGroupRequests(string supervisorEmails = "",int page = 1,int fetchRow = 5,string professions = "",int filterIsAssigned = -1)
        {
            try
            {
                _logger.LogInformation("Get register group request");

                Semester currentSemester =(await _semesterService.GetCurrentSemester()).ResultObj;
                User user = _dataRetrievalService.GetData<User>("sessionAccount");

                int[] professionsIds;
                if (string.IsNullOrEmpty(professions))
                {
                    professionsIds = (await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(user.UserId, true)).ResultObj.Select(p => p.ProfessionId).ToArray();
                }
                else
                {
                    professionsIds = professions.Split(",").Select(p => Convert.ToInt32(p)).ToArray();
                }

                bool isBeforeDeadline = DateTime.Now.Date <= currentSemester.DeadlineRegisterGroup;

                List<RegisteredGroupRequest> registeredGroupRequests;
                int totalPage = 0;
                int currentPage = 0;

                if (professionsIds.Length > 0)
                {
                    (currentPage, totalPage, registeredGroupRequests) = _registeredGroupService.GetListRegisteredGroupRequests(
                        supervisorEmails, page, fetchRow, professionsIds, isBeforeDeadline, filterIsAssigned);
                }
                else
                {
                    return BadRequest(new { message = "No valid professions provided." });
                }

                var response = new
                {
                    TotalPage = totalPage,
                    CurrentPage = currentPage + 1,
                    RegisteredGroupRequests = registeredGroupRequests
                };

                return Ok(response);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Get register group request error");
                return StatusCode(500, new { error = "Internal Server Error", message = "An unexpected error occurred." });
            }
        }

        [HttpGet("GetAssignedGroups")]
        public async Task<IActionResult> GetAssignedGroups([FromQuery] string supervisorEmails = "")
        {
            try
            {
                _logger.LogInformation("Get assigned group");

                User user = _dataRetrievalService.GetData<User>("sessionAccount");
                List<Profession> devheadProfessions =(await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(user.UserId, true)).ResultObj;

                if (devheadProfessions.Count == 0)
                {
                    return NotFound(new { message = "No professions found for the user." });
                }

                var result = _registeredGroupService.GetAssignedGroup(
                    supervisorEmails, devheadProfessions.Select(p => p.ProfessionId).ToArray());

                return Ok(result);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Get assigned group error");
                return StatusCode(500, new { error = "Internal Server Error", message = "An unexpected error occurred." });
            }
        }
        [HttpGet("GetDetailRequestRegisterGroup")]
        public async Task<IActionResult> GetDetailRequestRegisterGroup([FromQuery] int registeredGroupId)
        {
            try
            {
                _logger.LogInformation("Get detail of request register group");

                RegisteredGroup registeredGroup = (await _registeredGroupService.GetGroupIDByRegisteredGroupId(registeredGroupId)).ResultObj;
                var result =(await _registeredGroupService.GetRegisteredGroupRequest(registeredGroupId)).ResultObj;
                GroupIdea groupIdea = (await _groupIdeaService.GetGroupIdeaById(Convert.ToInt32(registeredGroup.GroupIdea.GroupIdeaId))).ResultObj;
                List<Student> ungroupedStudents = new List<Student>();
                int semesterId =(await _semesterService.GetCurrentSemester()).ResultObj.SemesterId;

                if (groupIdea != null && groupIdea.Semester.SemesterId == semesterId)
                {
                    List<With> withSpecs =(await _configurationService.GetWithsBySpecialtyID(groupIdea.Specialty.SpecialtyId)).ResultObj;
                    int[] specialtyIds = (int[])withSpecs.Select(w => w.Specialty.SpecialtyId);
                    ungroupedStudents =(await _studentService.GetListUngroupedStudentsBySpecialityIdAndSemesterId(semesterId, specialtyIds)).ResultObj;

                    // Send group details
                    User leader =(await _userService.GetUserByID((await _studentGroupIdeaService.GetLeaderIdByGroupIdeaId(groupIdea.GroupIdeaId)).ResultObj)).ResultObj;
                    result.Leader = leader;
                    List<User> memberList = new List<User>();
                    List<string> memberIdList =(await _studentGroupIdeaService.GetMemberIdByGroupIdeaId(groupIdea.GroupIdeaId)).ResultObj;
                    if (memberIdList != null)
                    {
                        foreach (string mId in memberIdList)
                        {
                            memberList.Add((await _userService.GetUserByID(mId)).ResultObj);
                        }
                    }
                    result.Members = memberList;
                    int numberOfMember = leader != null ? 1 : 0;
                    numberOfMember += memberList.Count;
                    result.NumberOfMember = numberOfMember;
                }

                return Ok(new { request = result, students = ungroupedStudents });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Get detail of request register group error");
                return StatusCode(500, new { error = "Internal Server Error", message = exception.Message });
            }
        }

        // POST: api/DevHeadManageRegisterGroupRequest/GetSupervisorsForAssigning
        [HttpPost("GetSupervisorsForAssigning")]
        public async Task<IActionResult> GetSupervisorsForAssigning([FromBody] int registeredGroupId)
        {
            try
            {
                _logger.LogInformation("Get supervisors for assigning");

                RegisteredGroupRequest request =(await _registeredGroupService.GetRegisteredGroupRequest(registeredGroupId)).ResultObj;
                User user = _dataRetrievalService.GetData<User>("sessionAccount");
                var result = _registeredGroupService.GetRegisteredGroupRequest(registeredGroupId);
                List<Profession> devheadProfessions =(await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(user.UserId, true)).ResultObj;
                GroupIdea groupIdea =(await _groupIdeaService.GetGroupIdeaById(Convert.ToInt32(request.GroupIdea.GroupIdeaId))).ResultObj;
                List<SupervisorForAssigning> supervisors = devheadProfessions.Count == 0 || groupIdea == null ? new List<SupervisorForAssigning>() : (await _supervisorService.GetSupervisorsForAssigning(devheadProfessions.Select(p => p.ProfessionId).ToArray(), groupIdea.Profession.ProfessionId, registeredGroupId)).ResultObj;

                return Ok(new { supervisors, group = result });
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Get supervisors for assigning error");
                return StatusCode(500, new { error = "Internal Server Error", message = exception.Message });
            }
        }

        // POST: api/DevHeadManageRegisterGroupRequest/ConfirmAsFinalGroups
        //[HttpPost("ConfirmAsFinalGroups")]
        //public async IActionResult ConfirmAsFinalGroups([FromBody] string registeredGroupIds)
        //{
        //    _logger.LogInformation("Confirm as final group");
        //    List<ConfirmFinalGroupResponse> responses = new List<ConfirmFinalGroupResponse>();
        //    using (var scope = new TransactionScope())
        //    {
        //        try
        //        {
        //            string[] registeredGroupIdArray = registeredGroupIds.Split(",");
        //            Semester currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;

        //            foreach (string registeredGroupID in registeredGroupIdArray)
        //            {
        //                Supervisor assignedSupervisor = null;

        //                RegisteredGroupRequest registeredGroupRequest = (await _registeredGroupService.GetRegisteredGroupRequestIncludingAllSupervisor(Convert.ToInt32(registeredGroupID))).ResultObj;
        //                var groupId = registeredGroupRequest.GroupIdea.GroupIdeaID;
        //                var groupIdea = _groupIdeaService.GetGroupIdeaById(groupId);
        //                var codeOfGroupName = _specialtyService.GetCodeOfGroupNameByGroupIdeaId(groupId);

        //                Generate final group name
        //                string groupName = $"{codeOfGroupName}_G1"; // Simplified logic for illustration
        //                int finalGroupId = 0;
        //                (finalGroupId, groupIdea, assignedSupervisor) = _finalGroupService.AddRegisteredGroupToFinalGroup(registeredGroupRequest, groupName);

        //                _notificationService.InsertDataNotification(assignedSupervisor.SupervisorID, $"You have been assigned to group {groupName}", "/Group/Index");
        //                responses.Add(new ConfirmFinalGroupResponse()
        //                {
        //                    GroupName = groupName,
        //                    ProjectName = groupIdea.ProjectEnglishName,
        //                    Profession = registeredGroupRequest.GroupIdea.Profession,
        //                    Specialty = registeredGroupRequest.GroupIdea.Specialty
        //                });

        //                Send notifications to students and set final group
        //                var listInforStudentInGroupIdea = _studentGroupIdeaService.GetInforStudentInGroupIdeaBySemester(groupId, currentSemester.SemesterID);
        //                string notificationContent = "Your group's registration request has been approved";
        //                string attachedLink = "/MyGroup/Index";
        //                string listEmailMemberInGroup = "";

        //                foreach (var student in listInforStudentInGroupIdea)
        //                {
        //                    if (student.Status == 1)
        //                    {
        //                        if (_studentService.SetFinalGroupForStudent(finalGroupId, 1, student.StudentID, groupName) == 1)
        //                        {
        //                            _notificationService.InsertDataNotification(student.StudentID, notificationContent, attachedLink);
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (_studentService.SetFinalGroupForStudent(finalGroupId, 0, student.StudentID, groupName) == 1)
        //                        {
        //                            _notificationService.InsertDataNotification(student.StudentID, notificationContent, attachedLink);
        //                        }
        //                    }

        //                    listEmailMemberInGroup += _userService.GetUserByID(student.StudentID).FptEmail + ",";
        //                }

        //                Send email notification for student submit registration

        //               string subject = $"{currentSemester.SubjectMailTemplate}".Replace("{semesterCode}", currentSemester.SemesterCode);
        //                string body = $"{currentSemester.BodyMailTemplate}".Replace("{groupInformation}", "some table").Replace("{semesterName}", currentSemester.SemesterName);
        //                _mailService.SendMailNotification(listEmailMemberInGroup, null, subject, body);

        //                _registeredGroupService.UpdateStatusOfRegisteredGroup(Convert.ToInt32(registeredGroupID), 3);
        //            }

        //            scope.Complete();
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Confirm as final group error");
        //            return StatusCode(500, new { error = "Internal Server Error", message = ex.Message });
        //        }
        //    }
        //    return Ok(responses);
        //}
    }
}
