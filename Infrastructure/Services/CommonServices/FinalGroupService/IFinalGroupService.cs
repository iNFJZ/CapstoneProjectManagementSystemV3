using Infrastructure.Entities.Common;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Entities.Common.ApiResult;
using Microsoft.AspNetCore.Http;
using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;

namespace Infrastructure.Services.CommonServices.FinalGroupService
{
    public interface IFinalGroupService
    {
        Task<ApiResult<List<FinalGroupDto>>> getAllFinalGroups(int semester_Id);
        Task<ApiResult<FinalGroupDto>> getFinalGroupById(int id);
        Task<ApiResult<List<FinalGroupDto>>> GetLackOfMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber);
        Task<ApiResult<List<FinalGroupDto>>> GetFullMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber);
        Task<ApiResult<bool>> UpdateNumberOfMemberWhenAdd(int groupId);
        Task<ApiResult<bool>> UpdateNumberOfMemberWhenRemove(int groupId);
        Task<ApiResult<bool>> UpdateGroupName(int groupId, string groupName);
        Task<ApiResult<bool>> DeleteFinalGroup(int finalGroupId);
        Task<ApiResult<bool>> CreateFinalGroup(int semesterId, int professionId, int specilatyId, string groupName, string englishName, string abbreviation, string vietnameseName, int maxMember, int numberOfMember);

        //NguyenNH
        Task<ApiResult<bool>> AddRegisteredGroupToFinalGroup(RegisteredGroupRequest registeredGroupRequest, string groupName);
        Task<ApiResult<FinalGroupDto>> GetFinalGroupByStudentIsLeader(string studentId, int semesterId);
        Task<ApiResult<FinalGroupDto>> GetOldTopicByGroupName(int finalGroupId);
        Task<ApiResult<List<StudentDto>>> GetListCurrentMemberHaveFinalGroupByGroupName(string groupName, int semesterId);
        Task<ApiResult<int>> GetMaxMemberOfFinalGroupByGroupName(string groupName, int semesterId);
        Task<ApiResult<bool>> UpdateNewTopicForFinalGroup(ChangeTopicRequest changeTopicRequest);
        Task<ApiResult<string>> GetLatestGroupName(string codeOfGroupName);
        Task<ApiResult<List<FinalGroupDto>>> GetListFinalGroupBySupervisorID(string supervisorId, int semesterId);
        Task<ApiResult<bool>> ConfirmFinalReport(FinalGroupDto finalGroup);

        Task<ApiResult<FinalGroupDto>> getFinalGroupDetailForSupervisor(int finalGroupId);

        Task<ApiResult<List<StudentDto>>> GetListMemberByFinalGroupId(int finalGroupId);
    }
}
