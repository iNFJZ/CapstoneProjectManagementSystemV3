using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.StaffViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SpecialtyService
{
    public interface ISpecialtyService
    {
        Task<ApiResult<List<SpecialtyDto>>> getAllSpecialty(int semesterId);
        Task<ApiResult<SpecialtyDto>> getSpecialtyById(int specialtyId);
        Task<ApiResult<SpecialtyDto>> GetSpecialtyByName(string specialtyFullname, int semesterId);

        Task<ApiResult<SpecialtyDto>> GetSpecialtyByName(string specialtyFullname, int semesterId, int professionId);
        Task<ApiResult<List<SpecialtyDto>>> getSpecialtiesByProfessionId(int professionId, int semesterId);
        Task<ApiResult<int>> AddSpecialtyThenReturnId(SpecialtyDto specialty, int semesterId);
        Task<ApiResult<bool>> UpdateSpecialty(SpecialtyDto specialty);
        Task<ApiResult<bool>> UpdateSpecialtyV2(SpecialtyDto specialty);

        Task<ApiResult<string>> GetCodeOfGroupNameByGroupIdeaId(int groupIdeaId);
        (int, int, List<SpecialtyWithRowNum>) GetAllSpecialtyForPaging(int semesterId, int pageNumber, string query);
        Task<ApiResult<bool>> RemoveSpecialty(int id);
    }
}
