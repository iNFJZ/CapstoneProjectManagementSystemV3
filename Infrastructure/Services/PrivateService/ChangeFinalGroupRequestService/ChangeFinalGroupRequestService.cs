using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.ChangeFinalGroupRequestRepository;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.Repositories.StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ChangeFinalGroupRequestService
{
    public class ChangeFinalGroupRequestService : IChangeFinalGroupRequestService
    {
        private readonly IChangeFinalGroupRequestRepository _changeFinalGroupRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IFinalGroupRepository _finalGroupRepository;
        public ChangeFinalGroupRequestService(
            IChangeFinalGroupRequestRepository changeFinalGroupRepository,
            IStudentRepository studentRepository,
            IFinalGroupRepository finalGroupRepository)
        {
            _changeFinalGroupRepository = changeFinalGroupRepository;
            _studentRepository = studentRepository;
            _finalGroupRepository = finalGroupRepository;
        }

        public async Task<ApiResult<int>> CountRecordChangeFinalGroupBySearchText(string searchText, int status, int semesterId)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = string.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var result = await _changeFinalGroupRepository.CountRecordChangeFinalGroupBySearchText(searchText, status, semesterId);
            return new ApiSuccessResult<int>(result);
        }

        public async Task<ApiResult<bool>> CreateChangeFinalGroupRequestDao(string fromStudentId, string toStudentId)
        {
            Expression<Func<Student, bool>> FromStudentExpression = x => x.StudentId ==fromStudentId;
            var fromStudent = await _studentRepository.GetById(FromStudentExpression);
            Expression<Func<Student, bool>> ToStudentExpression = x => x.StudentId == fromStudentId;
            var toStudent = await _studentRepository.GetById(ToStudentExpression);
            var changFinalGroup = new ChangeFinalGroupRequest()
            {
                FromStudentId = fromStudentId,
                ToStudentId = toStudentId,
                FromStudent = fromStudent,
                ToStudent = toStudent,
            };
            await _changeFinalGroupRepository.CreateAsync(changFinalGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<string>> GetFromStudentIdByChangeFinalGroupRequestIdAndToStudentId(int changeFinalGroupRequestId, string toStudentId)
        {
            List<Expression<Func<ChangeFinalGroupRequest, bool>>> cfgExpression = new List<Expression<Func<ChangeFinalGroupRequest, bool>>>();
            cfgExpression.Add(x => x.ToStudentId == toStudentId);
            cfgExpression.Add(x => x.ChangeFinalGroupRequestId == changeFinalGroupRequestId);
            var changeFinalGroup = await _changeFinalGroupRepository.GetByConditionId(cfgExpression);
            if(changeFinalGroup == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy người gửi yêu cầu");
            }
            return new ApiSuccessResult<string>(changeFinalGroup.FromStudentId);
        }

        public async Task<ApiResult<ChangeFinalGroupRequest>> GetInforOfStudentExchangeFinalGroup(int changeFinalGroupRequestId)
        {
            Expression<Func<ChangeFinalGroupRequest, bool>> cfgExpression = x => x.ChangeFinalGroupRequestId == changeFinalGroupRequestId;
            var changeFinalGroupRequest = await _changeFinalGroupRepository.GetById(cfgExpression);
            return new ApiSuccessResult<ChangeFinalGroupRequest>(changeFinalGroupRequest);
        }

        public async Task<ApiResult<List<ChangeFinalGroupRequest>>> GetListChangeFinalGroupRequest(string studentId, int semesterId)
        {
            List<Expression<Func<ChangeFinalGroupRequest, bool>>> cfgExpression = new List<Expression<Func<ChangeFinalGroupRequest, bool>>>();
            cfgExpression.Add(x => x.ToStudentId == studentId || x.FromStudentId == studentId);
            cfgExpression.Add(x => x.StatusOfTo == 1);
            cfgExpression.Add(x => x.DeletedAt == null);
            var changeFinalGroup = await _changeFinalGroupRepository.GetByConditions(cfgExpression);
            changeFinalGroup.OrderByDescending(x => x.DeletedAt);
            //Lấy danh sách sinh viên được gửi yêu cầu
            Expression<Func<Student, bool>> studentExpression = x => x.StudentId == changeFinalGroup.Select(r => r.FromStudentId).FirstOrDefault();
            var fromStudent = await _studentRepository.GetById(studentExpression);
            //Lấy danh sách sinh viên nhận yêu cầu
            Expression<Func<Student, bool>> toStudentExpression = x => x.StudentId == changeFinalGroup.Select(r => r.ToStudentId).FirstOrDefault();
            var toStudent = await _studentRepository.GetById(studentExpression);
            //Lọc them học kỳ
            List<Expression<Func<FinalGroup, bool>>> fgExpression = new List<Expression<Func<FinalGroup, bool>>>();
            fgExpression.Add(x => x.SemesterId == semesterId);
            fgExpression.Add(x => x.DeletedAt == null);
            var finalFroups = await _finalGroupRepository.GetByConditions(fgExpression);
            var result = changeFinalGroup.Where(r => finalFroups.Any(g => g.FinalGroupId == r.FromStudent.FinalGroupId) &&
                                                     finalFroups.Any(g => g.FinalGroupId == r.ToStudent.FinalGroupId))
                                                     .Select(r => new ChangeFinalGroupRequest
                                                     {
                                                         ChangeFinalGroupRequestId = r.ChangeFinalGroupRequestId,
                                                         FromStudent = new Student()
                                                         {
                                                             EmailAddress = r.FromStudent.EmailAddress,
                                                             GroupName = r.FromStudent.GroupName,
                                                         },
                                                         ToStudent = new Student()
                                                         {
                                                             EmailAddress = r.ToStudent.EmailAddress,
                                                             GroupName = r.ToStudent.GroupName,
                                                         },
                                                         StatusOfTo = r.StatusOfTo
                                                     }).ToList();
            return new ApiSuccessResult<List<ChangeFinalGroupRequest>>(result);
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<ChangeFinalGroupRequest>>> GetListChangeFinalGroupRequestBySearchText(string searchText, int status, int semesterId, int offsetNumber, int fetchNumber)
        {
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = string.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var result = await _changeFinalGroupRepository.GetListChangeFinalGroupRequestBySearchText(searchText,status,semesterId,offsetNumber,fetchNumber);
            return new ApiSuccessResult<List<ChangeFinalGroupRequest>>(result);
        }

        public async Task<ApiResult<List<ChangeFinalGroupRequest>>> GetListChangeFinalGroupRequestFromOfStudent(string fromStudentId, int semesterId)
        {
            List<Expression<Func<ChangeFinalGroupRequest, bool>>> cfgExpression = new List<Expression<Func<ChangeFinalGroupRequest, bool>>>();
            cfgExpression.Add(x => x.FromStudentId == fromStudentId);
            cfgExpression.Add(x => x.DeletedAt == null);
            var changeFinalGroup = await _changeFinalGroupRepository.GetByConditions(cfgExpression);
            changeFinalGroup.OrderByDescending(x => x.DeletedAt);
            //Lấy danh sách sinh viên được gửi yêu cầu
            Expression<Func<Student, bool>> studentExpression = x => x.StudentId == changeFinalGroup.Select(r => r.FromStudentId).FirstOrDefault();
            var fromStudent = await _studentRepository.GetById(studentExpression);
            //Lấy danh sách sinh viên nhận yêu cầu
            Expression<Func<Student, bool>> toStudentExpression = x => x.StudentId == changeFinalGroup.Select(r => r.ToStudentId).FirstOrDefault();
            var toStudent = await _studentRepository.GetById(studentExpression);
            //Lọc them học kỳ
            List<Expression<Func<FinalGroup, bool>>> fgExpression = new List<Expression<Func<FinalGroup, bool>>>();
            fgExpression.Add(x => x.SemesterId == semesterId);
            fgExpression.Add(x => x.DeletedAt == null);
            var finalFroups = await _finalGroupRepository.GetByConditions(fgExpression);
            var result = changeFinalGroup.Where(r => finalFroups.Any(g => g.FinalGroupId == r.FromStudent.FinalGroupId) &&
                                                     finalFroups.Any(g => g.FinalGroupId == r.ToStudent.FinalGroupId))
                                                     .Select(r => new ChangeFinalGroupRequest
                                                     {
                                                         ChangeFinalGroupRequestId = r.ChangeFinalGroupRequestId,
                                                         FromStudent = new Student()
                                                         {
                                                             EmailAddress = r.FromStudent.EmailAddress,
                                                             GroupName = r.FromStudent.GroupName,
                                                         },
                                                         ToStudent = new Student()
                                                         {
                                                             EmailAddress = r.ToStudent.EmailAddress,
                                                             GroupName = r.ToStudent.GroupName,
                                                         },
                                                         StatusOfTo = r.StatusOfTo
                                                     }).ToList();
            return new ApiSuccessResult<List<ChangeFinalGroupRequest>>(result);
        }

        public async Task<ApiResult<List<ChangeFinalGroupRequest>>> GetListChangeFinalGroupRequestToOfStudent(string toStudentId, int semesterId)
        {
            List<Expression<Func<ChangeFinalGroupRequest, bool>>> cfgExpression = new List<Expression<Func<ChangeFinalGroupRequest, bool>>>();
            cfgExpression.Add(x => x.ToStudentId == toStudentId);
            cfgExpression.Add(x => x.DeletedAt == null);
            var changeFinalGroup = await _changeFinalGroupRepository.GetByConditions(cfgExpression);
            changeFinalGroup.OrderByDescending(x => x.DeletedAt);
            //Lấy danh sách sinh viên được gửi yêu cầu
            Expression<Func<Student, bool>> studentExpression = x => x.StudentId == changeFinalGroup.Select(r => r.FromStudentId).FirstOrDefault();
            var fromStudent = await _studentRepository.GetById(studentExpression);
            //Lấy danh sách sinh viên nhận yêu cầu
            Expression<Func<Student, bool>> toStudentExpression = x => x.StudentId == changeFinalGroup.Select(r => r.ToStudentId).FirstOrDefault();
            var toStudent = await _studentRepository.GetById(studentExpression);
            //Lọc them học kỳ
            List<Expression<Func<FinalGroup, bool>>> fgExpression = new List<Expression<Func<FinalGroup, bool>>>();
            fgExpression.Add(x => x.SemesterId == semesterId);
            fgExpression.Add(x => x.DeletedAt == null);
            var finalFroups = await _finalGroupRepository.GetByConditions(fgExpression);
            var result = changeFinalGroup.Where(r => finalFroups.Any(g => g.FinalGroupId == r.FromStudent.FinalGroupId) &&
                                                     finalFroups.Any(g => g.FinalGroupId == r.ToStudent.FinalGroupId))
                                                     .Select(r => new ChangeFinalGroupRequest
                                                     {
                                                         ChangeFinalGroupRequestId = r.ChangeFinalGroupRequestId,
                                                         FromStudent = new Student()
                                                         {
                                                             EmailAddress = r.FromStudent.EmailAddress,
                                                             GroupName = r.FromStudent.GroupName,
                                                         },
                                                         ToStudent = new Student()
                                                         {
                                                             EmailAddress = r.ToStudent.EmailAddress,
                                                             GroupName = r.ToStudent.GroupName,
                                                         },
                                                         StatusOfTo = r.StatusOfTo
                                                     }).ToList();
            return new ApiSuccessResult<List<ChangeFinalGroupRequest>>(result);
        }

        public async Task<ApiResult<bool>> UpdateGroupForStudentByChangeFinalGroupRequest(ChangeFinalGroupRequest changeFinalGroupRequest)
        {
            Expression<Func<ChangeFinalGroupRequest , bool>> cfgExpression = x => x.ChangeFinalGroupRequestId == changeFinalGroupRequest.ChangeFinalGroupRequestId;
            var changeFinalGroup = await _changeFinalGroupRepository.GetById(cfgExpression);
            if (changeFinalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            changeFinalGroup.StatusOfStaff = 1;
            await _changeFinalGroupRepository.UpdateAsync(changeFinalGroup);
            Expression<Func<Student, bool>> studentExpression = x => x.StudentId == changeFinalGroupRequest.FromStudentId;
            var student = await _studentRepository.GetById(studentExpression);
            if (student == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            student.FinalGroupId = changeFinalGroup.FromStudent.FinalGroupId;
            student.IsLeader = changeFinalGroup.FromStudent.IsLeader;
            await _studentRepository.UpdateAsync(student);
            Expression<Func<Student, bool>> toStudentExpression = x => x.StudentId == changeFinalGroupRequest.FromStudentId;
            var toStudent = await _studentRepository.GetById(studentExpression);
            if (toStudent == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            toStudent.FinalGroupId = changeFinalGroup.ToStudent.FinalGroupId;
            toStudent.IsLeader = changeFinalGroup.ToStudent.IsLeader;
            await _studentRepository.UpdateAsync(toStudent);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusAcceptOfToStudentByChangeFinalGroupRequestId(int changeFinalGroupRequestId)
        {
            Expression<Func<ChangeFinalGroupRequest, bool>> cfgExpression = x => x.ChangeFinalGroupRequestId == changeFinalGroupRequestId;
            var changeFinalGroup = await _changeFinalGroupRepository.GetById(cfgExpression);
            if (changeFinalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            changeFinalGroup.StatusOfTo = 1;
            await _changeFinalGroupRepository.UpdateAsync(changeFinalGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusOfStaffByChangeFinalGroupRequestId(int changeFinalGroupRequestId, string staffComment)
        {
            Expression<Func<ChangeFinalGroupRequest, bool>> cfgExpression = x => x.ChangeFinalGroupRequestId == changeFinalGroupRequestId;
            var changeFinalGroup = await _changeFinalGroupRepository.GetById(cfgExpression);
            if (changeFinalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            changeFinalGroup.StatusOfStaff = 2;
            changeFinalGroup.StaffComment = staffComment;
            await _changeFinalGroupRepository.UpdateAsync(changeFinalGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateStatusRejectOfToStudentByChangeFinalGroupRequestId(int changeFinalGroupRequestId)
        {
            Expression<Func<ChangeFinalGroupRequest, bool>> cfgExpression = x => x.ChangeFinalGroupRequestId == changeFinalGroupRequestId;
            var changeFinalGroup = await _changeFinalGroupRepository.GetById(cfgExpression);
            if (changeFinalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            changeFinalGroup.StatusOfTo = 2;
            await _changeFinalGroupRepository.UpdateAsync(changeFinalGroup);
            return new ApiSuccessResult<bool>(true);
        }
    }
}
