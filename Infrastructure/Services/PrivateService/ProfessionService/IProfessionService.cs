using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ProfessionService
{
    public interface IProfessionService
    {
        Task<ApiResult<List<ProfessionDto>>> getAllProfession(int semesterId);
        Task<ApiResult<ProfessionDto>> getProfessionById(int professionId);
        Task<ApiResult<ProfessionDto>> GetProfessionByName(string professionFullname, int semesterId);
        Task<ApiResult<int>> AddProfessionThenReturnId(ProfessionDto profession, int semesterId);
        Task<ApiResult<bool>> UpdateProfession(ProfessionDto profession);
        Task<ApiResult<int>> UpdateProfessionV2(ProfessionDto profession);
        Task<ApiResult<bool>> DeleteProfession(int id);

        Task<ApiResult<bool>> RemoveProfession(int id);

        Task<ApiResult<List<ProfessionDto>>> GetProfessionsBySupervisorIdAndIsDevHead(string supervisorId, bool isDevHead);
        Task<ApiResult<List<ProfessionDto>>> GetAllProfessionWithSpecialty(int semesterId);
        Task<ApiResult<ProfessionDto>> GetProfessionBySpecialty(int specialtyId);
    }
}