using ClosedXML.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.StudentService
{
    public interface IStudentService
    {
        Task<ApiResult<bool>> UpdateStudentByGroupId(int finalGroupId, string groupName, int status, string studentId);
        Task<ApiResult<bool>> DeleteFinalGroupIdOfStudent(string studentId);
        Task<ApiResult<StudentDto>> GetStudentByStudentId(string studentId);
        Task<ApiResult<List<StudentDto>>> GetStudentSearchList(int semester_Id, int profession_Id, int specialty_Id, int offsetNumber, int fetchNumber);
        Task<ApiResult<StudentDto>> GetStudentByFptEmail(string fptEmail, int semesterId);
        Task<ApiResult<StudentDto>> getLeaderByFinalGroupId(int finalGroupId);
        Task<ApiResult<List<StudentDto>>> getListMemberByFinalGroupId(int finalGroupId);
        Task<ApiResult<List<StudentDto>>> GetListStudentIdByFinalGroupId(int finalGroupId);
        Task<ApiResult<List<StudentDto>>> GetListUngroupedStudentsBySpecialityIdAndSemesterId(int semesterId, int[] specialityIds);
        Task<ApiResult<List<StudentDto>>> getListStudentNotHaveGroupBySpecialtyId(int semester_Id, int specialtyId);
        Task<ApiResult<StudentDto>> GetProfileOfStudentByUserId(string userId);
        Task<ApiResult<bool>> UpdateProfileOfStudent(Student student);
        Task<ApiResult<bool>> UpdateMajorOfStudentByUserId(string userId, int professionId, int specialtyId);
        Task<ApiResult<bool>> UpdateSemesterOfStudentByUserId(string userId);
        Task<ApiResult<bool>> UpdateGroupName(string studentId, string groupName);
        Task<ApiResult<int>> GetProfessionIdOfStudentByUserId(string userId);
        Task<ApiResult<int>> GetSpecialtyIdOfStudentByUserId(string userId);
        Task<ApiResult<StudentDto>> GetProfessionAndSpecialtyByStudentId(string studentId);
        Task<ApiResult<StudentDto>> GetStudentNotHaveGroupFinalByFptEmail(string fptEmail, int semesterId);
        Task<ApiResult<bool>> ChangeMemberForStudent(string[] listStudentIdOfOldMember, string[] listStudentIdOfNewMember, int finalGroupId, int changeMemberRequestId);
        Task<ApiResult<bool>> SetFinalGroupForStudent(int finalGroupId, int isLeader, string studentId, string groupName);
        Task<ApiResult<int>> GetFinalGroupIdOfStudentIsLeader(int groupIdeaId);
        Task<ApiResult<string>> GetStudentIDByFptEmailAndGroupName(string fptEmail, string groupName, int semesterId);
        Task<ApiResult<StudentDto>> GetInforStudentHaveFinalGroup(string studentId, int semesterId);
        Task<ApiResult<StudentDto>> GetInforStudentHaveRegisteredGroup(string fptEmail, int groupIdeaId);
        Task<ApiResult<StudentDto>> GetStudentById(string id);
        Task<ApiResult<StudentDto>> GetStudentById2(string StudentId);
        Task<ApiResult<List<StudentDto>>> GetUnGroupedStudentsBySemesterId(int semesterId);
        ApiResult<XLWorkbook> CreateWorkBookBasedOnStudentList(List<StudentDto> students, string firstSheetName, int currentRow);
        Task<ApiResult<bool>> StudentIsEligible(string studentID);
        Task<ApiResult<bool>> ChangeNotEligibleLeaderToOtherMember(string studentID);
        Task<ApiResult<bool>> DeleteAllRelatetoNotEligibleStudent(string studentId, bool isDeleteIdea);
        Task<ApiResult<bool>> UpdateStudentEligible(string studentId, bool eligible);
        Task<ApiResult<bool>> IsLeaderOfGroupIdea(string studentId);
        Task<ApiResult<bool>> IsFirstSemesterDoCapstoneProject(string fptEduEmail, int semesterId);
        Task<ApiResult<List<StudentDto>>> getStudentsBySpecialityId(int specialityId);
        Task<ApiResult<StudentDto>> GetFullnameAndGenderOfPreviousSemester(string fptEduEmail, int currentSemesterId);
        Task<ApiResult<List<StudentDto>>> getAllStudentOfSemester(int semesterId);
    }
}