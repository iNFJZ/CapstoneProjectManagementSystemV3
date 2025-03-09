using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageDevheadController : BaseApiController
    {
        private readonly ISupervisorService _supervisorService;
        private readonly IProfessionService _professionService;
        private readonly ISemesterService _semesterService;
        private readonly ILogger<ManageDevheadController> _logger;
        public ManageDevheadController(ISupervisorService supervisorService,
            IProfessionService professionService,
            ISemesterService semesterService,
            ILogger<ManageDevheadController> logger)
        {
            _supervisorService = supervisorService;
            _professionService = professionService;
            _semesterService = semesterService;
            _logger = logger;
        }
        [HttpGet("get-manage-devhead")]
        public async Task<IActionResult> GetManageDevhead(int page = 1, string search = "", int professionId = 0)
        {
            try
            {
                _logger.LogInformation("Fetching devhead management data");

                int totalPage = 0;
                int semesterId = (await _semesterService.GetCurrentSemester()).ResultObj.SemesterID;

                List<ProfessionDto> professions = (await _professionService.getAllProfession(semesterId)).ResultObj;
                List<SupervisorDto> supervisors;
                (totalPage, page, supervisors) = (await _supervisorService.GetListDevheadForStaffPaging(page, search.Trim(), professionId)).ResultObj;

                var response = new
                {
                    TotalPage = totalPage,
                    PageIndex = page,
                    Search = search,
                    ProfessionId = professionId,
                    Professions = professions,
                    Supervisors = supervisors
                };

                return Ok(new ApiSuccessResult<object>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching devhead management data");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }
        [HttpGet("get-devhead-detail/{devheadId}")]
        public async Task<IActionResult> GetDevheadDetail(string devheadId)
        {
            try
            {
                _logger.LogInformation($"Fetching devhead details for {devheadId}");

                int semesterId = (await _semesterService.GetCurrentSemester()).ResultObj.SemesterID;
                List<ProfessionDto> professions = (await _professionService.getAllProfession(semesterId)).ResultObj;
                SupervisorDto supervisor = (await _supervisorService.GetDevheadDetailForStaff(devheadId, semesterId)).ResultObj;

                var response = new
                {
                    Professions = professions,
                    Supervisor = supervisor
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching devhead details");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Cập nhật thông tin Devhead
        /// </summary>
        [HttpPost("update-devhead")]
        public async Task<IActionResult> UpdateDevhead([FromBody] SupervisorDto supervisor)
        {
            try
            {
                _logger.LogInformation("Updating devhead");

                string feEduEmail = (await _supervisorService.GetSupervisorById(supervisor.SupervisorNavigation.UserID)).ResultObj.FeEduEmail.ToLower();
                bool checkDuplicateFEMail = !feEduEmail.Equals(supervisor.FeEduEmail.Trim().ToLower()) &&
                                            !string.IsNullOrEmpty(supervisor.FeEduEmail) &&
                                            (await _supervisorService.checkDuplicateFEEduEmail(supervisor.FeEduEmail.Trim().ToLower())).ResultObj;

                if (checkDuplicateFEMail)
                {
                    return Conflict(new { Message = "Duplicate FE email" }); // HTTP 409 Conflict
                }

                bool updateSuccess = (await _supervisorService.UpdateDevhead(supervisor)).ResultObj;

                if (updateSuccess)
                {
                    return Ok(new { Message = "Update successful", Status = 1 });
                }

                return BadRequest(new { Message = "Update failed", Status = 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating devhead");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}

