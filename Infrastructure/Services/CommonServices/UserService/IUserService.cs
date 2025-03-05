using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.UserDto;
using Infrastructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.UserService
{
     public interface IUserService
    {
        Task<ApiResult<bool>> AddUser(User user, int roleId, int professionId, int specId);

        Task<ApiResult<User>> GetUserByID(string userId);

        Task<ApiResult<bool>> CheckRoleOfUser(string userId, string role);

        Task<ApiResult<bool>> CheckProfileUserHaveAttributeIsNullByUserId(string userId);

        Task<ApiResult<bool>> UpdateAvatar(string avatar, string userId);

        Task<ApiResult<List<string>>> GetListFptEmailByGroupIdeaId(int groupIdeaId);

        Task<ApiResult<string>> GetListUserIDByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<string>> GetNameStudentByUserId(string userId);
        Task<ApiResult<User>> GetUserByFptEmail(string fptEmail, int roleLoginAs);
        Task<ApiResult<(int, int, List<UserWithRowNum>)>> GetListUserForAdminPaging(int pageNumber, string search, int role);
        Task<ApiResult<bool>> CreateStaffForAdmin(User user);

        Task<ApiResult<List<User>>> GetUserByRoleID(int roleId);

        Task<ApiResult<List<UserWithRowNum>>> GetListUserByRoleList(List<int> roleIds);

        Task<ApiResult<bool>> CreateSupervisorLeaderForAdmin(Supervisor supervisorleader);

        Task<ApiResult<bool>> checkDuplicateUser(string userID);

        Task<ApiResult<bool>> CheckReferenceDUserData(User user);

        Task<ApiResult<bool>> DeleteUser(User user);
    }
}
