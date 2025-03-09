using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.SpecialtyService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageMajorController : BaseApiController
    {
        private readonly IProfessionService _professionService;
        private readonly ISpecialtyService _specialtyService;
        private readonly ISemesterService _semesterService;
        private readonly ILogger<ManageMajorController> _logger;

        public ManageMajorController() { }
        public ManageMajorController(IProfessionService professionService,
            ISpecialtyService specialtyService,
            ISemesterService semesterService,
            ILogger<ManageMajorController> logger)
        {
            _professionService = professionService;
            _specialtyService = specialtyService;
            _semesterService = semesterService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetProfessionsAndSpecialties()
        {
            try
            {
                _logger.LogInformation("Fetching all professions and specialties");

                var currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (currentSemester == null)
                {
                    return BadRequest(new ApiResult<string> { IsSuccessed = false, Message = "No active semester found." });
                }

                int semesterId = currentSemester.SemesterID;
                List<ProfessionDto> professionList = (await _professionService.getAllProfession(semesterId)).ResultObj;

                if (professionList == null || professionList.Count == 0)
                {
                    return NotFound(new ApiResult<string> { IsSuccessed = false, Message = "No professions found for this semester." });
                }

                foreach (var pro in professionList)
                {
                    List<SpecialtyDto> specialtyList = (await _specialtyService.getSpecialtiesByProfessionId(pro.ProfessionID, semesterId)).ResultObj;
                    pro.Specialties = specialtyList;
                }

                return Ok(new ApiSuccessResult<List<ProfessionDto>>(professionList));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching professions and specialties");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> UpdateMajor([FromBody] ProfessionDto data)
        {
            try
            {
                _logger.LogInformation("Updating major");

                var currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (currentSemester == null)
                {
                    return BadRequest(new ApiResult<string> { IsSuccessed = false, Message = "No active semester found." });
                }

                int semesterId = currentSemester.SemesterID;
                ProfessionDto profession = data;

                // Thêm mới ngành học (Profession)
                if (profession.ProfessionID == 0)
                {
                    int professionId = (await _professionService.AddProfessionThenReturnId(profession, semesterId)).ResultObj;

                    foreach (var spec in profession.Specialties)
                    {
                        SpecialtyDto specialty = new SpecialtyDto
                        {
                            SpecialtyAbbreviation = spec.SpecialtyAbbreviation,
                            SpecialtyFullName = spec.SpecialtyFullName,
                            CodeOfGroupName = spec.CodeOfGroupName,
                            Profession = new ProfessionDto { ProfessionID = professionId },
                            MaxMember = spec.MaxMember
                        };
                        _specialtyService.AddSpecialtyThenReturnId(specialty, semesterId);
                    }
                }
                // Cập nhật ngành học cũ (Profession)
                else
                {
                    await _professionService.UpdateProfession(profession);

                    foreach (SpecialtyDto spec in profession.Specialties)
                    {
                        if (spec.SpecialtyID == 0) // Thêm mới chuyên ngành
                        {
                            SpecialtyDto specialty = new SpecialtyDto
                            {
                                SpecialtyAbbreviation = spec.SpecialtyAbbreviation,
                                SpecialtyFullName = spec.SpecialtyFullName,
                                CodeOfGroupName = spec.CodeOfGroupName,
                                Profession = new ProfessionDto { ProfessionID = profession.ProfessionID },
                                MaxMember = spec.MaxMember
                            };
                            _specialtyService.AddSpecialtyThenReturnId(specialty, semesterId);
                        }
                        else // Cập nhật chuyên ngành cũ
                        {
                            SpecialtyDto specialty = new SpecialtyDto
                            {
                                SpecialtyID = spec.SpecialtyID,
                                SpecialtyAbbreviation = spec.SpecialtyAbbreviation,
                                SpecialtyFullName = spec.SpecialtyFullName,
                                CodeOfGroupName = spec.CodeOfGroupName,
                                Profession = new ProfessionDto { ProfessionID = profession.ProfessionID },
                                MaxMember = spec.MaxMember
                            };
                            _specialtyService.UpdateSpecialty(specialty);
                        }
                    }
                }

                return Ok(new ApiSuccessResult<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating major");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }
        [HttpPost]
        [Route("/UpdateMajorV2")]
        public async Task<IActionResult> UpdateMajorV2([FromBody] ProfessionDto data)
        {
            try
            {
                _logger.LogInformation("Updating major (V2)");

                var currentSemester = (await _semesterService.GetCurrentSemester()).ResultObj;
                if (currentSemester == null)
                {
                    return BadRequest(new ApiResult<string> { IsSuccessed = false, Message = "No active semester found." });
                }

                int semesterId = currentSemester.SemesterID;
                data.Semester = new SemesterDto { SemesterID = semesterId };

                // Cập nhật hoặc thêm mới ngành học (Profession)
                int professionId = (await _professionService.UpdateProfessionV2(data)).ResultObj;

                foreach (SpecialtyDto spec in data.Specialties)
                {
                    SpecialtyDto specialty = new SpecialtyDto
                    {
                        SpecialtyAbbreviation = spec.SpecialtyAbbreviation,
                        SpecialtyFullName = spec.SpecialtyFullName,
                        CodeOfGroupName = spec.CodeOfGroupName,
                        Profession = new ProfessionDto { ProfessionID = professionId },
                        MaxMember = spec.MaxMember,
                        Semester = new SemesterDto { SemesterID = semesterId }
                    };

                    _specialtyService.UpdateSpecialtyV2(specialty);
                }

                return Ok(new ApiSuccessResult<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating major (V2)");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }

        [HttpDelete("delete-profession/{id}")]
        public IActionResult DeleteProfession(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting profession with ID: {id}");
                _professionService.DeleteProfession(id);
                return Ok(new ApiSuccessResult<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profession");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }

        [HttpDelete("remove-profession/{id}")]
        public IActionResult RemoveProfession(int id)
        {
            try
            {
                _logger.LogInformation($"Removing profession with ID: {id}");
                _professionService.RemoveProfession(id);
                return Ok(new ApiSuccessResult<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing profession");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }

        [HttpDelete("remove-specialty/{id}")]
        public IActionResult RemoveSpecialty(int id)
        {
            try
            {
                _logger.LogInformation($"Removing specialty with ID: {id}");
                _specialtyService.RemoveSpecialty(id);
                return Ok(new ApiSuccessResult<bool>(true));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing specialty");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }

    }
}