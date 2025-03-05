using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.ViewModel.StaffViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SpecialtyService
{
    public interface ISpecialtyService
    {
        Task<ApiResult<List<Specialty>>> getAllSpecialty(int semesterId);
        Task<ApiResult<Specialty>> getSpecialtyById(int specialtyId);
        Task<ApiResult<Specialty>> GetSpecialtyByName(string specialtyFullname, int semesterId);

        Task<ApiResult<Specialty>> GetSpecialtyByName(string specialtyFullname, int semesterId, int professionId);
        Task<ApiResult<List<Specialty>>> getSpecialtiesByProfessionId(int professionId, int semesterId);
        Task<ApiResult<int>> AddSpecialtyThenReturnId(Specialty specialty, int semesterId);
        Task<ApiResult<bool>> UpdateSpecialty(Specialty specialty);
        Task<ApiResult<bool>> UpdateSpecialtyV2(Specialty specialty);

        Task<ApiResult<string>> GetCodeOfGroupNameByGroupIdeaId(int groupIdeaId);
        (int, int, List<SpecialtyWithRowNum>) GetAllSpecialtyForPaging(int semesterId, int pageNumber, string query);
        Task<ApiResult<bool>> RemoveSpecialty(int id);
    }
}
