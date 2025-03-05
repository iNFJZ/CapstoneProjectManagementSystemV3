using Infrastructure.Entities;
using Infrastructure.Entities.Common;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.SemesterDto;
using Infrastructure.Entities.Dto.StudentDto;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.RegisteredRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.StudentGroupIdeaRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Services.CommonServices.NotificationService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.StudentGroupIdeaService
{
    public class StudentGroupIdeaService : IStudentGroupIdeaService
    {
        private readonly IStudentGroupIdeaRepository _studentGroupIdeaRepository;
        private readonly INotificationService _notificationService;
        private readonly IRegisteredRepository _registeredRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        public StudentGroupIdeaService(IStudentGroupIdeaRepository studentGroupIdeaRepository, 
            INotificationService notificationService, 
            IRegisteredRepository registeredRepository,
            ISemesterRepository semesterRepository,
            IStudentRepository studentRepository,
            IGroupIdeaRepository groupIdeaRepository)
        {
            _studentGroupIdeaRepository = studentGroupIdeaRepository;
            _notificationService = notificationService;
            _registeredRepository = registeredRepository;
            _semesterRepository = semesterRepository;
            _studentRepository = studentRepository;
            _groupIdeaRepository = groupIdeaRepository;
        }

        public async Task<ApiResult<bool>> AddNewMembersToGroup(int groupIdeaId, string studentIds)
        {
            try
            {
                SemesterDto currentSemester = await _semesterRepository.GetCurrentSemester();
                string[] studentIdArray = studentIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                RegisteredGroup registeredGroup = await _registeredRepository.GetRegisteredGroupByGroupIdeaId(groupIdeaId);
                foreach (string studentId in studentIdArray)
                {
                    List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
                    expressions.Add(x => x.StudentId == studentId);
                    expressions.Add(x => x.DeletedAt == null);
                    var studentGroupIdea = await _studentGroupIdeaRepository.GetByConditionId(expressions);
                    studentGroupIdea.DeletedAt = DateTime.Now;
                    await _studentGroupIdeaRepository.UpdateAsync(studentGroupIdea);
                    _notificationService.InsertDataNotification(studentId, "You have been added to group", "/MyGroup/Index");
                    List<Expression<Func<StudentGroupIdea, bool>>> studentExpressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
                    expressions.Add(x => x.GroupIdeaId == groupIdeaId);
                    expressions.Add(x => x.DeletedAt == null);
                    var leader = await _studentGroupIdeaRepository.GetByConditionId(studentExpressions);
                    string leaderId = leader.StudentId;
                    if(leaderId == null)
                    {
                        var newStudentGroup = new StudentGroupIdea()
                        {
                            StudentId = studentId,
                            GroupIdeaId = groupIdeaId,
                            Status = 1,
                            Message = ""
                        };
                        await _studentGroupIdeaRepository.CreateAsync(newStudentGroup);
                    }
                    else
                    {
                        var newStudentGroup = new StudentGroupIdea()
                        {
                            StudentId = studentId,
                            GroupIdeaId = groupIdeaId,
                            Status = 2,
                            Message = ""
                        };
                        await _studentGroupIdeaRepository.CreateAsync(newStudentGroup);
                    }

                }
                string[] fptEmails = registeredGroup.StudentsRegistraiton.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach(string fptEmail in fptEmails)
                {
                    var student = await _studentRepository.GetStudentByFptEmail(fptEmail,currentSemester.SemesterID);
                    string studentId = student.StudentId;
                    _notificationService.InsertDataNotification(studentId, "You group have a new member", "/MyGroup/Index");
                }
                registeredGroup.StudentsRegistraiton += string.Join(",", studentIdArray.Select(studentId => studentId.Split("-")[0]).ToArray()) + ",";
                Expression<Func<RegisteredGroup, bool>> expression = x => x.RegisteredGroupId == registeredGroup.RegisteredGroupId;
                var registerGroup = await _registeredRepository.GetById(expression);
                registeredGroup.StudentsRegistraiton = registeredGroup.StudentsRegistraiton;
                await _registeredRepository.UpdateAsync(registerGroup);
                int numberOfMember = registeredGroup.StudentsRegistraiton.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                Expression<Func<GroupIdea, bool>> groupExpression = x => x.GroupIdeaId == groupIdeaId;
                var groupIdea = await _groupIdeaRepository.GetById(groupExpression);
                groupIdea.NumberOfMember = numberOfMember;
                await _groupIdeaRepository.UpdateAsync(groupIdea);
                return new ApiSuccessResult<bool>(true);
            } catch (Exception ex)
            {
                return new ApiSuccessResult<bool>(false);
            }
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> AddRecord(string studentId, int groupId, int status, string message)
        {
            var newStudentGroup = new StudentGroupIdea()
            {
                StudentId = studentId,
                GroupIdeaId = groupId,
                Status = status,
                Message = message
            };
            await _studentGroupIdeaRepository.CreateAsync(newStudentGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CheckAddedStudentIsValid(string fptEmail)
        {
            var listStatusOfStudentInEachGroup = await _studentGroupIdeaRepository.GetListStatusOfStudentInEachGroupByFptEmail(fptEmail);
            if (listStatusOfStudentInEachGroup == null)
            {
                return new ApiSuccessResult<bool>(false);
            }
            else if (listStatusOfStudentInEachGroup.Any(status => status == 1 || status == 2))
                return new ApiSuccessResult<bool>(true);
            else
                return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<bool>> CreateGroupIdea(GroupIdea groupIdea, string studentId, int semesterId, int maxMember)
        {
            try
            {
                var newGroupIdea = new GroupIdea()
                {
                    ProfessionId = groupIdea.ProfessionId,
                    SpecialtyId = groupIdea.SpecialtyId,
                    ProjectEnglishName = groupIdea.ProjectEnglishName,
                    ProjectVietNameseName = groupIdea.ProjectVietNameseName,
                    Abbreviation = groupIdea.Abbreviation,
                    Description = groupIdea.Description,
                    ProjectTags = groupIdea.ProjectTags,
                    SemesterId = semesterId,
                    NumberOfMember = 1,
                    MaxMember = maxMember,
                    CreatedAt = DateTime.Now,
                };
                await _groupIdeaRepository.CreateAsync(newGroupIdea);
                var newStudentGroupIdea = new StudentGroupIdea()
                {
                    StudentId = studentId,
                    GroupIdeaId = groupIdea.GroupIdeaId,
                    Status = 1,
                    CreatedAt = DateTime.Now
                };
                await _studentGroupIdeaRepository.CreateAsync(newStudentGroupIdea);
                return new ApiSuccessResult<bool>(true);
            }catch(Exception ex)
            {
                return new ApiErrorResult<bool>(ex.ToString());
            }
        }

        public async Task<ApiResult<bool>> DeleteAllRecordOfGroupIdea(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(x => x.GroupIdeaId == groupIdeaId);
            expressions.Add(x => x.DeletedAt == null);
            var studentGroupIdea = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            studentGroupIdea.DeletedAt = DateTime.Now;
            await _studentGroupIdeaRepository.UpdateAsync(studentGroupIdea);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteAllRequest(string studentId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(x => x.StudentId == studentId);
            expressions.Add(x => x.DeletedAt == null);
            expressions.Add(x => x.Status == 3 || x.Status == 4 || x.Status == 5);
            var request = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            request.DeletedAt = DateTime.Now;
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteGroupIdeaAndStudentInGroupIdea(int groupIdeaId)
        {
            Expression<Func<GroupIdea, bool>> expression = x => x.GroupIdeaId == groupIdeaId;
            var groupIdea = await _groupIdeaRepository.GetById(expression);
            groupIdea.DeletedAt = DateTime.Now;
            await _groupIdeaRepository.UpdateAsync(groupIdea);
            Expression<Func<StudentGroupIdea, bool>> studentGroupExpression = s => s.GroupIdeaId == groupIdeaId;
            var studentGroup = await _studentGroupIdeaRepository.GetById(studentGroupExpression);
            studentGroup.DeletedAt = DateTime.Now;
            await _studentGroupIdeaRepository.UpdateAsync(studentGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteMembersFromGroup(int groupIdeaId, string studentIds)
        {
            try
            {
                SemesterDto currentSemester = await _semesterRepository.GetCurrentSemester();
                List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
                expressions.Add(x => x.GroupIdeaId == groupIdeaId);
                expressions.Add(x => x.Status == 1);
                expressions.Add(x => x.DeletedAt == null);
                var studentGroup = await _studentGroupIdeaRepository.GetByConditionId(expressions);
                string leaderId = studentGroup.StudentId;
                string[] studentIdArray = studentIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string studentId in studentIdArray)
                {
                    _notificationService.InsertDataNotification(studentId, "You have been deleted from your group", "/MyGroup/Index");
                    if (leaderId != null && studentId == leaderId)
                    {
                        List<string> memberId = new List<string>();
                        List<Expression<Func<StudentGroupIdea, bool>>> memberExpressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
                        expressions.Add(x => x.GroupIdeaId == groupIdeaId);
                        expressions.Add(x => x.Status == 2);
                        expressions.Add(x => x.DeletedAt == null);
                        var studentGroups = await _studentGroupIdeaRepository.GetByConditions(memberExpressions);
                        foreach (var member in studentGroups)
                        {
                            memberId.Add(member.ToString());
                            if (memberId != null && memberId.Count() > 0)
                            {
                                List<Expression<Func<StudentGroupIdea, bool>>> memberExpression = new List<Expression<Func<StudentGroupIdea, bool>>>();
                                expressions.Add(x => x.GroupIdeaId == groupIdeaId);
                                expressions.Add(x => x.StudentId == memberId[0]);
                                expressions.Add(x => x.DeletedAt == null);
                                var studentToleader = await _studentGroupIdeaRepository.GetByConditionId(memberExpression);
                                studentToleader.Status = 1;
                                await _studentGroupIdeaRepository.UpdateAsync(studentToleader);
                            }
                            await _studentGroupIdeaRepository.DeleteRecord(studentId, groupIdeaId);
                        }
                    }
                }
                RegisteredGroup registeredGroup = await _registeredRepository.GetRegisteredGroupByGroupIdeaId(groupIdeaId);
                string studentsRegistration = "";
                string message = studentIdArray.Length > 1 ? "Some members has been deleted from your group" : "A member has been deleted from your group";
                string[] fptEmailArray = studentIdArray.Select(studentId => studentId.Split("-")[0]).ToArray();
                foreach (string student in registeredGroup.StudentsRegistraiton.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    studentsRegistration += student + ",";
                    var studentByEmail = await _studentRepository.GetStudentByFptEmail(student, currentSemester.SemesterID);
                    _notificationService.InsertDataNotification(studentByEmail.StudentId, message, "/MyGroup/Index");
                }
                registeredGroup.StudentsRegistraiton = studentsRegistration;
                Expression<Func<RegisteredGroup, bool>> registerGroupExpression = x => x.RegisteredGroupId == registeredGroup.RegisteredGroupId;
                var registerGroup = await _registeredRepository.GetById(registerGroupExpression);
                registerGroup.StudentsRegistraiton = registerGroup.StudentsRegistraiton;
                await _registeredRepository.UpdateAsync(registerGroup);
                int numberOfMember = registeredGroup.StudentsRegistraiton.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Length;
                List<Expression<Func<GroupIdea, bool>>> groupExpression = new List<Expression<Func<GroupIdea, bool>>>();
                expressions.Add(x => x.GroupIdeaId == groupIdeaId);
                expressions.Add(x => x.DeletedAt == null);
                var groupIdeaResult = await _groupIdeaRepository.GetByConditionId(groupExpression);
                groupIdeaResult.NumberOfMember = numberOfMember;
                await _groupIdeaRepository.UpdateAsync(groupIdeaResult);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiSuccessResult<bool>(false);
            }
        }

        public async Task<ApiResult<bool>> DeleteRecord(string studentId, int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(x => x.StudentId == studentId);
            expressions.Add(x => x.DeletedAt == null);
            expressions.Add(x => x.GroupIdeaId == groupIdeaId);
            var studentGroup = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            studentGroup.DeletedAt = DateTime.Now;
            studentGroup.Status = 6;
            await _studentGroupIdeaRepository.UpdateAsync(studentGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteRecordHaveStatusEqual3or4or5OfGroupIdea(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => new[] { 3, 4, 5 }.Contains(sg.Status.Value));
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            result.DeletedAt = DateTime.Now;
            await _studentGroupIdeaRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<int>> FilterPermissionOfStudent(string studentId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.DeletedAt == null);
            expressions.Add(sg => sg.Student.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            return new ApiSuccessResult<int>(result.Status.Value);
        }

        public async Task<ApiResult<bool>> FilterStudentHaveIdea(string studentId, int semesterId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.Status == 1 || sg.Status == 2);
            expressions.Add(sg => sg.GroupIdea.SemesterId ==  semesterId);
            expressions.Add(sg => sg.DeletedAt == null);
            expressions.Add(sg => sg.Student.DeletedAt == null);
            expressions.Add(sg => sg.GroupIdea.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            if(result != null)
            {
                return new ApiSuccessResult<bool>(false);
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<JoinRequest>>> GetAllJoinRequestByGroupIdeaId(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.Status == 3 );
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Message != null);
            expressions.Add(sg => sg.Student.DeletedAt == null);
            expressions.Add(sg => sg.DeletedAt == null);
            var results = await _studentGroupIdeaRepository.GetByConditions(expressions);
            var listJoinRequest = new List<JoinRequest>();
            foreach(var result in results)
            {
                var joinRequest = new JoinRequest()
                {
                    UserId = result.Student.StudentNavigation.UserId,
                    FptEmail = result.Student.StudentNavigation.FptEmail,
                    Avatar = result.Student.StudentNavigation.Avatar,
                    Message = result.Message,
                    CreatedAt = result.CreatedAt
                };
                listJoinRequest.Add(joinRequest);
            }
            
            return new ApiSuccessResult<List<JoinRequest>>(listJoinRequest);
        }

        public async Task<ApiResult<string>> GetGroupIdByStudentId(string studentId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.Status == 1 || sg.Status == 2);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            if (result != null)
            {
                return new ApiSuccessResult<string>("");
            }
            return new ApiSuccessResult<string>(result.GroupIdeaId.ToString());
        }

        public async Task<ApiResult<GroupIdeaDto>> GetGroupIdeaOfStudent(string studentId, int status)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.Status == status);
            expressions.Add(sg => sg.DeletedAt == null);
            expressions.Add(sg => sg.GroupIdea.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            var groupIdea = new GroupIdeaDto()
            {
                GroupIdeaID = result.GroupIdea.GroupIdeaId,
                ProfessionId = result.GroupIdea.ProfessionId,
                SpecialtyId = result.GroupIdea.SpecialtyId,
                ProjectEnglishName = result.GroupIdea.ProjectEnglishName,
                ProjectVietNameseName = result.GroupIdea.ProjectVietNameseName,
                Abrrevation = result.GroupIdea.Abbreviation,
                Description = result.GroupIdea.Description,
                ProjectTags = result.GroupIdea.ProjectTags
            };
            return new ApiSuccessResult<GroupIdeaDto>(groupIdea);
           
        }

        public async Task<ApiResult<List<StudentGroupIdeaDto>>> GetInforStudentInGroupIdea(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Status == 1 || sg.Status == 2);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditions(expressions);
            result.OrderBy(sg => sg.Status);
            var listStudentGroup = new List<StudentGroupIdeaDto>();
            foreach( var item in result)
            {
                var studentGroup = new StudentGroupIdeaDto()
                {
                    GroupIdeaId = item.GroupIdea.GroupIdeaId,
                    StudentId = item.Student.StudentId,
                    Avatar = item.Student.StudentNavigation.Avatar,
                    FptEmail = item.Student.StudentNavigation.FptEmail,
                    RollNumber = item.Student.RollNumber,
                    FullName = item.Student.StudentNavigation.FullName,
                    ProfessionFullName = item.Student.Profession.ProfessionFullName,
                    SpecialtyFullName = item.Student.Specialty.SpecialtyFullName,
                    CodeOfGroupName = item.Student.Specialty.CodeOfGroupName,
                    Status = item.Status ?? 0
                };
                listStudentGroup.Add(studentGroup);
            }
            return new ApiSuccessResult<List<StudentGroupIdeaDto>>(listStudentGroup);
        }

        public async Task<ApiResult<List<StudentGroupIdeaDto>>> GetInforStudentInGroupIdeaBySemester(int groupIdeaId, int semesterId)
        {

            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Status == 1 || sg.Status == 2);
            expressions.Add(sg => sg.Student.SemesterId == semesterId); //Minhnv bổ sung điều kiên
            expressions.Add(sg => sg.GroupIdea.SemesterId == semesterId);
            var result = await _studentGroupIdeaRepository.GetByConditions(expressions);
            result.OrderBy(sg => sg.Status);
            var listStudentGroup = new List<StudentGroupIdeaDto>();
            foreach (var item in result)
            {
                var studentGroup = new StudentGroupIdeaDto()
                {
                    GroupIdeaId = item.GroupIdea.GroupIdeaId,
                    StudentId = item.Student.StudentId,
                    Avatar = item.Student.StudentNavigation.Avatar,
                    FptEmail = item.Student.StudentNavigation.FptEmail,
                    RollNumber = item.Student.RollNumber,
                    FullName = item.Student.StudentNavigation.FullName,
                    ProfessionFullName = item.Student.Profession.ProfessionFullName,
                    SpecialtyFullName = item.Student.Specialty.SpecialtyFullName,
                    CodeOfGroupName = item.Student.Specialty.CodeOfGroupName,
                    Status = item.Status ?? 0
                };
                listStudentGroup.Add(studentGroup);
            }
            return new ApiSuccessResult<List<StudentGroupIdeaDto>>(listStudentGroup);
        }

        public async Task<ApiResult<string>> GetLeaderIdByGroupIdeaId(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Status == 1 );
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            return new ApiSuccessResult<string>(result.StudentId);
        }

        public async Task<ApiResult<List<StudentGroupIdea>>> GetListRequestByStudentId(string studentId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.Status == 3 || sg.Status == 4 || sg.Status == 5);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditions(expressions);
            return new ApiSuccessResult<List<StudentGroupIdea>>(result);
        }

        public async Task<ApiResult<List<StudentGroupIdea>>> GetListStudentInGroupByGroupIdeaId(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Status == 1 || sg.Status == 2);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditions(expressions);
            result.Select( sg => new StudentGroupIdeaDto()
            {
                StudentId =  sg.StudentId,
                Status = sg.Status.Value,
                GroupIdea = new GroupIdea()
                {
                    ProjectEnglishName = sg.GroupIdea.ProjectEnglishName
                }
            });
            return new ApiSuccessResult<List<StudentGroupIdea>>(result);
        }

        public async Task<ApiResult<List<string>>> GetMemberIdByGroupIdeaId(int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Status == 2);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditions(expressions);
            var studentIds = new List<string>();
            foreach(var student in result)
            {
                studentIds.Add(student.StudentId);
            }
            return new ApiSuccessResult<List<string>>(studentIds);
        }

        public async Task<ApiResult<StudentGroupIdea>> GetStudentGroupIdeaByGroupIdeaIdAndFptEmail(int groupIdeaId, string fptEmail)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Student.StudentNavigation.FptEmail == fptEmail);
            expressions.Add(sg => sg.Student.DeletedAt == null);
            expressions.Add(sg => sg.Student.StudentNavigation.DeletedAt == null);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            return new ApiSuccessResult<StudentGroupIdea>(result);
        }

        public async Task<ApiResult<List<Student>>> GetStudentsHadOneGroupIdea(int groupIdea)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.GroupIdeaId == groupIdea);
            expressions.Add(sg => sg.Status >= 1 && sg.Status <= 2);
            expressions.Add(sg => sg.DeletedAt == null);
            expressions.Add(sg => sg.Student.DeletedAt == null);
            expressions.Add(sg => sg.Student.StudentNavigation.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditions(expressions);
            result.OrderBy(result => result.Status);
            var listStudent = new List<Student>();
            foreach(var sg in result)
            {
                var student = new Student()
                {
                    StudentId = sg.StudentId,
                    StudentNavigation = new User()
                    {
                        FptEmail = sg.Student.StudentNavigation.FptEmail,
                        Avatar = sg.Student.StudentNavigation.Avatar
                    }
                };
                listStudent.Add(student);
            }
            return new ApiSuccessResult<List<Student>>(listStudent);
        }

        public async Task<ApiResult<bool>> RecoveryStudentInGroupIdeaAfterRejected(string studentId, int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.Status == 1 && sg.Status == 2);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            result.DeletedAt = null;
            await _studentGroupIdeaRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusToAccept(string studentId, int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            result.Status = 4;
            await _studentGroupIdeaRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusToLeader(string studentId, int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            result.Status = 1;
            await _studentGroupIdeaRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusToMember(string studentId, int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            result.Status = 2;
            await _studentGroupIdeaRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusToReject(string studentId, int groupIdeaId)
        {
            List<Expression<Func<StudentGroupIdea, bool>>> expressions = new List<Expression<Func<StudentGroupIdea, bool>>>();
            expressions.Add(sg => sg.StudentId == studentId);
            expressions.Add(sg => sg.GroupIdeaId == groupIdeaId);
            expressions.Add(sg => sg.DeletedAt == null);
            var result = await _studentGroupIdeaRepository.GetByConditionId(expressions);
            result.Status = 5;
            await _studentGroupIdeaRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }
    }
}
