using DocumentFormat.OpenXml.Wordprocessing;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.FinalGroupRepository
{
    public interface IFinalGroupRepository : IRepositoryBase<FinalGroup>
    {
        Task<List<FinalGroupDto>> getAllFinalGroups(int semesterId);
        Task<FinalGroupDto> getFinalGroupById(int id);
        Task<bool> ConfirmFinalReport(FinalGroupDto finalGroup);
        Task<List<FinalGroupDto>> GetFullMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber);
        Task<List<FinalGroupDto>> GetLackOfMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber);
        Task<int> GetMaxMemberOfFinalGroupByGroupName(string groupName, int semesterId);
        Task UpdateNumberOfMemberWhenAdd(int groupId);
        Task<FinalGroupDto> GetFinalGroupByStudentIsLeader(string studentId, int semesterId);
        Task<FinalGroupDto> GetOldTopicByFinalGroupId(int finalGroupId);
        Task<List<StudentDto>> GetListCurrentMemberHaveFinalGroupByGroupName(string groupName, int semesterId);
        Task<string> GetLatestGroupName(string codeOfGroupName);
        Task<List<FinalGroupDto>> GetListFinalGroupBySupervisorID(string supervisorId, int semesterId);
        Task<FinalGroupDto> GetFinalGroupDetailForSupervisor(int finalGroupId);
        Task<List<StudentDto>> GetListMemberByFinalGroupId(int finalGroupId);

    }
}
