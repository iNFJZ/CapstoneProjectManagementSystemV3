using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ChangeFinalGroupRequestService
{
    public interface IChangeFinalGroupRequestService
    {
        Task<ApiResult<bool>> CreateChangeFinalGroupRequestDao(string fromStudentId, string toStudentId);
        Task<ApiResult<List<ChangeFinalGroupRequestDto>>> GetListChangeFinalGroupRequestFromOfStudent(string fromStudentId, int semesterId);
        Task<ApiResult<List<ChangeFinalGroupRequestDto>>> GetListChangeFinalGroupRequestToOfStudent(string toStudentId, int semesterId);
        Task<ApiResult<List<ChangeFinalGroupRequestDto>>> GetListChangeFinalGroupRequest(string studentId, int semesterId);
        Task<ApiResult<bool>> UpdateStatusAcceptOfToStudentByChangeFinalGroupRequestId(int changeFinalGroupRequestId);
        Task<ApiResult<bool>> UpdateStatusRejectOfToStudentByChangeFinalGroupRequestId(int changeFinalGroupRequestId);
        Task<ApiResult<string>> GetFromStudentIdByChangeFinalGroupRequestIdAndToStudentId(int changeFinalGroupRequestId, string toStudentId);
        Task<ApiResult<List<ChangeFinalGroupRequestDto>>> GetListChangeFinalGroupRequestBySearchText
                (string searchText, int status, int semesterId, int offsetNumber, int fetchNumber);
        Task<ApiResult<int>> CountRecordChangeFinalGroupBySearchText(string searchText, int status, int semesterId);
        Task<ApiResult<ChangeFinalGroupRequestDto>> GetInforOfStudentExchangeFinalGroup(int changeFinalGroupRequestId);
        Task<ApiResult<bool>> UpdateGroupForStudentByChangeFinalGroupRequest(ChangeFinalGroupRequest changeFinalGroupRequest);
        Task<ApiResult<bool>> UpdateStatusOfStaffByChangeFinalGroupRequestId(int changeFinalGroupRequestId, string staffComment);
    }
}
