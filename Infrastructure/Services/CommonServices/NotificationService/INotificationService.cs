using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.NotificationService
{
    public interface INotificationService
    {
        Task<ApiResult<int>> CountNotificationNotRead(string userId);
        Task<ApiResult<int>> CountAllNotification(string userId);
        Task<ApiResult<List<NotificationDto>>> GetListAllNotificationByUserId(int numberOfRecord, string userId);
        Task<ApiResult<List<NotificationDto>>> GetListNotificationNotReadByReceiverID(int numberOfRecord, string userId);
        Task<ApiResult<bool>> InsertDataNotification(string userId, string notificationContent, string attachedLink);

        Task<ApiResult<string>> GetAttachedLinkByNotificationId(int notificationId);
        Task<ApiResult<bool>> UpdateReadedNotificationByNotificationId(int notificationId);
    }
}
