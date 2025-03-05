using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Infrastructure.Entities.Dto.StudentDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentRepository
{
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        private readonly DBContext _dbContext;
        public StudentRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<StudentDto> GetStudentByFptEmail(string fptEmail, int semesterId)
        {
            return await (from s in _dbContext.Students
                          join u in _dbContext.Users on s.StudentId equals u.UserId
                          where u.FptEmail == fptEmail
                                && s.SemesterId == semesterId
                                && u.DeletedAt == null
                                && s.DeletedAt == null
                          select new StudentDto
                          {
                              StudentId = s.StudentId,
                              RollNumber = s.RollNumber,
                              EmailAddress = u.FptEmail,
                              Avatar = u.Avatar,
                              FullName = u.FullName,
                              FinalGroupId = s.FinalGroupId,
                              IsEligible = s.IsEligible
                          }).FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateFinalGroupIdForStudent( List<string> listStudentIdOfOldMember, List<string> listStudentIdOfNewMember, int finalGroupId, int changeMemberRequestId)
        {
            // Cập nhật FinalGroup_ID thành NULL cho các thành viên cũ
            var oldMembers = await _dbContext.Students
                .Where(s => listStudentIdOfOldMember.Contains(s.StudentId) && s.FinalGroupId == finalGroupId)
                .ToListAsync();

            foreach (var student in oldMembers)
            {
                student.FinalGroupId = null;
            }

            // Cập nhật FinalGroup_ID cho các thành viên mới nếu FinalGroup_ID đang NULL
            var newMembers = await _dbContext.Students
                .Where(s => listStudentIdOfNewMember.Contains(s.StudentId) && s.FinalGroupId == null)
                .ToListAsync();

            foreach (var student in newMembers)
            {
                student.FinalGroupId = finalGroupId;
            }

            // Cập nhật trạng thái của ChangeMemberRequests
            //var changeRequest = await _dbContext.ChangeMemberRequests
            //    .FirstOrDefaultAsync(c => c.ChangeMemberRequestID == changeMemberRequestId);

            //if (changeRequest != null)
            //{
            //    changeRequest.Status = true;
            //}

            // Lưu thay đổi vào database
            var rowsAffected = await _dbContext.SaveChangesAsync();

            return rowsAffected > 0;
        }
        public async Task<Student> GetFullnameAndGenderOfPreviousSemester(string fptEduEmail, int currentSemesterId)
        {
            return await (from s in _dbContext.Students
                          join u in _dbContext.Users on s.StudentId equals u.UserId
                          where s.StudentId.Contains(fptEduEmail)
                                && (s.SemesterId == null || s.SemesterId != currentSemesterId)
                          orderby s.SemesterId descending
                          select new Student
                          {
                              StudentNavigation = new User
                              {
                                  FullName = u.FullName,
                                  Gender = u.Gender
                              }
                          }).FirstOrDefaultAsync();
        }
        public async Task<List<Student>> GetAllUngroupedStudentsBySemesterIdAndSpecialityId(int[] specialityIds, int semesterId)
        {
            return await _dbContext.Students
                .Where(s => s.SemesterId == semesterId &&
                            specialityIds.Contains((int)s.SpecialtyId) &&
                            s.IsEligible == true &&
                            s.FinalGroupId == null &&
                            !_dbContext.StudentGroupIdeas
                                .Where(sg => sg.DeletedAt == null && (sg.Status == 1 || sg.Status == 2))
                                .Join(_dbContext.RegisteredGroups,
                                      sg => sg.GroupIdeaId,
                                      rg => rg.GroupIdeaId,
                                      (sg, rg) => sg.StudentId)
                                .Contains(s.StudentId))
                .Include(s => s.Profession)
                .Include(s => s.Specialty)
                .Include(s => s.StudentNavigation)
                .ToListAsync();
        }
        public async Task<Student> GetProfileOfStudentByUserId(string userId)
        {
            return await _dbContext.Students
                .Where(s => s.StudentId == userId && s.DeletedAt == null)
                .Include(s => s.StudentNavigation)
                .Include(s => s.Semester)
                .Include(s => s.Profession)
                .Include(s => s.Specialty)
                .Select(s => new Student
                {
                    StudentId = s.StudentId,
                    StudentNavigation = new User
                    {
                        UserId = s.StudentNavigation.UserId,
                        Avatar = s.StudentNavigation.Avatar,
                        FullName = s.StudentNavigation.FullName,
                        Gender = s.StudentNavigation.Gender,
                        FptEmail = s.StudentNavigation.FptEmail
                    },
                    RollNumber = s.RollNumber,
                    Curriculum = s.Curriculum,
                    ExpectedRoleInGroup = s.ExpectedRoleInGroup,
                    SelfDiscription = s.SelfDiscription,
                    PhoneNumber = s.PhoneNumber,
                    EmailAddress = s.EmailAddress,
                    LinkFacebook = s.LinkFacebook,
                    Semester = new Semester
                    {
                        SemesterId = s.Semester.SemesterId,
                        SemesterName = s.Semester.SemesterName
                    },
                    ProfessionId = s.ProfessionId,
                    SpecialtyId = s.SpecialtyId,
                    WantToBeGrouped = s.WantToBeGrouped,
                    Profession = new Profession
                    {
                        ProfessionId = s.Profession.ProfessionId,
                        ProfessionFullName = s.Profession.ProfessionFullName
                    },
                    Specialty = new Specialty
                    {
                        SpecialtyId = s.Specialty.SpecialtyId,
                        SpecialtyFullName = s.Specialty.SpecialtyFullName
                    }
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<Student>> GetStudentSearchList(int semester_Id,int profession_Id,int specialty_Id,int offsetNumber,int fetchNumber)
        {
            var query = _dbContext.Students
                .Include(s => s.StudentNavigation) // Join với bảng Users
                .Where(s => s.SemesterId == semester_Id&& s.FinalGroupId == null && s.DeletedAt == null
                    && s.ProfessionId != null && s.SpecialtyId != null);

            // Lọc theo Profession_ID nếu có giá trị hợp lệ
            if (profession_Id > 0)
            {
                query = query.Where(s => s.ProfessionId == profession_Id);
            }

            // Lọc theo Specialty_ID nếu có giá trị hợp lệ
            if (specialty_Id > 0)
            {
                query = query.Where(s => s.SpecialtyId == specialty_Id);
            }

            // Sắp xếp và phân trang
            var students = await query
                .OrderBy(s => s.StudentId)
                .Skip(offsetNumber)
                .Take(fetchNumber)
                .ToListAsync();

            return students;
        }
        public async Task<List<Student>> getAllUngroupedStudentsBySemesterIdAndProfessionId(int[] professionIds, int semesterId, bool isDeleted)
        {
            var query = _dbContext.Students
                .Include(s => s.Profession)
                .Include(s => s.Specialty)
                .Include(s => s.StudentNavigation)
                .Where(s => s.SemesterId == semesterId
                    && s.FinalGroupId == null
                    && professionIds.Contains(s.ProfessionId.Value));

            // Lọc theo trạng thái Deleted
            if (!isDeleted)
            {
                query = query.Where(s => s.DeletedAt == null);
            }

            // Lọc sinh viên không thuộc Student_GroupIdea có trạng thái (1,2)
            var studentsInGroup = _dbContext.StudentGroupIdeas
                .Where(sg => sg.Status == 1 || sg.Status == 2)
                .Select(sg => sg.StudentId);

            query = query.Where(s => !studentsInGroup.Contains(s.StudentId));

            // Trả về danh sách sinh viên không có nhóm
            return await query.ToListAsync();
        }
        public async Task<bool> IsLeaderOfGroupIdea(string studentId)
        {
            return await _dbContext.StudentGroupIdeas
                .Where(sgi => sgi.StudentId == studentId &&
                              sgi.DeletedAt == null &&
                              sgi.GroupIdea.DeletedAt == null)
                .Select(sgi => sgi.Status)
                .FirstOrDefaultAsync() == 1; // Giả sử Status = 1 nghĩa là Leader
        }




    }
}
