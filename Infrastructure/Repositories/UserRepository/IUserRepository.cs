using Infrastructure.Entities;
using Infrastructure.Entities.Dto.ViewModel.AdminViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UserRepository
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<bool> AddUser(User user, int roleId, int professionId, int specialtyId);
        Task<bool> CheckProfileUserHaveAttributeIsNullByUserId(string userId);
        Task<bool> CheckReferenceDUserData(UserDto user);
        Task<bool> CheckUserByUserIdAndRoleExist(string userId, string role);
        Task<List<string>> GetListFptEmailByGroupIdeaId(int groupIdeaId);
        Task<List<UserWithRowNum>> GetListUserByRoleList(List<int> roleIds);
        Task<List<UserWithRowNum>> GetListUserForAdminPaging(int pageNumber, string search, int role);
        Task<string> GetListUserIDByGroupIdeaId(int groupIdeaId);
        Task<string> GetStudentNameByUserId(string userId);
        Task<User> GetUserByFptEmailAsync(string fptEmail, int roleLoginAs);
    }
}
