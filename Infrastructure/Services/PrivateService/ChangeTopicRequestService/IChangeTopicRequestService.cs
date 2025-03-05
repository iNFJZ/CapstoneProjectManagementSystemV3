using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ChangeTopicRequestService
{
    public interface IChangeTopicRequestService
    {
        Task<ApiResult<bool>> AddChangeTopicRequest(ChangeTopicRequest changeTopicRequest);
        Task<ApiResult<List<ChangeTopicRequest>>> GetChangeTopicRequestsByStudentId(string studentId, int semesterId);
        Task<ApiResult<ChangeTopicRequest>> GetDetailChangeTopicRequestsByRequestId(int requestId);
        Task<ApiResult<List<ChangeTopicRequest>>> GetChangeTopicRequestsBySearchText(string searchText, int status, int semesterId, int offsetNumber, int fetchNumber);
        Task<ApiResult<bool>> UpdateStatusOfChangeTopicRequest(int changeTopicRequestId, int status, string staffComment);
        Task<ApiResult<ChangeTopicRequest>> GetNewTopicByChangeTopicRequestId(int changeTopicRequestId);
        Task<ApiResult<bool>> DeleteChangeTopicRequestsByFinalGroup(int finalGropId);
        Task<ApiResult<int>> CountRecordChangeTopicRequestsBySearchText(string searchText, int status, int semesterId);

        Task<ApiResult<(int, int, List<ChangeTopicRequest>)>>GetChangeTopicRequestsBySupervisorEmail(string supervisorEmail, string searchText, int status, int semesterId, int offsetNumber, int fetchNumber, bool isDevHead, int[] professions, bool isConfirmationOfDevHeadNeeded, int[] statuses, string supervisorEmails);

        Task<ApiResult<bool>> UpdateChangeTopicRequestBySupervisor(bool isDevHead, bool isBeforeDeadline, bool isAccepted, int requestId, bool isConfirmationOfDevHeadNeeded, string userId, HttpRequest httpRequest);

        Task<ApiResult<bool>> CancelChangeTopicRequestBySupervisor(int requestId, int status, bool isBeforeDeadline, bool isDevHead, string userId, HttpRequest httpRequest);

        Task<ApiResult<bool>> checkContainStatus(int[] statuses, int status);
    }
}
