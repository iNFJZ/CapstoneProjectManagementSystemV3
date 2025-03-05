using Infrastructure.Entities.Common;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.StudentDto;

namespace Infrastructure.Services.PrivateService.StudentGroupIdeaService
{
    public interface IStudentGroupIdeaService
    {
        //NguyenLH
        Task<ApiResult<string>> GetLeaderIdByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<List<string>>> GetMemberIdByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<List<JoinRequest>>> GetAllJoinRequestByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<string>> GetGroupIdByStudentId(string studentId);
        Task<ApiResult<List<StudentGroupIdea>>> GetListRequestByStudentId(string studentId);
        Task<ApiResult<bool>> UpdateStatusToAccept(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> UpdateStatusToReject(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> UpdateStatusToMember(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> UpdateStatusToLeader(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> DeleteRecord(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> DeleteAllRequest(string studentId);
        Task<ApiResult<bool>> DeleteAllRecordOfGroupIdea(int groupIdeaId);
        Task<ApiResult<bool>> DeleteRecordHaveStatusEqual3or4or5OfGroupIdea(int groupIdeaId);
        //NguyenNH
        Task<ApiResult<int>> FilterPermissionOfStudent(string studentId);
        Task<ApiResult<bool>> FilterStudentHaveIdea(string studentId, int semesterId);
        Task<ApiResult<GroupIdeaDto>> GetGroupIdeaOfStudent(string studentId, int status);
        Task<ApiResult<List<Student>>> GetStudentsHadOneGroupIdea(int groupIdea);
        Task<ApiResult<bool>> CheckAddedStudentIsValid(string fptEmail);
        Task<ApiResult<bool>> CreateGroupIdea(GroupIdea groupIdea, string studentId, int semesterId, int maxMember);
        Task<ApiResult<bool>> AddRecord(string studentId, int groupId, int status, string message);
        Task<ApiResult<bool>> DeleteGroupIdeaAndStudentInGroupIdea(int groupIdeaId);
        Task<ApiResult<StudentGroupIdea>> GetStudentGroupIdeaByGroupIdeaIdAndFptEmail(int groupIdeaId, string fptEmail);
        Task<ApiResult<List<StudentGroupIdea>>> GetListStudentInGroupByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<List<StudentGroupIdeaDto>>> GetInforStudentInGroupIdeaBySemester(int groupIdeaId, int semesterId); // thuantv8 add
        Task<ApiResult<List<StudentGroupIdeaDto>>> GetInforStudentInGroupIdea(int groupIdeaId);
        Task<ApiResult<bool>> RecoveryStudentInGroupIdeaAfterRejected(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> AddNewMembersToGroup(int groupIdeaId, string studentIds);
        Task<ApiResult<bool>> DeleteMembersFromGroup(int groupIdeaId, string studentIds);
    }
}
