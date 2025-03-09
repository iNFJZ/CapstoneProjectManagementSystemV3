using CapstoneProjectManagementSystemV3.Controllers.CommonController;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.SemesterService;
using Infrastructure.Services.PrivateService.ConfigurationService;
using Infrastructure.Services.PrivateService.ProfessionService;
using Infrastructure.Services.PrivateService.SpecialtyService;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Entities.Dto.ViewModel.StaffViewModel;

namespace CapstoneProjectManagementSystemV3.Controllers.StaffController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageConfigController : BaseApiController
    {
        private readonly IProfessionService _professionService;
        private readonly ISpecialtyService _specialtyService;
        private readonly ISemesterService _semesterService;
        private readonly IConfigurationService _configurationService;
        private readonly ILogger<ManageConfigController> _logger;
        public ManageConfigController() { }
        public ManageConfigController(IProfessionService professionService,
            ISpecialtyService specialtyService,
            ISemesterService semesterService,
            IConfigurationService configurationService,
            ILogger<ManageConfigController> logger)
        {
            _professionService = professionService;
            _specialtyService = specialtyService;
            _semesterService = semesterService;
            _configurationService = configurationService;
            _logger = logger;
        }
        [HttpGet("get-config-profession")]
        public async Task<IActionResult> GetConfigProfession(int page = 1, string query = "")
        {
            try
            {
                _logger.LogInformation("Fetching configuration for professions");

                int totalPage = 0;
                var semester = (await _semesterService.GetCurrentSemester()).ResultObj;
                List<ProfessionDto> professions = (await _professionService.getAllProfession(semester.SemesterID)).ResultObj;
                List<SpecialtyDto> specialties1 = (await _specialtyService.getAllSpecialty(semester.SemesterID)).ResultObj;
                List<SpecialtyWithRowNum> specialties2;

                query = query ?? "";
                (totalPage, page, specialties2) = _specialtyService.GetAllSpecialtyForPaging(semester.SemesterID, page, query.Trim());

                List<WithDto> listWith = new List<WithDto>();
                foreach (var profession in professions)
                {
                    List<SpecialtyDto> spes = new List<SpecialtyDto>();
                    foreach (var specialty in specialties1)
                    {
                        listWith = (await _configurationService.GetWithsBySpecialtyID(specialty.SpecialtyID)).ResultObj;
                        specialty.listWith = listWith ?? new List<WithDto>();
                        if (specialty.Profession.ProfessionID == profession.ProfessionID)
                        {
                            spes.Add(specialty);
                        }
                    }
                    profession.Specialties = spes;
                }

                //foreach (SpecialtyWithRowNum specialty in specialties2)
                //{
                //    specialty.listWith =(await _configurationService.GetWithsBySpecialtyID(specialty.SpecialtyID)).ResultObj ?? new List<WithDto>();
                //}

                var response = new
                {
                    TotalPage = totalPage,
                    PageIndex = page,
                    Query = query,
                    Specialties = specialties2,
                    Professions = professions
                };

                return Ok(new ApiSuccessResult<object>(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching configuration for professions");
                return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
            }
        }
        //[HttpPost("insert-config-profession")]
        //public async Task<IActionResult> InsertConfigProfession(int Specialty, int[] WithProfession, int[] WithSpecialty)
        //{
        //    try
        //    {
        //        _logger.LogInformation("Inserting configuration for profession");

        //        int result = 0;
        //        List<With> Withs = new List<With>();

        //        foreach (int withProfession in WithProfession)
        //        {
        //            foreach (int withSpecialty in WithSpecialty)
        //            {
        //                Specialty specialty = (await _specialtyService.getSpecialtyById(withSpecialty)).ResultObj;
        //                if (specialty.Profession.ProfessionId == withProfession)
        //                {
        //                    Withs.Add(new With
        //                    {
        //                        Profession = new Profession { ProfessionId = withProfession },
        //                        Specialty = specialty
        //                    });
        //                }
        //            }
        //        }

        //        Profession profession = (await _professionService.GetProfessionBySpecialty(Specialty)).ResultObj;
        //        await _configurationService.Insert(Specialty, Withs, profession.ProfessionId);

        //        return Ok(new ApiSuccessResult<bool>(true));
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error inserting configuration for profession");
        //        return StatusCode(500, new ApiResult<string> { IsSuccessed = false, Message = "Internal server error" });
        //    }
        //}
    }
}