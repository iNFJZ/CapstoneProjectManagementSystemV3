using ClosedXML.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorService
{
    public interface ISupervisorService
    {
        Task<ApiResult<XLWorkbook>> CreateWorkBookBasedOnSupervisorList(List<Supervisor> supervisors, int currentRow, string supervisorId);

        Task<ApiResult<(List<Supervisor>, List<string>, List<int>)>> CreateSupervisorListBasedOnWorkSheet(IXLWorksheet worksheet, int startRow, List<Profession> professions);

        Task<ApiResult<Dictionary<string, List<Supervisor>>>> ImportSupervisorList(List<Supervisor> supervisors, string devHeadId);

        Task<ApiResult<Supervisor>> GetSupervisorById(string supervisorID);

        Task<ApiResult<List<Supervisor>>> GetAllSupervisor();

        Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorForPaging(int page, string search, string userId);

        Task<ApiResult<bool>> UpdateInforProfileOfSupervisor(SupervisorDto supervisor);

        Task<ApiResult<Supervisor>> GetProfileOfSupervisorByUserId(string userId);

        Task<ApiResult<Supervisor>> GetSupervisorByUserId(string userId);

        Task<ApiResult<bool>> checkDuplicateFEEduEmail(string feEduEmail);

        Task<ApiResult<List<Supervisor>>> GetDevHeadByProfessionID(int professionID);

        Task<ApiResult<List<Supervisor>>> getListSupervisorForRegistration(int professionID, int semesterId);


        Task<ApiResult<List<SupervisorForAssigning>>> GetSupervisorsForAssigning(int[] professions, int professionOfGroupIdea, int registerGroupId);

        Task<ApiResult<(int, int, List<Supervisor>)>> GetListDevheadForStaffPaging(int pageNumber, string search, int professionId);

        Task<ApiResult<Supervisor>> GetDevheadDetailForStaff(string devheadId, int semesterId);

        Task<ApiResult<bool>> UpdateDevhead(Supervisor devhead);

        Task<ApiResult<List<SupervisorProfession>>> GetListSupervisorProfessionBySupervisorId(string supervisorId);

        Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorForPagingForStudent
                                                (int pageNumber, int professionId, string search);

        Task<ApiResult<(int, int, List<SupervisorWithRowNum>)>> GetListSupervisorPagingForDevHead
                                             (int pageNumber, string search, string userId, int status);

        Task<ApiResult<bool>> UpdateStatusForSupervisor(bool status, string supervisorId);

        Task<ApiResult<Supervisor>> GetProfileOfSupervisorByUserIdFullPro(string userid);
    }
}
