using ClosedXML.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.RegisteredGroupSupervisorRepository;
using Infrastructure.Repositories.RegisteredRepository;
using Infrastructure.ViewModel.StudentViewModel;
using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.RegisteredService
{
    public class RegisteredService : IRegisteredService
    {
        private readonly IRegisteredRepository _registeredRepository;
        private readonly IRegisteredGroupSupervisorRepository _registeredGroupSupervisorRepository;

        public RegisteredService(IRegisteredRepository registeredRepository,
            IRegisteredGroupSupervisorRepository registeredGroupSupervisorRepository)
        {
            _registeredRepository = registeredRepository;
            _registeredGroupSupervisorRepository = registeredGroupSupervisorRepository;
        }

        public async Task<ApiResult<bool>> AddRegisteredGroup(RegisteredGroup registeredGroup, List<RegisteredGroupSupervisorForSubmitRegistration> supervisorSubmitRegist)
        {
            try
            {
                var registeredGroups = new RegisteredGroup()
                {
                    GroupIdeaId = registeredGroup.GroupIdeaId,
                    StudentComment = registeredGroup.StudentComment,
                    StudentsRegistraiton = registeredGroup.StudentsRegistraiton
                };
                await _registeredRepository.CreateAsync(registeredGroups);
                foreach (RegisteredGroupSupervisorForSubmitRegistration rg in supervisorSubmitRegist)
                {
                    if (rg.supervisor_ID != null)
                    {
                        var groupIdeaId = (await _registeredRepository.GetById(rg => rg.GroupIdeaId == registeredGroup.GroupIdeaId && rg.DeletedAt == null)).RegisteredGroupId;
                        var reisterGroupSupervisor = new RegisterdGroupSupervisor()
                        {
                            RegisteredGroupId = groupIdeaId,
                            SupervisorId = rg.supervisor_ID,
                            GroupIdeaOfSupervisorId = rg.groupIdeaOfSupervior_ID,
                            Status = 0
                        };
                        await _registeredGroupSupervisorRepository.CreateAsync(reisterGroupSupervisor);
                    }

                }
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiSuccessResult<bool>(false);
            }
        }


        public async Task<ApiResult<bool>> AddRegisteredGroupSupervisor(int registeredGroupId, string supervisorId, int status)
        {
            var reisterGroupSupervisor = new RegisterdGroupSupervisor()
            {
                RegisteredGroupId = registeredGroupId,
                SupervisorId = supervisorId,
                Status = status
            };
            await _registeredGroupSupervisorRepository.CreateAsync(reisterGroupSupervisor);
            return new ApiSuccessResult<bool>(true);
        }

        public (List<RegisteredGroupRequest>, List<RegisteredGroupRequest>, List<RegisteredGroupRequest>, List<string>, List<int>) AssignMentorsBasedOnRegisterGroupWorkSheet(IXLWorksheet worksheet, int startRow, List<Profession> professions)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> CheckRegisteredGroupAccept()
        {
            var resul = await _registeredRepository.GetByCondition(rg => rg.Status == 1 && rg.DeletedAt == null);
            if(resul.Count() == 0)
            {
                return new ApiSuccessResult<bool>(false);
            }
            return new ApiSuccessResult<bool>(true);
        }

        public Task<ApiResult<int>> CountRecordRegisteredGroupSearchList(int semesterId, int status, string searchText)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<XLWorkbook>> CreateWorkBookAllRegisteredGroupList(int currentSemesterId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<XLWorkbook>> CreateWorkBookAllUnsubmittedGroup(int currentSemesterId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<XLWorkbook>> CreateWorkBookBasedOnRegisterGroupList(List<RegisteredGroupRequest> registeredGroupRequests, int currentRow, List<Profession> devHeadProfessions)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> DeleteRecord(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> DeleteRegisteredGroup(int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> DeleteRegisteredGroupSupervisor(int registeredGroupId, string supervisorId)
        {
            throw new NotImplementedException();
        }

        public Task<List<RegisteredGroupRequest>> GetAssignedGroup(string supervisorIds, int[] professions)
        {
            throw new NotImplementedException();
        }

        public Task<Supervisor> GetAssignedSupervisor(int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroup>> GetDetailRegistrationOfStudentByGroupIdeaId(int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroup>> GetGroupIDByRegisteredGroupId(int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public (int, int, List<RegisteredGroupRequest>) GetListRegisteredGroupRequests(string supervisorsIds, int offsetRow, int fetchNumber, int[] professions, bool isBeforeDeadline, int filterIsAssigned)
        {
            throw new NotImplementedException();
        }

        public (int, int, List<RegisteredGroup>) GetListRegisteredGroupsForSupervisor(int semester_ID, string supervisor_ID, string search, int pageNumber, int[] statuses)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroup>> GetRegisteredGroupByGroupIdeaId(int groupIdeaId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroupRequest>> GetRegisteredGroupForSupervisorById(int registeredGroupId, string supervisorId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<Student>>> GetRegisteredGroupMember(string studentId, bool isLeader)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroupRequest>> GetRegisteredGroupRequest(int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroupRequest>> GetRegisteredGroupRequestIncludingAllSupervisor(int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<RegisteredGroup>> GetRegisteredGroupsBySearch(int semesterId, int status, string searchText, int offsetNumber, int fetchNumber)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> RejectWhenRegisteredGroupAccepted(int registeredGroupID, string commentReject, int groupIdeaId, int finalGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateRegisteredGroupSupervisor(int registeredGroupId, string supervisorId, bool isAssigned)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateStaffCommentByRegisteredGroupID(string staffComment, int registeredGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateStatusByRegisteredGroupID(int registeredGroupID)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateStatusOfRegisteredGroup(int registeredGroupId, int status)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateStatusRegisterGroupForSupervisor(int status, int registeredGroupID, string supervisorID)
        {
            throw new NotImplementedException();
        }
    }
}
