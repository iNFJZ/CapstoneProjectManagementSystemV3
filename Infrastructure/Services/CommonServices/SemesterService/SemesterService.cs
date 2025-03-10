using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.AffiliateAccountRepository;
using Infrastructure.Repositories.ChangeFinalGroupRequestRepository;
using Infrastructure.Repositories.ChangeTopicRequestRepository;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.NewsRepository;
using Infrastructure.Repositories.NotificationRepository;
using Infrastructure.Repositories.RegisteredRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.Student_FavoriteGroupIdeaRepository;
using Infrastructure.Repositories.StudentGroupIdeaRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Repositories.SupportRepository;
using Infrastructure.Repositories.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.SemesterService
{
    public class SemesterService : ISemesterService
    {
        private readonly IChangeTopicRequestRepository _changeTopicRequestRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IStudentGroupIdeaRepository _studentGroupIdeaRepository;
        private readonly IRegisteredRepository _registeredRepository;
        private readonly IStudentFavoriteGroupIdeaRepository _studentFavoriteGroupIdeaRepository;
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        private readonly IFinalGroupRepository _finalGroupRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly ISupportRepository _supportRepository;
        private readonly IAffiliateAccountRepository _affiliateAccountRepository;
        private readonly INewsRepository _newsRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IChangeFinalGroupRequestRepository _changeFinalGroupRequestRepository;
        public SemesterService(IChangeTopicRequestRepository changeTopicRequestRepository,
            ISemesterRepository semesterRepository,
            IStudentRepository studentRepository,
            IStudentGroupIdeaRepository studentGroupIdeaRepository,
            IRegisteredRepository registeredRepository,
            IStudentFavoriteGroupIdeaRepository studentFavoriteGroupIdeaRepository,
            IGroupIdeaRepository groupIdeaRepository,
            IFinalGroupRepository finalGroupRepository,
            INotificationRepository notificationRepository,
            ISupportRepository supportRepository,
            IAffiliateAccountRepository affiliateAccountRepository,
            INewsRepository newsRepository,
            IUserRepository userRepository,
            IChangeFinalGroupRequestRepository changeFinalGroupRequestRepository)
        {
            _changeTopicRequestRepository = changeTopicRequestRepository;
            _semesterRepository = semesterRepository;
            _studentRepository = studentRepository;
            _affiliateAccountRepository = affiliateAccountRepository;
            _notificationRepository = notificationRepository;
            _supportRepository = supportRepository;
            _studentGroupIdeaRepository = studentGroupIdeaRepository;
            _registeredRepository = registeredRepository;
            _studentFavoriteGroupIdeaRepository = studentFavoriteGroupIdeaRepository;
            _groupIdeaRepository = groupIdeaRepository;
            _finalGroupRepository = finalGroupRepository;
            _newsRepository = newsRepository;
            _userRepository = userRepository;
            _changeFinalGroupRequestRepository = changeFinalGroupRequestRepository;
        }

        public async Task<ApiResult<bool>> AddNewSemester(Semester semester)
        {
            if (semester == null)
            {
                return new ApiErrorResult<bool>("Dữ liệu học kỳ không hợp lệ.");
            }

            try
            {
                // Thêm mới học kỳ vào database
                await _semesterRepository.CreateAsync(new Semester
                {
                    SemesterName = semester.SemesterName,
                    SemesterCode = semester.SemesterCode,
                    StartTime = semester.StartTime,
                    EndTime = semester.EndTime,
                    DeadlineChangeIdea = semester.DeadlineChangeIdea,
                    DeadlineRegisterGroup = semester.DeadlineRegisterGroup,
                    IsConfirmationOfDevHeadNeeded = semester.IsConfirmationOfDevHeadNeeded,
                    SubjectMailTemplate = semester.SubjectMailTemplate,
                    BodyMailTemplate = semester.BodyMailTemplate
                });
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Lỗi khi thêm học kỳ: {ex.Message}");
            }
        }

        public async Task<ApiResult<bool>> ChangeShowGroupNameStatus(int semesterId, int status)
        {
            var semester = await _semesterRepository.GetById(s => s.SemesterId == semesterId);
            semester.ShowGroupName = status == 0;
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CloseCurrentSemester()
        {
            try
            {
                var currentTime = DateTime.UtcNow;
#pragma warning disable CS8629 // Nullable value type may be null.
                var users = await _userRepository.GetByCondition(u => !new List<int> { 2, 3, 4, 5 }.Contains(u.RoleId.Value));
#pragma warning restore CS8629 // Nullable value type may be null.
                var students = await _studentRepository.GetByCondition(s => s.DeletedAt == null);
                // Xóa mềm tất cả các bảng liên quan
#pragma warning disable CS8629 // Nullable value type may be null.
                var entitiesToUpdate = new List<object>
                {
                    await _studentGroupIdeaRepository.GetByCondition(s => s.DeletedAt == null),
                    await _registeredRepository.GetByCondition(rg => rg.DeletedAt == null),
                    await _studentFavoriteGroupIdeaRepository.GetByCondition(f => f.DeletedAt == null),
                    await _groupIdeaRepository.GetByCondition(gi => gi.DeletedAt == null),
                    await _changeTopicRequestRepository.GetByCondition(ct => ct.DeletedAt == null),
                    await _finalGroupRepository.GetByCondition(fg => fg.DeletedAt == null),
                    await _changeFinalGroupRequestRepository.GetByCondition(cf => cf.DeletedAt == null),
                    await _notificationRepository.GetByCondition(n => n.DeletedAt == null),
                    await _supportRepository.GetByCondition(s => s.DeletedAt == null),
                    await _affiliateAccountRepository.GetByCondition(a => a.DeletedAt == null && users.Select(u => u.UserId).Contains(a.AffiliateAccountId)),
                    await _newsRepository.GetByCondition(n => n.TypeSupport == false && n.DeletedAt == null),
                    await _studentRepository.GetByCondition(s => s.DeletedAt == null),
                    await _userRepository.GetByCondition(u => u.DeletedAt == null && students.Any(s => s.StudentId == u.UserId) && !new List<int> { 2, 3, 4, 5 }.Contains(u.RoleId.Value)),
                    await _semesterRepository.GetByCondition(s => s.DeletedAt == null)
                    };
#pragma warning restore CS8629 // Nullable value type may be null.

                foreach (var entityList in entitiesToUpdate)
                {
                    foreach (var entity in (IEnumerable<dynamic>)entityList)
                    {
                        entity.DeletedAt = currentTime;
                    }
                }

                // Cập nhật trạng thái của Semesters
                var semesters = await _semesterRepository.GetByCondition(s => s.DeletedAt == null);
                foreach (var semester in semesters)
                {
                    semester.StatusCloseBit = false;
                    semester.DeletedAt = currentTime;
                }
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Lỗi khi xóa dữ liệu: {ex.Message}");
            }
        }

        public Task<ApiResult<string>> generateGroupInformationMailContent(string groupName, List<StudentGroupIdea> students, string projectEnglishName, string projectVietnamese, string abbreviation, Supervisor supervisor)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<SemesterDto>>> GetAllSemester()
        {
            var semesterList = await _semesterRepository.GetByCondition(s => true);
            semesterList.OrderBy(s => s.SemesterId).ToList();
            var result = new List<SemesterDto>();
            foreach (var semester in semesterList)
            {
                result.Add(new SemesterDto()
                {
                    SemesterID = semester.SemesterId,
                    SemesterName = semester.SemesterName,
                    SemesterCode = semester.SemesterCode,
                    StartTime = semester.StartTime,
                    EndTime = semester.EndTime
                });
            }
            return new ApiSuccessResult<List<SemesterDto>>(result);
        }

        public async Task<ApiResult<SemesterDto>> GetCurrentSemester()
        {
            var semester = await _semesterRepository.GetById(s => s.StatusCloseBit == true && s.DeletedAt == null);
            var result = new SemesterDto()
            {
                SemesterID = semester.SemesterId,
                SemesterName = semester.SemesterName,
                SemesterCode = semester.SemesterCode,
                StartTime = semester.StartTime,
                EndTime = semester.EndTime,
                ShowGroupName = semester.ShowGroupName,
                DeadlineChangeIdea = semester.DeadlineChangeIdea,
                DeadlineRegisterGroup = semester.DeadlineRegisterGroup,
                IsConfirmationOfDevHeadNeeded = semester.IsConfirmationOfDevHeadNeeded,
                SubjectMailTemplate = semester.SubjectMailTemplate,
                BodyMailTemplate = semester.BodyMailTemplate
            };
            return new ApiSuccessResult<SemesterDto>(result);
        }

        public async Task<ApiResult<SemesterDto>> GetLastSemester()
        {
            var semesterList = await _semesterRepository.GetByCondition(s => s.StatusCloseBit == true && s.DeletedAt == null);
            var lastSemester = semesterList.OrderBy(s => s.SemesterId).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var result = new SemesterDto()
            {
                SemesterID = lastSemester.SemesterId,
                SemesterName = lastSemester.SemesterName,
                SemesterCode = lastSemester.SemesterCode,
                StartTime = lastSemester.StartTime,
                EndTime = lastSemester.EndTime
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return new ApiSuccessResult<SemesterDto>(result);
        }

        public async Task<ApiResult<SemesterDto>> GetLastSemesterDeleteAt()
        {
            var semesterDeleteList = await _semesterRepository.GetByCondition(s => s.DeletedAt != null);
            var lastSemester = semesterDeleteList.OrderBy(s => s.SemesterId).FirstOrDefault();
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var result = new SemesterDto()
            {
                SubjectMailTemplate = lastSemester.SubjectMailTemplate,
                BodyMailTemplate = lastSemester.BodyMailTemplate
            };
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            return new ApiSuccessResult<SemesterDto>(result);
        }

        public async Task<ApiResult<SemesterDto>> GetSemesterById(int semesterId)
        {
            var semester = await _semesterRepository.GetById(s => s.SemesterId == semesterId);
            var result = new SemesterDto()
            {
                SemesterID = semester.SemesterId,
                SemesterName = semester.SemesterName,
                SemesterCode = semester.SemesterCode,
                StartTime = semester.StartTime,
                EndTime = semester.EndTime
            };
            return new ApiSuccessResult<SemesterDto>(result);
        }

        public async Task<ApiResult<bool>> UpdateCurrentSemester(Semester semester)
        {
            var result = await _semesterRepository.GetById(s => s.SemesterId == semester.SemesterId && s.StatusCloseBit == true);
            result.SemesterName = semester.SemesterName;
            result.SemesterCode = semester.SemesterCode;
            result.StartTime = semester.StartTime;
            result.EndTime = semester.EndTime;
            result.DeadlineChangeIdea = semester.DeadlineChangeIdea;
            result.DeadlineRegisterGroup = semester.DeadlineRegisterGroup;
            result.IsConfirmationOfDevHeadNeeded = semester.IsConfirmationOfDevHeadNeeded;
            result.SubjectMailTemplate = semester.SubjectMailTemplate;
            result.BodyMailTemplate = semester.BodyMailTemplate;
            await _semesterRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }
    }
}
