using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.GroupIdeaService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.CommonServices.UserService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.SpecialtyService;
using Infrastructure.Services.PrivateService.SupervisorGroupIdeaService;
using Infrastructure.Services.PrivateService.SupervisorProfessionService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Infrastructure.Services.PrivateService.SupportService;
using Infrastructure.ViewModel.SupervisorViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace CapstoneProjectManagementSystemV3.Controllers.DevHeadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevHeadCreateIdeaController : BaseApiController
    {
        private readonly IGroupIdeaService _groupIdeaService;
        private readonly ISessionExtensionService _sessionExtensionService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly ISupportService _supportService;
        private readonly IProfessionService _professionService;
        private readonly ISemesterService _semesterService;
        private readonly ISpecialtyService _specialService;
        private readonly ISupervisorService _supervisorService;
        private readonly ISupervisorGroupIdeaService _supervisorGroupIdeaService;
        private readonly ISupervisorProfessionService _supervisorProfessionService;
        private readonly ILogger<DevHeadCreateIdeaController> _logger;

        public DevHeadCreateIdeaController(IGroupIdeaService groupIdeaService, 
            ISessionExtensionService sessionExtensionService, 
            IUserService userService, 
            NotificationService notificationService, 
            ISupportService supportService, 
            IProfessionService professionService, 
            ISemesterService semesterService, 
            ISpecialtyService specialService, 
            ISupervisorService supervisorService, 
            ISupervisorGroupIdeaService supervisorGroupIdeaService, 
            ISupervisorProfessionService supervisorProfessionService, 
            ILogger<DevHeadCreateIdeaController> logger)
        {
            _groupIdeaService = groupIdeaService;
            _sessionExtensionService = sessionExtensionService;
            _userService = userService;
            _notificationService = notificationService;
            _supportService = supportService;
            _professionService = professionService;
            _semesterService = semesterService;
            _specialService = specialService;
            _supervisorService = supervisorService;
            _supervisorGroupIdeaService = supervisorGroupIdeaService;
            _supervisorProfessionService = supervisorProfessionService;
            _logger = logger;
        }
        [HttpGet("ideas")]
        public async Task<IActionResult> Index(int page = 1, int filterStatus = -1, string query = "")
        {
            try
            {
                _logger.LogInformation("Fetching ideas created by supervisor");
                int totalPage = 0;
                User user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                Supervisor supervisor = (await _supervisorService.GetSupervisorByUserId(user.UserId)).ResultObj;
                if (query == null)
                {
                    query = "";
                }

                (totalPage, page, var ideas) = (await _supervisorGroupIdeaService.GetListIdeaOfSupervisorForPaging(page, supervisor.SupervisorId, filterStatus, query.Trim())).ResultObj;

                List<GroupIdeasOfSupervisor> groupIdeaRegisted = (await _supervisorGroupIdeaService.GetGroupIdeaRegistedOfSupervisor(supervisor.SupervisorId)).ResultObj.ToList();
                List<int> registeredIdeaIds = groupIdeaRegisted.Select(g => g.GroupIdeaId).ToList();

                return Ok(new
                {
                    totalPage,
                    currentPage = page,
                    filterStatus,
                    query,
                    ideas,
                    registeredIdeaIds
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching ideas");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("ideas/view/create")]
        public async Task<IActionResult> CreateNewIdeaView(int profession)
        {
            try
            {
                _logger.LogInformation("Fetching data for idea creation");
                User user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                Supervisor supervisor = (await _supervisorService.GetSupervisorByUserId(user.UserId)).ResultObj;
                Semester semester = (await _semesterService.GetCurrentSemester()).ResultObj ?? new Semester { SemesterId = 0 };

                List<Profession> supervisorProfessions = semester.SemesterId == 0
                    ? new List<Profession>()
                    : (await _supervisorProfessionService.GetProfessionsBySupervisorID(supervisor.SupervisorId, semester.SemesterId)).ResultObj;

                return Ok(new
                {
                    supervisor,
                    professions = supervisorProfessions,
                    semester,
                    profession
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data for idea creation");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpPost("ideas/create")]
        public async Task<IActionResult> CreateNewIdea(string Supervisor, int[] Profession, string ProjectEnglishName, string ProjetVietnameseName, string Abbreviation, string Description, string ProjectTags, int Semester, int NumberOfMember, int MaxMember)
        {
            try
            {
                _logger.LogInformation("Creating a new idea");

                Regex regex = new Regex(@"\s+");
                List<GroupIdeaOfSupervisorProfession> groupIdeaProfessions = Profession
                    .Select(professionId => new GroupIdeaOfSupervisorProfession
                    {
                        Profession = new Profession { ProfessionId = professionId }
                    }).ToList();

                bool result = (await _supervisorGroupIdeaService.CreateNewGroupIdeaOfMentor(
                    Supervisor.Trim().ToLower(),
                    groupIdeaProfessions,
                    regex.Replace(ProjectEnglishName.Trim(), " "),
                    regex.Replace(ProjetVietnameseName.Trim(), " "),
                    regex.Replace(Abbreviation.Trim(), " "),
                    regex.Replace(Description.Trim(), " "),
                    regex.Replace(ProjectTags.Trim(), " "),
                    Semester,
                    NumberOfMember,
                    MaxMember)).IsSuccessed;

                return Ok(new { success = true});
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating idea");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpGet("ideas/update/{groupIdeaId}")]
        public async Task<IActionResult> UpdateIdeaView(int groupIdeaId)
        {
            try
            {
                _logger.LogInformation("Fetching data for updating idea");

                User user = _sessionExtensionService.GetObjectFromJson<User>(HttpContext.Session, "sessionAccount");
                Supervisor supervisor =(await _supervisorService.GetSupervisorById(user.UserId)).ResultObj;
                Semester currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj ?? new Semester { SemesterId = 0 };

                List<Profession> supervisorProfessions = currentSemester.SemesterId == 0
                    ? new List<Profession>()
                    : (await _supervisorProfessionService.GetProfessionsBySupervisorID(supervisor.SupervisorId, currentSemester.SemesterId)).ResultObj;

                supervisorProfessions.ForEach(async p =>
                    p.Specialties = (await _specialService.getSpecialtiesByProfessionId(p.ProfessionId, currentSemester.SemesterId)).ResultObj);

                GroupIdeasOfSupervisor groupIdea =  (await _supervisorGroupIdeaService.GetAllGroupIdeaById(groupIdeaId)).ResultObj;

                return Ok(new
                {
                    groupIdea,
                    professions = supervisorProfessions,
                    supervisor
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching data for updating idea");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }
        [HttpPut("ideas/update")]
        public async Task<IActionResult> UpdateIdea(int GroupIdeaID, string Supervisor, int[] Profession, string ProjectEnglishName, string ProjetVietnameseName, string Abbreviation, string Description, string ProjectTags, int Semester, int NumberOfMember, int MaxMember, int status)
        {
            try
            {
                _logger.LogInformation("Updating an idea");

                Regex regex = new Regex(@"\s+");
                List<GroupIdeaOfSupervisorProfession> groupIdeaProfessions = Profession
                    .Select(professionId => new GroupIdeaOfSupervisorProfession
                    {
                        Profession = new Profession { ProfessionId = professionId }
                    }).ToList();

                GroupIdeasOfSupervisor groupIdea = new GroupIdeasOfSupervisor
                {
                    GroupIdeaId = GroupIdeaID,
                    Supervisor = new Supervisor { SupervisorId = Supervisor.Trim().ToLower() },
                    GroupIdeaOfSupervisorProfessions = groupIdeaProfessions,
                    ProjectEnglishName = regex.Replace(ProjectEnglishName.Trim(), " "),
                    ProjectVietNameseName = regex.Replace(ProjetVietnameseName.Trim(), " "),
                    Abbreviation = regex.Replace(Abbreviation.Trim(), " "),
                    Description = regex.Replace(Description.Trim(), " "),
                    ProjectTags = regex.Replace(ProjectTags.Trim(), " "),
                    Semester = new Semester { SemesterId = Semester },
                    NumberOfMember = NumberOfMember,
                    MaxMember = MaxMember,
                    IsActive = status == 1
                };

                bool result = (await _supervisorGroupIdeaService.UpdateAllIdea(groupIdea, Semester)).IsSuccessed;

                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating idea");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpDelete("ideas/delete/{id}")]
        public async Task<IActionResult> DeleteIdea(int id)
        {
            try
            {
                _logger.LogInformation("Deleting an idea");
                bool result = (await _supervisorGroupIdeaService.DeleteGroupIdea(id)).IsSuccessed;

                return Ok(new { success = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting idea");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpPatch("ideas/update-status")]
        public async Task<IActionResult> UpdateStatus(bool status, int ideaid)
        {
            try
            {
                _logger.LogInformation("Updating idea status");
                await _supervisorGroupIdeaService.UpdateStatusOfIdea(ideaid, status);
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating idea status");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

        [HttpGet("specialties/{professionId}")]
        public async Task<IActionResult> GetSpecialty(int professionId)
        {
            try
            {
                Semester currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                var specialties = _specialService.getSpecialtiesByProfessionId(professionId, currentSemester.SemesterId);

                return Ok(specialties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving specialties");
                return StatusCode(500, new { message = "Internal server error" });
            }
        }

    }
}
