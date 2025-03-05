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
        Task<ApiResult<Student>> GetStudentByStudentId(string studentId);
        Task<ApiResult<List<Student>>> GetStudentSearchList(int semester_Id, int profession_Id, int specialty_Id, int offsetNumber, int fetchNumber);
        Task<ApiResult<Student>> GetStudentByFptEmail(string fptEmail, int semesterId);
        Task<ApiResult<Student>> getLeaderByFinalGroupId(int finalGroupId);
        Task<ApiResult<List<Student>>> getListMemberByFinalGroupId(int finalGroupId);
        Task<ApiResult<List<Student>>> GetListStudentIdByFinalGroupId(int finalGroupId);
        Task<ApiResult<List<Student>>> GetListUngroupedStudentsBySpecialityIdAndSemesterId(int semesterId, int[] specialityIds);
        Task<ApiResult<List<Student>>> getListStudentNotHaveGroupBySpecialtyId(int semester_Id, int specialtyId);
        Task<ApiResult<Student>> GetProfileOfStudentByUserId(string userId);
        Task<ApiResult<bool>> UpdateProfileOfStudent(Student student);
        Task<ApiResult<bool>> UpdateMajorOfStudentByUserId(string userId, int professionId, int specialtyId);
        Task<ApiResult<bool>> UpdateSemesterOfStudentByUserId(string userId);
        Task<ApiResult<bool>> UpdateGroupName(string studentId, string groupName);
        Task<ApiResult<int>> GetProfessionIdOfStudentByUserId(string userId);
        Task<ApiResult<int>> GetSpecialtyIdOfStudentByUserId(string userId);
        Task<ApiResult<Student>> GetProfessionAndSpecialtyByStudentId(string studentId);
        Task<ApiResult<Student>> GetStudentNotHaveGroupFinalByFptEmail(string fptEmail, int semesterId);
        Task<ApiResult<bool>> ChangeMemberForStudent(string[] listStudentIdOfOldMember, string[] listStudentIdOfNewMember, int finalGroupId, int changeMemberRequestId);
        Task<ApiResult<bool>> SetFinalGroupForStudent(int finalGroupId, int isLeader, string studentId, string groupName);
        Task<ApiResult<int>> GetFinalGroupIdOfStudentIsLeader(int groupIdeaId);
        Task<ApiResult<string>> GetStudentIDByFptEmailAndGroupName(string fptEmail, string groupName, int semesterId);
        Task<ApiResult<Student>> GetInforStudentHaveFinalGroup(string studentId, int semesterId);
        Task<ApiResult<Student>> GetInforStudentHaveRegisteredGroup(string fptEmail, int groupIdeaId);
        Task<ApiResult<Student>> GetStudentById(string id);
        Task<ApiResult<Student>> GetStudentById2(string StudentId);
        Task<ApiResult<List<Student>>> GetUnGroupedStudentsBySemesterId(int semesterId);
        ApiResult<XLWorkbook> CreateWorkBookBasedOnStudentList(List<Student> students, string firstSheetName, int currentRow);
        Task<ApiResult<bool>> StudentIsEligible(string studentID);
        Task<ApiResult<bool>> ChangeNotEligibleLeaderToOtherMember(string studentID);
        Task<ApiResult<bool>> DeleteAllRelatetoNotEligibleStudent(string studentId, bool isDeleteIdea);
        Task<ApiResult<bool>> UpdateStudentEligible(string studentId, bool eligible);
        Task<ApiResult<bool>> IsLeaderOfGroupIdea(string studentId);
        Task<ApiResult<bool>> IsFirstSemesterDoCapstoneProject(string fptEduEmail, int semesterId);
        Task<ApiResult<List<Student>>> getStudentsBySpecialityId(int specialityId);
        Task<ApiResult<Student>> GetFullnameAndGenderOfPreviousSemester(string fptEduEmail, int currentSemesterId);
        Task<ApiResult<List<Student>>> getAllStudentOfSemester(int semesterId);
    }
}
