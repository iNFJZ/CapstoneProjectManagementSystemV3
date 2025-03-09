using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentRepository
{
    public interface IStudentRepository : IRepositoryBase<Student>
    {
        Task<StudentDto> GetStudentByFptEmail(string fptEmail, int semesterId);
        Task<bool> UpdateFinalGroupIdForStudent(List<string> listStudentIdOfOldMember, List<string> listStudentIdOfNewMember, int finalGroupId, int changeMemberRequestId);
        Task<Student> GetFullnameAndGenderOfPreviousSemester(string fptEduEmail, int currentSemesterId);
        Task<List<Student>> GetAllUngroupedStudentsBySemesterIdAndSpecialityId(int[] specialityIds, int semesterId);
        Task<Student> GetProfileOfStudentByUserId(string userId);
        Task<List<Student>> GetStudentSearchList(int semester_Id, int profession_Id, int specialty_Id, int offsetNumber, int fetchNumber);
        Task<List<Student>> getAllUngroupedStudentsBySemesterIdAndProfessionId(int[] professionIds, int semesterId, bool isDeleted);
        Task<bool> IsLeaderOfGroupIdea(string studentId);
    }
}
