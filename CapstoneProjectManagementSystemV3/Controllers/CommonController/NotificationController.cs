using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities;
using Infrastructure.Services.CommonServices.DataRetrievalService;
using Infrastructure.Services.CommonServices.NotificationService;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneProjectManagementSystemV3.Controllers.CommonController
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseApiController
    {
        private readonly IDataRetrievalService _dataRetrievalService;
        private readonly INotificationService _notificationService;
        public NotificationController(IDataRetrievalService dataRetrievalService, INotificationService notificationService)
        {
            _dataRetrievalService = dataRetrievalService;
            _notificationService = notificationService;
        }
        [HttpGet("count-unread-notifications")]
        public async Task<IActionResult> CountNotificationNotReadOfUser()
        {
            var user =  _dataRetrievalService.GetData<User>("sessionAccount");
            var count = (await _notificationService.CountNotificationNotRead(user.UserId)).ResultObj;
            return Ok(new ApiSuccessResult<int>(count));
        }

        [HttpGet("count-all-notifications")]
        public async Task<IActionResult> CountAllNotification()
        {
            var user =  _dataRetrievalService.GetData<User>("sessionAccount");
            var count = (await _notificationService.CountNotificationNotRead(user.UserId)).ResultObj;
            return Ok(new ApiSuccessResult<int>(count));
        }

        [HttpGet("unread-notifications/{numberOfRecord}")]
        public async Task<IActionResult> GetListNotificationNotReadByReceiverID(int numberOfRecord)
        {
            var user =  _dataRetrievalService.GetData<User>("sessionAccount");
            var notifications =( await _notificationService.GetListNotificationNotReadByReceiverID(numberOfRecord, user.UserId)).ResultObj;
            return Ok(new ApiSuccessResult<IEnumerable<Notification>>(notifications));
        }

        [HttpGet("all-notifications/{numberOfRecord}")]
        public async Task<IActionResult> GetListAllNotificationByUserId(int numberOfRecord)
        {
            var user =  _dataRetrievalService.GetData<User>("sessionAccount");
            var notifications = (await _notificationService.GetListAllNotificationByUserId(numberOfRecord, user.UserId)).ResultObj;
            return Ok(new ApiSuccessResult<IEnumerable<Notification>>(notifications));
        }

        [HttpPut("mark-as-read/{notificationId}")]
        public async Task<IActionResult> ReadedNotification(int notificationId)
        {
            await _notificationService.UpdateReadedNotificationByNotificationId(notificationId);
            var attachedLink = (await _notificationService.GetAttachedLinkByNotificationId(notificationId)).ResultObj;
            return Ok(new ApiSuccessResult<string>(attachedLink));
        }

    }
}
