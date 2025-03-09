using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.DBContext;
using Infrastructure.Repositories.ChangeTopicRequestRepository;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.StudentRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.Services.CommonServices.MailService;
using Infrastructure.Services.CommonServices.NotificationService;
using Infrastructure.Services.PrivateService.StaffService;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ChangeTopicRequestService
{
    public class ChangeTopicRequestService : IChangeTopicRequestService
    {
        private readonly IChangeTopicRequestRepository _changeTopicRequestRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IMailService _mailService;
        private readonly IStudentRepository _studentRepository;
        private readonly IFinalGroupRepository _finalGroupRepository;
        private readonly IProfessionRepository _professionRepository;

        public ChangeTopicRequestService(IChangeTopicRequestRepository changeTopicRequestRepository,
            IStaffRepository staffRepository,
            IUserRepository userRepository,
            INotificationService notificationService,
            ISupervisorRepository supervisorRepository,
            IMailService mailService,
            IStudentRepository studentRepository,
            IProfessionRepository professionRepository,
            IFinalGroupRepository finalGroupRepository)
        {
            _changeTopicRequestRepository = changeTopicRequestRepository;
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
            _supervisorRepository = supervisorRepository;
            _mailService = mailService;
            _studentRepository = studentRepository;
            _finalGroupRepository = finalGroupRepository;
            _professionRepository = professionRepository;
        }
        public async Task<ApiResult<bool>> AddChangeTopicRequest(ChangeTopicRequest changeTopicRequest)
        {
            var changeTopic = new ChangeTopicRequest()
            {
                OldTopicNameEnglish = changeTopicRequest.OldTopicNameEnglish,
                OldTopicNameVietNamese = changeTopicRequest.OldTopicNameVietNamese,
                OldAbbreviation = changeTopicRequest.OldAbbreviation,
                NewTopicNameEnglish = changeTopicRequest.NewTopicNameEnglish,
                NewTopicNameVietNamese = changeTopicRequest.NewTopicNameVietNamese,
                NewAbbreviation = changeTopicRequest.NewAbbreviation,
                EmailSuperVisor = changeTopicRequest.EmailSuperVisor,
                ReasonChangeTopic = changeTopicRequest.ReasonChangeTopic,
                FinalGroupId = changeTopicRequest.FinalGroupId,
            };
            await _changeTopicRequestRepository.CreateAsync(changeTopic);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CancelChangeTopicRequestBySupervisor(int requestId, int status, bool isBeforeDeadline, bool isDevHead, string userId, HttpRequest httpRequest)
        {
            ChangeTopicRequest request = await _changeTopicRequestRepository.GetById(ctp => ctp.RequestId == requestId && ctp.DeletedAt == null);
            FinalGroup finalGroup = request.FinalGroup;
            bool isRequestOfUser = request.EmailSuperVisor.Equals(userId);
            string contentNotification = "";
            if ((status == 3 && isBeforeDeadline) || (status == 3 && isDevHead && isRequestOfUser))
            {
                var users = await _userRepository.GetByCondition(u => u.RoleId == 3);
                List<Staff> listStaff = users.Select(u => new Staff
                {
                    StaffId = u.UserId ?? "",
                    StaffNavigation = new User
                    {
                        FptEmail = u.FptEmail ?? ""
                    }
                }).ToList();
                if (listStaff != null)
                {
                    foreach (Staff staff in listStaff)
                    {
                        contentNotification = $"Supervisor {userId} has canceled their confirmation of request change topic for group {finalGroup.GroupName}";
                        _notificationService.InsertDataNotification(staff.StaffId, contentNotification, "/ManageChangeTopic/Index");
                    }
                }
                return new ApiSuccessResult<bool>(await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(requestId, 0, ""));
            }
            else if (status == 2)
            {
                var devheads = await _supervisorRepository.GetByCondition(s => s.SupervisorProfessions.Any(sp => sp.ProfessionId == finalGroup.Profession.ProfessionId));
                contentNotification = $"Supervisor {userId} has canceled their confirmation of request change topic for group {finalGroup.GroupName}";
                foreach (Supervisor supervisor in devheads)
                {
                    _notificationService.InsertDataNotification(supervisor.SupervisorId, contentNotification, "/SupervisorChangeTopicRequest/Index?status=2");
                }
                contentNotification += $"<a href=\"{httpRequest?.Scheme}://{httpRequest?.Host.Value}/SupervisorChangeTopicRequest/Index?status=2\">Change Topic Requests</a>";
                _mailService.SendMailNotification(string.Join(",", devheads.Select(devhead => devhead.SupervisorNavigation.FptEmail)), null, "Change Topic Requests", contentNotification);
                return new ApiSuccessResult<bool>(await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(requestId, 0, ""));
            }
            var user = await _userRepository.GetByCondition(u => u.RoleId == 3);
            List<Staff> staffs = user.Select(u => new Staff
            {
                StaffId = u.UserId ?? "",
                StaffNavigation = new User
                {
                    FptEmail = u.FptEmail ?? ""
                }
            }).ToList();
            if (staffs != null)
            {
                foreach (Staff staff in staffs)
                {
                    contentNotification = $"Department leader {userId} has canceled their confirmation of request change topic for group {finalGroup.GroupName}";
                    _notificationService.InsertDataNotification(staff.StaffId, contentNotification, "/ManageChangeTopic/Index");
                }
            }
            return new ApiSuccessResult<bool>(await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(requestId, 2, ""));
        }

        public async Task<ApiResult<bool>> checkContainStatus(int[] statuses, int status)
        {
            if (statuses == null || statuses.Length == 0) return new ApiSuccessResult<bool>(true);
            foreach (int s in statuses)
            {
                if (s == status) return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<int>> CountRecordChangeTopicRequestsBySearchText(string searchText, int status, int semesterId)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = string.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var changeTopics = await _changeTopicRequestRepository.GetByCondition(ctr => ctr.Status == status && ctr.DeletedAt == null &&
            ctr.FinalGroup.SemesterId == semesterId && (searchText == "" || ctr.FinalGroup.GroupName.ToUpper().Replace(" ", "").Contains(searchText)));
            var count = changeTopics.Count();
            return new ApiSuccessResult<int>(count);
        }

        public async Task<ApiResult<bool>> DeleteChangeTopicRequestsByFinalGroup(int finalGroupId)
        {
            var changeTopic = await _changeTopicRequestRepository.GetById(ctr => ctr.FinalGroupId == finalGroupId && ctr.DeletedAt == null);
            changeTopic.DeletedAt = DateTime.Now;
            await _changeTopicRequestRepository.UpdateAsync(changeTopic);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<ChangeTopicRequestDto>>> GetChangeTopicRequestsBySearchText(string searchText, int status, int semesterId, int offsetNumber, int fetchNumber)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = string.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var changeTopicRequests = await _changeTopicRequestRepository.GetByCondition(ctr => ctr.Status == status && ctr.DeletedAt == null &&
            ctr.FinalGroup.SemesterId == semesterId && (searchText == "" || ctr.FinalGroup.GroupName.ToUpper().Replace(" ", "").Contains(searchText)));
            var result = new List<ChangeTopicRequestDto>();
            foreach (var request in changeTopicRequests)
            {
                result.Add(new ChangeTopicRequestDto()
                {
                    RequestID = request.RequestId,
                    FinalGroup = new FinalGroupDto()
                    {
                        GroupName = request.FinalGroup.GroupName
                    },
                    OldTopicNameEnglish = request.OldTopicNameEnglish,
                    NewTopicNameEnglish = request.NewTopicNameEnglish,
                    EmailSuperVisor = request.EmailSuperVisor,
                    StaffComment = request.StaffComment,
                    Status = request.Status.Value
                });
            }
            return new ApiSuccessResult<List<ChangeTopicRequestDto>>(result);
        }

        public async Task<ApiResult<List<ChangeTopicRequestDto>>> GetChangeTopicRequestsByStudentId(string studentId, int semesterId)
        {
            var student = await _studentRepository.GetByCondition(s => s.StudentId == studentId);
            var changeTopicRequests = await _changeTopicRequestRepository.GetByCondition(c => student.Select(s => s.FinalGroupId).Contains(c.FinalGroupId) &&
            c.FinalGroup.SemesterId == semesterId &&
            c.DeletedAt == null);
            var result = new List<ChangeTopicRequestDto>();
            foreach (var request in changeTopicRequests)
            {
                result.Add(new ChangeTopicRequestDto()
                {
                    RequestID = request.RequestId,
                    FinalGroup = new FinalGroupDto()
                    {
                        GroupName = request.FinalGroup.GroupName
                    },
                    OldTopicNameEnglish = request.OldTopicNameEnglish,
                    NewTopicNameEnglish = request.NewTopicNameEnglish,
                    EmailSuperVisor = request.EmailSuperVisor,
                    StaffComment = request.StaffComment,
                    Status = request.Status.Value
                });
            }
            return new ApiSuccessResult<List<ChangeTopicRequestDto>>(result);
        }

        public async Task<ApiResult<(int, int, List<ChangeTopicRequestDto>)>> GetChangeTopicRequestsBySupervisorEmail(string supervisorEmail, string searchText, int status, int semesterId, int offsetNumber, int fetchNumber, bool isDevHead, int[] professions, bool isConfirmationOfDevHeadNeeded, int[] statuses, string supervisorEmails)
        {
            offsetNumber--;
            if (offsetNumber < 0) { offsetNumber = 0; }
            List<ChangeTopicRequest> changeTopicRequests = new List<ChangeTopicRequest>();
            var result = new List<ChangeTopicRequestDto>();
            foreach (var request in changeTopicRequests)
            {
                result.Add(new ChangeTopicRequestDto()
                {
                    RequestID = request.RequestId,
                    FinalGroup = new FinalGroupDto()
                    {
                        GroupName = request.FinalGroup.GroupName
                    },
                    OldTopicNameEnglish = request.OldTopicNameEnglish,
                    NewTopicNameEnglish = request.NewTopicNameEnglish,
                    EmailSuperVisor = request.EmailSuperVisor,
                    StaffComment = request.StaffComment,
                    Status = request.Status.Value
                });
            }
            int totalPage = 0;
            if (status == 0)
            {
                var topicRequest = await _changeTopicRequestRepository.GetChangeTopicRequestsBySupervisorEmail(supervisorEmail, searchText, statuses == null || statuses.Length == 0 ? new int[] { status, 3, 2, 4 } : statuses, semesterId, offsetNumber, fetchNumber);
                int totalRecord = topicRequest.Count();
                totalPage = (totalRecord / fetchNumber) + (totalRecord % fetchNumber > 0 ? 1 : 0);
                if (offsetNumber >= totalPage) offsetNumber = totalPage - 1;
                changeTopicRequests.AddRange(topicRequest);
            }
            else if (status == 2 && isDevHead && isConfirmationOfDevHeadNeeded && professions != null)
            {
                var topicRequestByProfession = await _changeTopicRequestRepository.GetChangeTopicRequestsByProfessionId(professions, searchText, statuses == null || statuses.Length == 0 ? new int[] { 3, 2 } : statuses, semesterId, offsetNumber, fetchNumber, supervisorEmails);
                int totalRecord = topicRequestByProfession.Count();
                totalPage = (totalRecord / fetchNumber) + (totalRecord % fetchNumber > 0 ? 1 : 0);
                if (offsetNumber >= totalPage) offsetNumber = totalPage - 1;
                var temp = topicRequestByProfession;
                if (temp != null) { changeTopicRequests.AddRange(temp); }
            }
            return new ApiSuccessResult<(int, int, List<ChangeTopicRequestDto>)>((offsetNumber, totalPage, result));
        }

        public async Task<ApiResult<ChangeTopicRequestDto>> GetDetailChangeTopicRequestsByRequestId(int requestId)
        {
            var changeTopicRequest = (await _changeTopicRequestRepository.GetByCondition(c =>
        c.RequestId == requestId && c.DeletedAt == null)).FirstOrDefault();
            var result = new ChangeTopicRequestDto()
            {
                RequestID = changeTopicRequest.RequestId,
                FinalGroup = new FinalGroupDto()
                {
                    FinalGroupId = changeTopicRequest.FinalGroupId,
                    GroupName = changeTopicRequest.FinalGroup.GroupName,
                    Profession = new ProfessionDto()
                    {
                        ProfessionID = changeTopicRequest.FinalGroup.ProfessionId
                    }
                },
                OldTopicNameEnglish = changeTopicRequest.OldTopicNameEnglish,
                OldTopicNameVietNamese = changeTopicRequest.OldTopicNameVietNamese,
                NewTopicNameEnglish = changeTopicRequest.NewTopicNameEnglish,
                NewTopicNameVietNamese = changeTopicRequest.NewTopicNameVietNamese,
                OldAbbreviation = changeTopicRequest.OldAbbreviation,
                NewAbbreviation = changeTopicRequest.NewAbbreviation,
                EmailSuperVisor = changeTopicRequest.EmailSuperVisor,
                StaffComment = changeTopicRequest.StaffComment,
                ReasonChangeTopic = changeTopicRequest.ReasonChangeTopic,
                Status = changeTopicRequest.Status.Value
            };
            if (result == null)
            {
                return new ApiErrorResult<ChangeTopicRequestDto>("Không tìm thấy yêu cầu đổi đề tài.");
            }
            var finalGroup = (await _finalGroupRepository.GetByCondition(f =>
                f.FinalGroupId == changeTopicRequest.FinalGroupId)).FirstOrDefault();
            var profession = (await _professionRepository.GetByCondition(p =>
                p.ProfessionId == finalGroup.ProfessionId)).FirstOrDefault();
            if (finalGroup != null)
            {
                changeTopicRequest.FinalGroup.GroupName = finalGroup.GroupName;
            }

            if (profession != null)
            {
                changeTopicRequest.FinalGroup.ProfessionId = profession.ProfessionId;
            }
            return new ApiSuccessResult<ChangeTopicRequestDto>(result);
        }

        public async Task<ApiResult<ChangeTopicRequestDto>> GetNewTopicByChangeTopicRequestId(int changeTopicRequestId)
        {
            var changeTopicRequest = await _changeTopicRequestRepository.GetById(ctr => ctr.RequestId == changeTopicRequestId && ctr.DeletedAt == null);
            var result = new ChangeTopicRequestDto()
            {
                FinalGroup = new FinalGroupDto()
                {
                    FinalGroupId = changeTopicRequest.FinalGroupId,
                },
                NewTopicNameEnglish = changeTopicRequest.NewTopicNameEnglish,
                NewTopicNameVietNamese = changeTopicRequest.NewTopicNameVietNamese,
                NewAbbreviation = changeTopicRequest.NewAbbreviation
            };
            if (result == null)
            {
                return new ApiErrorResult<ChangeTopicRequestDto>("Không tìm thấy đối tượng");
            }
            return new ApiSuccessResult<ChangeTopicRequestDto>(result);
        }

        public async Task<ApiResult<bool>> UpdateChangeTopicRequestBySupervisor(bool isDevHead, bool isBeforeDeadline, bool isAccepted, int requestId, bool isConfirmationOfDevHeadNeeded, string userId, HttpRequest httpRequest)
        {
            var changeTopicRequest = (await _changeTopicRequestRepository.GetByCondition(c => c.RequestId == requestId))
                                .FirstOrDefault();
            if (changeTopicRequest == null)
            {
                return new ApiErrorResult<bool>("Change topic request not found");
            }

            var finalGroup = (await _finalGroupRepository.GetByCondition(f => f.FinalGroupId == changeTopicRequest.FinalGroupId))
                                .FirstOrDefault();
            if (finalGroup == null)
            {
                return new ApiErrorResult<bool>("Final group not found");
            }

            var profession = (await _professionRepository.GetByCondition(p => p.ProfessionId == finalGroup.ProfessionId))
                                .FirstOrDefault();
            string groupName = finalGroup.GroupName;

            if (isAccepted)
            {
                if ((!isBeforeDeadline && isDevHead) || isBeforeDeadline || !isConfirmationOfDevHeadNeeded)
                {
                    var users = await _userRepository.GetByCondition(u => u.RoleId == 3);
                    List<Staff> staffs = users.Select(u => new Staff
                    {
                        StaffId = u.UserId ?? "",
                        StaffNavigation = new User
                        {
                            FptEmail = u.FptEmail ?? ""
                        }
                    }).ToList();
                    if (staffs?.Count > 0)
                    {
                        foreach (var staff in staffs)
                        {
                            var contentNotification = isDevHead
                                ? $"Department leader {userId} has confirmed request change topic of group {groupName}"
                                : $"Supervisor {userId} has confirmed request change topic of group {groupName}";
                            await _notificationService.InsertDataNotification(staff.StaffId, contentNotification, "/ManageChangeTopic/Index");
                        }
                    }
                    await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(requestId, 3, "");
                    return new ApiSuccessResult<bool>(true);
                }
                else if (!isBeforeDeadline && !isDevHead)
                {
                    var devheads = await _supervisorRepository.GetByCondition(s => s.SupervisorProfessions.Any(sp => sp.ProfessionId == finalGroup.Profession.ProfessionId));
                    if (devheads?.Count > 0)
                    {
                        var contentNotification = $"Supervisor {userId} has confirmed request change topic of group {groupName} ";
                        contentNotification += $"<a href=\"{httpRequest.Scheme}://{httpRequest.Host.Value}/SupervisorChangeTopicRequest/Index?status=2\">Change Topic Requests</a>";

                        foreach (var supervisor in devheads)
                        {
                            await _notificationService.InsertDataNotification(supervisor.SupervisorId, contentNotification, "/SupervisorChangeTopicRequest/Index?status=2");
                        }
                        await _mailService.SendMailNotification(string.Join(",", devheads.Select(d => d.SupervisorNavigation.FptEmail)), null, "Change Topic Requests", contentNotification);
                    }
                    await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(requestId, 2, "");
                    return new ApiSuccessResult<bool>(true);
                }
            }

            var students = await _studentRepository.GetByCondition(s => s.FinalGroupId == finalGroup.FinalGroupId);
            string attachedLinkForStudent = "/MyGroup/Index";
            string contentNotificationForStudent = "Your group's changing topic request has been rejected";
            string listEmailMemberInGroup = string.Join(",", students.Select(async s => (await _userRepository.GetById(u => u.UserId == s.StudentId)).FptEmail));

            foreach (var student in students)
            {
                await _notificationService.InsertDataNotification(student.StudentId, contentNotificationForStudent, attachedLinkForStudent);
            }
            contentNotificationForStudent += $"<a href=\"{httpRequest.Scheme}://{httpRequest.Host.Value}/MyGroup/Index\">Your Group</a>";
            await _mailService.SendMailNotification(listEmailMemberInGroup, null, "Change Topic Request", contentNotificationForStudent);

            await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(requestId, -1, "");
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusOfChangeTopicRequest(int changeTopicRequestId, int status, string staffComment)
        {
            await _changeTopicRequestRepository.UpdateStatusOfChangeTopicRequest(changeTopicRequestId, status, staffComment);
            return new ApiSuccessResult<bool>(true);
        }
    }
}
