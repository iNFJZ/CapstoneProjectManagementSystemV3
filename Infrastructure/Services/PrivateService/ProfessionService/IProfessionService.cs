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
        Task<ApiResult<List<Profession>>> getAllProfession(int semesterId);
        Task<ApiResult<Profession>> getProfessionById(int professionId);
        Task<ApiResult<Profession>> GetProfessionByName(string professionFullname, int semesterId);
        Task<ApiResult<int>> AddProfessionThenReturnId(Profession profession, int semesterId);
        Task<ApiResult<bool>> UpdateProfession(Profession profession);
        Task<ApiResult<int>> UpdateProfessionV2(Profession profession);
        Task<ApiResult<bool>> DeleteProfession(int id);

        Task<ApiResult<bool>> RemoveProfession(int id);

        Task<ApiResult<List<Profession>>> GetProfessionsBySupervisorIdAndIsDevHead(string supervisorId, bool isDevHead);
        Task<ApiResult<List<Profession>>> GetAllProfessionWithSpecialty(int semesterId);
        Task<ApiResult<Profession>> GetProfessionBySpecialty(int specialtyId);
    }
}
