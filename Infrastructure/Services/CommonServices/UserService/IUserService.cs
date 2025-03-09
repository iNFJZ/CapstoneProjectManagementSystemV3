using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.AdminViewModel;
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

        Task<ApiResult<UserDto>> GetUserByID(string userId);

        Task<ApiResult<bool>> CheckRoleOfUser(string userId, string role);

        Task<ApiResult<bool>> CheckProfileUserHaveAttributeIsNullByUserId(string userId);

        Task<ApiResult<bool>> UpdateAvatar(string avatar, string userId);

        Task<ApiResult<List<string>>> GetListFptEmailByGroupIdeaId(int groupIdeaId);

        Task<ApiResult<string>> GetListUserIDByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<string>> GetNameStudentByUserId(string userId);
        Task<ApiResult<UserDto>> GetUserByFptEmail(string fptEmail, int roleLoginAs);
        Task<ApiResult<(int, int, List<UserWithRowNum>)>> GetListUserForAdminPaging(int pageNumber, string search, int role);
        Task<ApiResult<bool>> CreateStaffForAdmin(UserDto user);

        Task<ApiResult<List<UserDto>>> GetUserByRoleID(int roleId);

        Task<ApiResult<List<UserWithRowNum>>> GetListUserByRoleList(List<int> roleIds);

        Task<ApiResult<bool>> CreateSupervisorLeaderForAdmin(SupervisorDto supervisorleader);

        Task<ApiResult<bool>> checkDuplicateUser(string userID);

        Task<ApiResult<bool>> CheckReferenceDUserData(UserDto user);

        Task<ApiResult<bool>> DeleteUser(UserDto user);
    }
}
