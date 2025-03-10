using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.NotificationRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.NotificationService
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly RealTimeHub _realtimeHub;
        public NotificationService(INotificationRepository notificationRepository,
            RealTimeHub realtimeHub)
        {
            _notificationRepository = notificationRepository;
            _realtimeHub = realtimeHub;
        }

        public async Task<ApiResult<int>> CountAllNotification(string userId)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>();
            expressions.Add(x => x.UserId == userId);
            expressions.Add(x => x.DeletedAt == null);
            var result = await _notificationRepository.GetByConditions(expressions);
            return new ApiSuccessResult<int>(result.Count);
        }

        public async Task<ApiResult<int>> CountNotificationNotRead(string userId)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>();
            expressions.Add(x => x.UserId == userId);
            expressions.Add(x => x.DeletedAt == null);
            expressions.Add(x => x.Readed == false);
            var result = await _notificationRepository.GetByConditions(expressions);
            return new ApiSuccessResult<int>(result.Count);
        }

        public async Task<ApiResult<string>> GetAttachedLinkByNotificationId(int notificationId)
        {
            Expression<Func<Notification, bool>> expression = x => x.NotificationId == notificationId;
            var result = await _notificationRepository.GetById(expression);
            if (result == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy liên kết");
            }
#pragma warning disable CS8604 // Possible null reference argument.
            return new ApiSuccessResult<string>(result.AttachedLink);
#pragma warning restore CS8604 // Possible null reference argument.
        }

        public async Task<ApiResult<List<NotificationDto>>> GetListAllNotificationByUserId(int numberOfRecord, string userId)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>();
            expressions.Add(x => x.UserId == userId);
            expressions.Add(x => x.DeletedAt == null);
            var notificationList = await _notificationRepository.GetByConditions(expressions);
            var result = new List<NotificationDto>();
            foreach (var notification in notificationList)
            {
                result.Add(new NotificationDto
                {
                    NotificationID = notification.NotificationId,
                    Readed = notification.Readed,
                    NotificationContent = notification.NotificationContent,
                    AttachedLink = notification.AttachedLink,
                    CreatedAt = notification.CreatedAt
                });
            }
            return new ApiSuccessResult<List<NotificationDto>>(result);
        }

        public async Task<ApiResult<List<NotificationDto>>> GetListNotificationNotReadByReceiverID(int numberOfRecord, string userId)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>();
            expressions.Add(x => x.UserId == userId);
            expressions.Add(x => x.DeletedAt == null);
            expressions.Add(x => x.Readed == false);
            var unReadNotificationList = await _notificationRepository.GetByConditions(expressions);
            unReadNotificationList.Take(numberOfRecord);
            unReadNotificationList.OrderBy(x => x.CreatedAt);
            var result = new List<NotificationDto>();
            foreach (var notification in unReadNotificationList)
            {
                result.Add(new NotificationDto()
                {
                    NotificationID = notification.NotificationId,
                    Readed = notification.Readed,
                    NotificationContent = notification.NotificationContent,
                    AttachedLink = notification.AttachedLink,
                    CreatedAt = notification.CreatedAt
                });
            }
            return new ApiSuccessResult<List<NotificationDto>>(result);
        }

        public async Task<ApiResult<bool>> InsertDataNotification(string userId, string notificationContent, string attachedLink)
        {
            var newNotification = new Notification()
            {
                UserId = userId,
                NotificationContent = notificationContent,
                AttachedLink = attachedLink
            };
            await _notificationRepository.CreateAsync(newNotification);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            _realtimeHub.SendMessage(userId, attachedLink);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateReadedNotificationByNotificationId(int notificationId)
        {
            Expression<Func<Notification, bool>> expression = x => x.NotificationId == notificationId;
            var result = await _notificationRepository.GetById(expression);
            if (result == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy thông báo");
            }
            result.Readed = true;
            await _notificationRepository.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);

        }
    }
}
