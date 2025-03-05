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
            if(result == null)
            {
                return new ApiErrorResult<string>("Không tìm thấy liên kết");
            }
            return new ApiSuccessResult<string>(result.AttachedLink);
        }

        public async Task<ApiResult<List<Notification>>> GetListAllNotificationByUserId(int numberOfRecord, string userId)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>();
            expressions.Add(x => x.UserId == userId);
            expressions.Add(x => x.DeletedAt == null);
            var result = await _notificationRepository.GetByConditions(expressions);
            return new ApiSuccessResult<List<Notification>>(result);
        }

        public async Task<ApiResult<List<Notification>>> GetListNotificationNotReadByReceiverID(int numberOfRecord, string userId)
        {
            List<Expression<Func<Notification, bool>>> expressions = new List<Expression<Func<Notification, bool>>>();
            expressions.Add(x => x.UserId == userId);
            expressions.Add(x => x.DeletedAt == null);
            expressions.Add(x => x.Readed == false);
            var result = await _notificationRepository.GetByConditions(expressions);
            result.Take(numberOfRecord);
            result.OrderBy(x => x.CreatedAt);
            return new ApiSuccessResult<List<Notification>>(result);
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
            _realtimeHub.SendMessage(userId, attachedLink);
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
