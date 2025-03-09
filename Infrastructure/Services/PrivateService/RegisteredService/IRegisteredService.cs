using ClosedXML.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.StudentViewModel;
using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.RegisteredService
{
    public interface IRegisteredService
    {
        Task<ApiResult<bool>> AddRegisteredGroup(RegisteredGroupDto registeredGroup, List<RegisteredGroupSupervisorForSubmitRegistration> supervisorSubmitRegist);
        Task<ApiResult<RegisteredGroupDto>> GetRegisteredGroupByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<RegisteredGroupDto>> GetRegisteredGroupsBySearch(int semesterId, int status, string searchText, int offsetNumber, int fetchNumber);
        Task<ApiResult<int>> CountRecordRegisteredGroupSearchList(int semesterId, int status, string searchText);
        Task<ApiResult<bool>> UpdateStatusByRegisteredGroupID(int registeredGroupID);

        Task<ApiResult<bool>> UpdateStatusOfRegisteredGroup(int registeredGroupId, int status);
        Task<ApiResult<RegisteredGroupDto>> GetDetailRegistrationOfStudentByGroupIdeaId(int registeredGroupId);
        Task<ApiResult<bool>> UpdateStaffCommentByRegisteredGroupID(string staffComment, int registeredGroupId);
        Task<ApiResult<RegisteredGroupDto>> GetGroupIDByRegisteredGroupId(int registeredGroupId);
        Task<ApiResult<bool>> DeleteRecord(int id);

        Task<ApiResult<bool>> DeleteRegisteredGroup(int registeredGroupId);
        Task<ApiResult<bool>> RejectWhenRegisteredGroupAccepted(int registeredGroupID, string commentReject, int groupIdeaId, int finalGroupId);

        (int, int, List<RegisteredGroupRequest>) GetListRegisteredGroupRequests(string supervisorsIds, int offsetRow, int fetchNumber, int[] professions, bool isBeforeDeadline, int filterIsAssigned);

        Task<ApiResult<RegisteredGroupRequest>> GetRegisteredGroupRequest(int registeredGroupId);
        Task<Supervisor> GetAssignedSupervisor(int registeredGroupId);

        Task<ApiResult<bool>> UpdateRegisteredGroupSupervisor(int registeredGroupId, string supervisorId, bool isAssigned);

        Task<ApiResult<bool>> DeleteRegisteredGroupSupervisor(int registeredGroupId, string supervisorId);

        Task<ApiResult<bool>> AddRegisteredGroupSupervisor(int registeredGroupId, string supervisorId, int status);

        Task<ApiResult<XLWorkbook>> CreateWorkBookAllRegisteredGroupList(int currentSemesterId);

        Task<ApiResult<XLWorkbook>> CreateWorkBookAllUnsubmittedGroup(int currentSemesterId);

        (int, int, List<RegisteredGroupDto>) GetListRegisteredGroupsForSupervisor(int semester_ID, string supervisor_ID, string search, int pageNumber, int[] statuses);
        Task<ApiResult<RegisteredGroupRequest>> GetRegisteredGroupForSupervisorById(int registeredGroupId, string supervisorId);

        Task<ApiResult<bool>> UpdateStatusRegisterGroupForSupervisor(int status, int registeredGroupID, string supervisorID);

        Task<List<RegisteredGroupRequest>> GetAssignedGroup(string supervisorIds, int[] professions);

        Task<ApiResult<List<StudentDto>>> GetRegisteredGroupMember(string studentId, bool isLeader);

        Task<ApiResult<XLWorkbook>> CreateWorkBookBasedOnRegisterGroupList(List<RegisteredGroupRequest> registeredGroupRequests, int currentRow, List<Profession> devHeadProfessions);

        (List<RegisteredGroupRequest>, List<RegisteredGroupRequest>, List<RegisteredGroupRequest>, List<string>, List<int>) AssignMentorsBasedOnRegisterGroupWorkSheet(IXLWorksheet worksheet, int startRow, List<ProfessionDto> professions);

        Task<ApiResult<bool>> CheckRegisteredGroupAccept();

        Task<ApiResult<RegisteredGroupRequest>> GetRegisteredGroupRequestIncludingAllSupervisor(int registeredGroupId);
    }
}
