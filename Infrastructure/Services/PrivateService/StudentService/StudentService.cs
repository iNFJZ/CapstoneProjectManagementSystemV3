using ClosedXML.Excel;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.SemesterDto;
using Infrastructure.Entities.Dto.StudentDto;
using Infrastructure.Entities.Dto.UserDto;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.StudentGroupIdeaRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Services.CommonServices.ExcelService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.StudentService
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IStudentGroupIdeaRepository _groupIdeaRepository;
        private readonly IExcelService _excelService;
        private readonly IUserRepository _userRepository;
        private readonly IProfessionRepository _professionRepository;
        public StudentService(IStudentRepository studentRepository,
            ISemesterRepository semesterRepository,
            IStudentGroupIdeaRepository studentGroupIdeaRepository,
            IExcelService excelService,
            IUserRepository userRepository,
            IProfessionRepository professionRepository)
        {
            _studentRepository = studentRepository;
            _semesterRepository = semesterRepository;
            _groupIdeaRepository = studentGroupIdeaRepository;
            _excelService = excelService;
            _userRepository = userRepository;
            _professionRepository = professionRepository;
        }
        public async Task<ApiResult<bool>> ChangeMemberForStudent(string[] listStudentIdOfOldMember, string[] listStudentIdOfNewMember, int finalGroupId, int changeMemberRequestId) //thừa changeMemberRequestId tại db không tìm thấy bảng
        {
            List<string> listStudentIdOld = new List<string>();
            List<string> listStudentIdNew = new List<string>();
            var currentSemester = await _semesterRepository.GetCurrentSemester();
            foreach (var item in listStudentIdOfOldMember)
            {
                var oldStudentDto = await _studentRepository.GetStudentByFptEmail(item, currentSemester.SemesterID);
                listStudentIdOld.Add(oldStudentDto.StudentId);
            }
            foreach (var item in listStudentIdOfNewMember)
            {
                var newStudentDto = await _studentRepository.GetStudentByFptEmail(item, currentSemester.SemesterID);
                listStudentIdNew.Add(newStudentDto.StudentId);
            }
            await _studentRepository.UpdateFinalGroupIdForStudent(listStudentIdOld, listStudentIdNew, finalGroupId, changeMemberRequestId);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> ChangeNotEligibleLeaderToOtherMember(string studentID) //bảng StudentGroups
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(s => s.StudentId == studentID);
            expressions.Add(s => s.DeletedAt == null);
            var findStudent = await _groupIdeaRepository.GetByConditionId(expressions);
            if (findStudent == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findStudent.Status = 1;
            await _groupIdeaRepository.UpdateAsync(findStudent);
            return new ApiSuccessResult<bool>(true);
        }

        public ApiResult<XLWorkbook> CreateWorkBookBasedOnStudentList(List<Student> students, string firstSheetName, int currentRow)
        {
            XLWorkbook workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(firstSheetName);
            _excelService.setWorkSheetIntroduction(worksheet, "");
            if (students == null || students.Count == 0)
            {
                worksheet.Cell(currentRow, 1).Value = "There's No Student";
                return new ApiSuccessResult<XLWorkbook>(workbook);
            }
            worksheet.Cell(currentRow, 1).Value = "Fpt Email";
            worksheet.Cell(currentRow, 9).Value = "Full Name";
            worksheet.Cell(currentRow, 3).Value = "Roll Number";
            worksheet.Cell(currentRow, 4).Value = "Curriculum";
            worksheet.Cell(currentRow, 5).Value = "Self Description";
            worksheet.Cell(currentRow, 6).Value = "Expected Role In Group";
            worksheet.Cell(currentRow, 7).Value = "Phone Number";
            worksheet.Cell(currentRow, 8).Value = "Link Facebook";
            worksheet.Cell(currentRow, 2).Value = "Personal Email";
            worksheet.Cell(currentRow, 10).Value = "Profession";
            worksheet.Cell(currentRow, 11).Value = "Speciality";
            worksheet.Cell(currentRow, 12).Value = "The Wish To Be Grouped For";
            // set column width
            worksheet.Column(1).Width = 23;
            worksheet.Column(9).Width = 19;
            worksheet.Column(3).Width = 13;
            worksheet.Column(7).Width = 12;
            worksheet.Column(10).Width = 19;
            worksheet.Column(11).Width = 20;
            worksheet.Column(12).Width = 23;
            // Style for title row
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 12)).Style.Fill.BackgroundColor = XLColor.FromHtml("#f7caac");
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 12)).Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 12)).Style.Border.OutsideBorderColor = XLColor.Black;
            worksheet.Range(worksheet.Cell(currentRow, 1), worksheet.Cell(currentRow, 12)).Style.Font.Bold = true;
            foreach (Student st in students)
            {
                currentRow++;
                worksheet.Cell(currentRow, 1).Value = st.StudentNavigation.FptEmail;
                worksheet.Cell(currentRow, 9).Value = st.StudentNavigation != null && st.StudentNavigation.FullName != null && st.StudentNavigation.FullName.Length != 0 ? st.StudentNavigation.FullName : "None";
                worksheet.Cell(currentRow, 3).Value = st.RollNumber;
                worksheet.Cell(currentRow, 4).Value = st.Curriculum == null || st.Curriculum.Length == 0 ? "None" : st.Curriculum;
                worksheet.Cell(currentRow, 5).Value = st.SelfDiscription == null || st.SelfDiscription.Length == 0 ? "None" : st.SelfDiscription;
                worksheet.Cell(currentRow, 6).Value = st.ExpectedRoleInGroup == null || st.ExpectedRoleInGroup.Length == 0 ? "None" : st.ExpectedRoleInGroup;
                worksheet.Cell(currentRow, 7).Value = st.PhoneNumber == null || st.PhoneNumber.Length == 0 ? "None" : st.PhoneNumber;
                worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0000000000";
                worksheet.Cell(currentRow, 8).Value = st.LinkFacebook == null || st.LinkFacebook.Length == 0 ? "None" : st.LinkFacebook;
                worksheet.Cell(currentRow, 2).Value = st.StudentNavigation != null && st.StudentNavigation.AffiliateAccount != null ? st.StudentNavigation.AffiliateAccount.PersonalEmail : "None";
                worksheet.Cell(currentRow, 10).Value = st.Profession == null || st.Profession.ProfessionFullName == null || st.Profession.ProfessionFullName.Length == 0 ? "None" : st.Profession.ProfessionFullName;
                worksheet.Cell(currentRow, 11).Value = st.Specialty == null || st.Specialty.SpecialtyFullName == null || st.Specialty.SpecialtyFullName.Length == 0 ? "None" : st.Specialty.SpecialtyFullName;
                worksheet.Cell(currentRow, 12).Value = st.WantToBeGrouped.Value;
            }
            return new ApiSuccessResult<XLWorkbook>(workbook);
        }

        public async Task<ApiResult<bool>> DeleteAllRelatetoNotEligibleStudent(string studentId, bool isDeleteIdea)
        {
            Expression<Func<Student, bool>> expression = x => x.StudentId == studentId;
            var findStudent = await _studentRepository.GetById(expression);
            if (findStudent.IsEligible == false)
            {
                return new ApiSuccessResult<bool>(false);
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteFinalGroupIdOfStudent(string studentId)
        {
            Expression<Func<Student, bool>> expression = x => x.StudentId == studentId;
            var findStudent = await _studentRepository.GetById(expression);
            if (findStudent == null)
            {
                return new ApiErrorResult<bool>("không tìm thấy dữ liệu");
            }
            findStudent.FinalGroupId = null;
            findStudent.GroupName = null;
            findStudent.IsLeader = false;
            await _studentRepository.UpdateAsync(findStudent);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<Student>>> getAllStudentOfSemester(int semesterId)
        {

            List<Expression<Func<Student, bool>>> expressions = new List<Expression<Func<Student, bool>>>();
            expressions.Add(s => s.SemesterId == semesterId);
            expressions.Add(s => s.DeletedAt == null);
            var studentList = await _studentRepository.GetByConditions(expressions);
            return new ApiSuccessResult<List<Student>>(studentList);
        }

        public async Task<ApiResult<int>> GetFinalGroupIdOfStudentIsLeader(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(s => s.GroupIdeaId == groupIdeaId);
            expressions.Add(s => s.DeletedAt == null);
            expressions.Add(s => s.Status == 1);
            var studentLeader = await _groupIdeaRepository.GetByConditionId(expressions);
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentLeader.StudentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            List<Expression<Func<Student, bool>>> studentExpression2 = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == student.StudentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var studentLead = await _studentRepository.GetByConditionId(studentExpression2);
            return new ApiSuccessResult<int>((int)studentLead.FinalGroupId);
        }

        public async Task<ApiResult<Student>> GetFullnameAndGenderOfPreviousSemester(string fptEduEmail, int currentSemesterId)
        {
            var result = await _studentRepository.GetFullnameAndGenderOfPreviousSemester(fptEduEmail, currentSemesterId);
            return new ApiSuccessResult<Student>(result);
        }

        public async Task<ApiResult<Student>> GetInforStudentHaveFinalGroup(string studentId, int semesterId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.SemesterId == semesterId);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            if (objFind == null)
            {
                return new ApiErrorResult<Student>("không tìm thấy dữ liệu");
            }
            var student = new Student()
            {
                StudentId = objFind.StudentId,
                EmailAddress = objFind.StudentNavigation.FptEmail,
                GroupName = objFind.FinalGroup.GroupName,
            };
            return new ApiSuccessResult<Student>(student);
        }

        public async Task<ApiResult<Student>> GetInforStudentHaveRegisteredGroup(string fptEmail, int groupIdeaId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == fptEmail || s.StudentNavigation.FptEmail == fptEmail);
            studentExpression.Add(s => s.FinalGroup.FinalGroupId == groupIdeaId);
            studentExpression.Add(s => s.DeletedAt == null);
            studentExpression.Add(s => s.StudentNavigation.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            return new ApiSuccessResult<Student>(student);
        }

        public async Task<ApiResult<Student>> getLeaderByFinalGroupId(int finalGroupId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.FinalGroupId == finalGroupId);
            studentExpression.Add(s => s.IsLeader == true);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            var studentLeader = new Student()
            {
                StudentId = objFind.StudentId,
                Curriculum = objFind.Curriculum,
                EmailAddress = objFind.EmailAddress,
                ExpectedRoleInGroup = objFind.ExpectedRoleInGroup,
                SelfDiscription = objFind.SelfDiscription,
                PhoneNumber = objFind.PhoneNumber,
                LinkFacebook = objFind.LinkFacebook,
                FinalGroup = objFind.FinalGroup,
                GroupName = objFind.FinalGroup.GroupName,
                RollNumber = objFind.RollNumber,
                IsLeader = objFind.IsLeader,
                CreatedAt = objFind.CreatedAt,
                StudentNavigation = objFind.StudentNavigation,
                Profession = objFind.Profession,
                Specialty = objFind.Specialty,
            };
            return new ApiSuccessResult<Student>(studentLeader);
        }

        public async Task<ApiResult<List<Student>>> getListMemberByFinalGroupId(int finalGroupId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.FinalGroupId == finalGroupId);
            studentExpression.Add(s => s.IsLeader == false);
            var objFind = await _studentRepository.GetByConditions(studentExpression);
            var listStudent = new List<Student>();
            foreach (var item in objFind)
            {
                var student = new Student()
                {
                    StudentId = item.StudentId,
                    Curriculum = item.Curriculum,
                    EmailAddress = item.EmailAddress,
                    ExpectedRoleInGroup = item.ExpectedRoleInGroup,
                    SelfDiscription = item.SelfDiscription,
                    PhoneNumber = item.PhoneNumber,
                    LinkFacebook = item.LinkFacebook,
                    FinalGroup = item.FinalGroup,
                    GroupName = item.FinalGroup.GroupName,
                    RollNumber = item.RollNumber,
                    IsLeader = item.IsLeader,
                    CreatedAt = item.CreatedAt,
                    StudentNavigation = item.StudentNavigation,
                    Profession = item.Profession,
                    Specialty = item.Specialty,
                };
                listStudent.Add(student);
            }
            return new ApiSuccessResult<List<Student>>(listStudent);
        }

        public async Task<ApiResult<List<Student>>> GetListStudentIdByFinalGroupId(int finalGroupId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.FinalGroupId == finalGroupId);
            var objFind = await _studentRepository.GetByConditions(studentExpression);
            var listStudent = new List<Student>();
            foreach (var item in objFind)
            {
                var student = new Student()
                {
                    StudentId = item.StudentId,
                    Curriculum = item.Curriculum,
                    EmailAddress = item.EmailAddress,
                    ExpectedRoleInGroup = item.ExpectedRoleInGroup,
                    SelfDiscription = item.SelfDiscription,
                    PhoneNumber = item.PhoneNumber,
                    LinkFacebook = item.LinkFacebook,
                    FinalGroup = item.FinalGroup,
                    GroupName = item.FinalGroup.GroupName,
                    RollNumber = item.RollNumber,
                    IsLeader = item.IsLeader,
                    CreatedAt = item.CreatedAt,
                    StudentNavigation = item.StudentNavigation,
                    Profession = item.Profession,
                    Specialty = item.Specialty,
                };
                listStudent.Add(student);
            }
            return new ApiSuccessResult<List<Student>>(listStudent);
        }

        public async Task<ApiResult<List<Student>>> getListStudentNotHaveGroupBySpecialtyId(int semester_Id, int specialtyId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.SemesterId == semester_Id);
            studentExpression.Add(s => s.FinalGroupId == null);
            studentExpression.Add(s => s.SpecialtyId == specialtyId);
            studentExpression.Add(s => s.DeletedAt == null);
            studentExpression.Add(s => s.StudentNavigation.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditions(studentExpression);
            var listStudent = new List<Student>();
            foreach (var item in objFind)
            {
                var student = new Student()
                {
                    StudentId = item.StudentId,
                    Curriculum = item.Curriculum,
                    EmailAddress = item.EmailAddress,
                    ExpectedRoleInGroup = item.ExpectedRoleInGroup,
                    SelfDiscription = item.SelfDiscription,
                    PhoneNumber = item.PhoneNumber,
                    LinkFacebook = item.LinkFacebook,
                    FinalGroup = item.FinalGroup,
                    GroupName = item.FinalGroup.GroupName,
                    RollNumber = item.RollNumber,
                    IsLeader = item.IsLeader,
                    CreatedAt = item.CreatedAt,
                    StudentNavigation = item.StudentNavigation,
                    Profession = item.Profession,
                    Specialty = item.Specialty,
                };
                listStudent.Add(student);
            }
            return new ApiSuccessResult<List<Student>>(listStudent);
        }

        public async Task<ApiResult<List<Student>>> GetListUngroupedStudentsBySpecialityIdAndSemesterId(int semesterId, int[] specialityIds)
        {
            var result = await _studentRepository.GetAllUngroupedStudentsBySemesterIdAndSpecialityId(specialityIds, semesterId);
            var listStudent = new List<Student>();
            foreach (var item in result)
            {
                var student = new Student()
                {
                    StudentId = item.StudentId,
                    Curriculum = item.Curriculum,
                    EmailAddress = item.EmailAddress,
                    ExpectedRoleInGroup = item.ExpectedRoleInGroup,
                    SelfDiscription = item.SelfDiscription,
                    PhoneNumber = item.PhoneNumber,
                    LinkFacebook = item.LinkFacebook,
                    FinalGroup = item.FinalGroup,
                    GroupName = item.FinalGroup.GroupName,
                    RollNumber = item.RollNumber,
                    IsLeader = item.IsLeader,
                    CreatedAt = item.CreatedAt,
                    StudentNavigation = item.StudentNavigation,
                    Profession = item.Profession,
                    Specialty = item.Specialty,
                };
                listStudent.Add(student);
            }
            return new ApiSuccessResult<List<Student>>(listStudent);
        }

        public async Task<ApiResult<Student>> GetProfessionAndSpecialtyByStudentId(string studentId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            var student = new Student()
            {
                Profession = objFind.Profession,
                Specialty = objFind.Specialty,
            };
            return new ApiSuccessResult<Student>(student);
        }

        public async Task<ApiResult<int>> GetProfessionIdOfStudentByUserId(string userId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == userId);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            return new ApiSuccessResult<int>(objFind.Profession.ProfessionId);
        }

        public async Task<ApiResult<Student>> GetProfileOfStudentByUserId(string userId)
        {
            var result = await _studentRepository.GetProfileOfStudentByUserId(userId);
            return new ApiSuccessResult<Student>(result);
        }

        public async Task<ApiResult<int>> GetSpecialtyIdOfStudentByUserId(string userId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == userId);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            return new ApiSuccessResult<int>(objFind.Specialty.SpecialtyId);
        }

        public async Task<ApiResult<Student>> GetStudentByFptEmail(string fptEmail, int semesterId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentNavigation.FptEmail == fptEmail);
            studentExpression.Add(s => s.SemesterId == semesterId);
            studentExpression.Add(s => s.StudentNavigation.DeletedAt == null);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            return new ApiSuccessResult<Student>(objFind);
        }

        public async Task<ApiResult<Student>> GetStudentById(string id)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == id);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            var student = new Student()
            {
                StudentId = objFind.StudentId,
                ProfessionId = objFind.ProfessionId,
            };
            return new ApiSuccessResult<Student>(student);
        }

        public async Task<ApiResult<Student>> GetStudentById2(string StudentId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == StudentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            var student = new Student()
            {
                StudentId = objFind.StudentId,
                ProfessionId = objFind.ProfessionId,
            };
            return new ApiSuccessResult<Student>(student);
        }

        public async Task<ApiResult<Student>> GetStudentByStudentId(string studentId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.StudentNavigation.DeletedAt == null);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            var student = new Student()
            {
                StudentId = objFind.StudentId,
                StudentNavigation = new User()
                {
                    FptEmail = objFind.StudentNavigation.FptEmail,
                    Avatar = objFind.StudentNavigation.Avatar,
                    FullName = objFind.StudentNavigation.FullName,
                },
                FinalGroup = new FinalGroup()
                {
                    FinalGroupId = objFind.FinalGroup.FinalGroupId,
                },
                GroupName = objFind.GroupName,
                IsLeader = objFind.IsLeader,
                Profession = new Profession()
                {
                    ProfessionId = objFind.Profession.ProfessionId,
                    ProfessionFullName = objFind.Profession.ProfessionFullName,
                },
                Specialty = new Specialty()
                {
                    SpecialtyId = objFind.Specialty.SpecialtyId,
                    SpecialtyFullName = objFind.Specialty.SpecialtyFullName,
                },
                PhoneNumber = objFind.PhoneNumber,
                IsEligible = objFind.IsEligible,
                Semester = new Semester()
                {
                    SemesterId = objFind.Semester.SemesterId,
                }
            };
            return new ApiSuccessResult<Student>(student);
        }

        public async Task<ApiResult<string>> GetStudentIDByFptEmailAndGroupName(string fptEmail, string groupName, int semesterId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentNavigation.FptEmail == fptEmail);
            studentExpression.Add(s => s.FinalGroup.GroupName == groupName);
            studentExpression.Add(s => s.Semester.SemesterId == semesterId);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            return new ApiSuccessResult<string>(objFind.StudentId);
        }

        public async Task<ApiResult<Student>> GetStudentNotHaveGroupFinalByFptEmail(string fptEmail, int semesterId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentNavigation.FptEmail == fptEmail);
            studentExpression.Add(s => s.Semester.SemesterId == semesterId);
            studentExpression.Add(s => s.FinalGroup.FinalGroupId == null);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditionId(studentExpression);
            return new ApiSuccessResult<Student>(objFind);
        }

        public async Task<ApiResult<List<Student>>> getStudentsBySpecialityId(int specialityId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.Specialty.SpecialtyId == specialityId);
            studentExpression.Add(s => s.StudentNavigation.DeletedAt == null);
            studentExpression.Add(s => s.DeletedAt == null);
            var objFind = await _studentRepository.GetByConditions(studentExpression);
            return new ApiSuccessResult<List<Student>>(objFind);
        }

        public async Task<ApiResult<List<Student>>> GetStudentSearchList(int semester_Id, int profession_Id, int specialty_Id, int offsetNumber, int fetchNumber)
        {
            var result = await _studentRepository.GetStudentSearchList(semester_Id, profession_Id, specialty_Id, offsetNumber, fetchNumber);
            return new ApiSuccessResult<List<Student>>(result);
        }

        public async Task<ApiResult<List<Student>>> GetUnGroupedStudentsBySemesterId(int semesterId)
        {
            var profession = await _professionRepository.GetAll(semesterId);
            int[] allProfessionIds = profession.Select(profession => profession.ProfessionId).ToArray();
            List<Student> students = await _studentRepository.getAllUngroupedStudentsBySemesterIdAndProfessionId(allProfessionIds, semesterId, true);
            return new ApiSuccessResult<List<Student>>(students);
        }

        public async Task<ApiResult<bool>> IsFirstSemesterDoCapstoneProject(string fptEduEmail, int semesterId)
        {
            Expression<Func<Student, bool>> expression = x => x.StudentId == fptEduEmail;
            var findStudent = await _studentRepository.GetByCondition(expression);
            var student = findStudent.OrderBy(s => s.SemesterId).FirstOrDefault();
            if (student == null)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(student.SemesterId == semesterId);
        }


        public async Task<ApiResult<bool>> IsLeaderOfGroupIdea(string studentId)
        {
            var result = await _studentRepository.IsLeaderOfGroupIdea(studentId);
            return new ApiSuccessResult<bool>(result);
        }

        public async Task<ApiResult<bool>> SetFinalGroupForStudent(int finalGroupId, int isLeader, string studentId, string groupName)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            if (student == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            student.FinalGroupId = finalGroupId;
            student.FinalGroup = new FinalGroup()
            {
                FinalGroupId = finalGroupId,
                GroupName = groupName,
            };
            student.IsLeader = isLeader == 1;
            student.GroupName = groupName;
            await _studentRepository.UpdateAsync(student);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> StudentIsEligible(string studentID)
        {
            Expression<Func<Student, bool>> expression = x => x.StudentId == studentID;
            var student = await _studentRepository.GetById(expression);
            return new ApiSuccessResult<bool>((bool)student.IsEligible);
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> UpdateGroupName(string studentId, string groupName)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            if (student == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            student.GroupName = groupName;
            await _studentRepository.UpdateAsync(student);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateMajorOfStudentByUserId(string userId, int professionId, int specialtyId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == userId);
            studentExpression.Add(s => s.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            if (student == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            student.ProfessionId = professionId;
            student.SpecialtyId = specialtyId;
            student.Profession = new Profession()
            {
                ProfessionId = professionId
            };
            student.Specialty = new Specialty()
            {
                SpecialtyId = specialtyId
            };
            await _studentRepository.UpdateAsync(student);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateProfileOfStudent(Student student)
        {
            Expression<Func<User, bool>> expression = x => x.UserId == student.StudentNavigation.UserId;
            var user = await _userRepository.GetById(expression);
            if (user == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy user tương ứng");
            }
            user.Gender = student.StudentNavigation.Gender;
            user.FullName = student.StudentNavigation.FullName;
            await _userRepository.UpdateAsync(user);
            Expression<Func<Student, bool>> studentExpression = x => x.StudentId == student.StudentId;
            var studentFind = await _studentRepository.GetById(studentExpression);
            if (studentFind == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy student");
            }
            studentFind.ExpectedRoleInGroup = student.ExpectedRoleInGroup;
            studentFind.SelfDiscription = student.SelfDiscription;
            studentFind.PhoneNumber = student.PhoneNumber;
            studentFind.LinkFacebook = student.LinkFacebook;
            studentFind.WantToBeGrouped = student.WantToBeGrouped;
            studentFind.SpecialtyId = student.SpecialtyId;
            studentFind.Specialty = student.Specialty;
            await _studentRepository.UpdateAsync(studentFind);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateSemesterOfStudentByUserId(string userId)
        {

            try
            {
                Expression<Func<Student, bool>> studentExpression = x => x.StudentId == userId;
                var student = await _studentRepository.GetById(studentExpression);
                Expression<Func<Semester, bool>> expression = a => a.StatusCloseBit == true;
                var semester = await _semesterRepository.GetById(student.SemesterId.Value);
                student.SemesterId = semester.SemesterId;
                student.Semester = semester;
                await _studentRepository.UpdateAsync(student);
                return new ApiSuccessResult<bool>(true);
            }
            catch(Exception ex)
            {
                return new ApiSuccessResult<bool>(true);
            }
        }

        public async Task<ApiResult<bool>> UpdateStudentByGroupId(int finalGroupId, string groupName, int status, string studentId)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            if (student == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            student.FinalGroupId = finalGroupId;
            student.GroupName = groupName;
            student.IsLeader = status == 1;
            await _studentRepository.UpdateAsync(student);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStudentEligible(string studentId, bool eligible)
        {
            List<Expression<Func<Student, bool>>> studentExpression = new List<Expression<Func<Student, bool>>>();
            studentExpression.Add(s => s.StudentId == studentId);
            studentExpression.Add(s => s.DeletedAt == null);
            var student = await _studentRepository.GetByConditionId(studentExpression);
            if (student == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            student.IsEligible = eligible;
            await _studentRepository.UpdateAsync(student);
            return new ApiSuccessResult<bool>(true);
        }
    }
}
