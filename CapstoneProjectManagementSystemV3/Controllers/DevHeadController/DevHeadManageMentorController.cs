using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using ClosedXML.Excel;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.CommonServices.SessionExtensionService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.SupervisorService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.DevHeadController
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevHeadManageMentorController : BaseApiController
    {
        private readonly ISupervisorService _supervisorService;
        private readonly ISemesterService _semesterService;
        //private readonly ISessionExtensionService sessionExtensionService;
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly IProfessionService _professionService;
        private readonly ILogger<DevHeadManageMentorController> _logger;
        public DevHeadManageMentorController(ISupervisorService supervisorService,
            ISemesterService semesterService,
            IDataRetrievalService dataRetrievalService,
            IProfessionService professionService,
            ILogger<DevHeadManageMentorController> logger)
        {
            _supervisorService = supervisorService;
            _semesterService = semesterService;
            _dataRetrievalService = dataRetrievalService;
            _professionService = professionService;
            _logger = logger;
        }
        [HttpGet("Index")]
        public async Task<IActionResult> GetManageMentorPage()
        {
            try
            {
                _logger.LogInformation("View manage mentor page");
                User user = _dataRetrievalService.GetData<User>("sessionAccount");
                SupervisorDto supervisor = (await _supervisorService.GetSupervisorByUserId(user.UserId)).ResultObj;
                List<ProfessionDto> professions = (await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(supervisor.SupervisorID, true)).ResultObj;

                var response = new
                {
                    DevheadProfessions = professions
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "View manage mentor page error");
                return StatusCode(500, new { error = "Internal Server Error", message = ex.Message });
            }
        }

        // Import Supervisor List (converted to an API)
        [HttpPost("ImportSupervisorList")]
        public async Task<IActionResult> ImportSupervisorList([FromForm] IFormFile file)
        {
            _logger.LogInformation("Import supervisor list");

            if (file == null || file.Length == 0)
            {
                return BadRequest(new { message = "No file uploaded." });
            }

            try
            {
                XLWorkbook workbook = new XLWorkbook(file.OpenReadStream());
                User user = _dataRetrievalService.GetData<User>("sessionAccount");
                SupervisorDto devhead = (await _supervisorService.GetSupervisorByUserId(user.UserId)).ResultObj;
                var sheet = workbook.Worksheets.Worksheet(1);
                List<SupervisorDto> supervisors;
                List<ProfessionDto> professions = (await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(devhead.SupervisorID, true)).ResultObj;
                List<string> errorMessages;
                List<int> errorLineNumbers;

                (supervisors, errorMessages, errorLineNumbers) = (await _supervisorService.CreateSupervisorListBasedOnWorkSheet(sheet, 10, professions)).ResultObj;

                if (errorMessages != null && errorMessages.Count > 0)
                {
                    return BadRequest(new { message = "There were errors in the imported data.", errors = errorMessages });
                }

                var importResult = (await _supervisorService.ImportSupervisorList(supervisors, devhead.SupervisorID)).ResultObj;
                return Ok(new
                {
                    AddedSupervisors = importResult.GetValueOrDefault("addedSupervisors"),
                    UpdatedSupervisors = importResult.GetValueOrDefault("updatedSupervisors")
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Import supervisor list error");
                return StatusCode(500, new { error = "Internal Server Error", message = ex.Message });
            }
        }

        // Get template supervisor list (converted to an API)
        [HttpGet("GetTemplateSupervisorList")]
        public async Task<IActionResult> GetTemplateSupervisorList()
        {
            try
            {
                _logger.LogInformation("Get template of supervisor list");
                SemesterDto currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                UserDto user = _dataRetrievalService.GetData<UserDto>("sessionAccount");
                SupervisorDto supervisor = (await _supervisorService.GetSupervisorByUserId(user.UserID)).ResultObj;
                List<ProfessionDto> professions = (await _professionService.GetProfessionsBySupervisorIdAndIsDevHead(supervisor.SupervisorID, true)).ResultObj;
                ProfessionDto profession = professions.Count != 0 ? professions[0] : null;
                List<SupervisorDto> supervisors = new List<SupervisorDto>
            {
                new SupervisorDto()
                {
                    SupervisorID = "chienbd",
                    FeEduEmail = "chienbd",
                    PhoneNumber = "0844427705",
                    SupervisorNavigation = new UserDto() { FullName = "Bùi Đình Chiến", Gender = 1 },
                    SupervisorProfessions = new List<SupervisorProfessionDto>
                    {
                        new SupervisorProfessionDto() { Profession = profession, MaxGroup = 3 }
                    },
                    IsActive = true,
                }
            };

                XLWorkbook workbook = (await _supervisorService.CreateWorkBookBasedOnSupervisorList(supervisors, 9, supervisor.SupervisorID)).ResultObj;
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        $"{currentSemester.SemesterCode}-Template Supervisor List.xlsx");
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Get template of supervisor list error");
                return StatusCode(500, new { error = "Internal Server Error", message = exception.Message });
            }
        }
    }
}