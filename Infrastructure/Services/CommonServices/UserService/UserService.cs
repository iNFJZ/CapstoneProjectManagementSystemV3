using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.UserDto;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.UserRepository;
using Infrastructure.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStaffRepository _staffRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        public UserService(IUserRepository userRepository, 
            IStaffRepository staffRepository, 
            ISupervisorRepository supervisorRepository)
        {
            _userRepository = userRepository;
            _staffRepository = staffRepository;
            _supervisorRepository = supervisorRepository;
        }

        public async Task<ApiResult<bool>> AddUser(User user, int roleId, int professionId, int specId)
        {
            var result =await _userRepository.AddUser(user,roleId,professionId,specId);
            if(result == true)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }
        public async Task<ApiResult<bool>> checkDuplicateUser(string userID)
        {
            Expression<Func<User, bool>> expression = x => x.UserId == userID;
            var findUser = await _userRepository.GetById(expression);
            if(findUser == null)
            {
                return new ApiSuccessResult<bool>(false);
            }
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CheckProfileUserHaveAttributeIsNullByUserId(string userId)
        {
            var result = await _userRepository.CheckProfileUserHaveAttributeIsNullByUserId(userId);
            if (result == true)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<bool>> CheckReferenceDUserData(User user)
        {
            var result = await _userRepository.CheckReferenceDUserData(user);
            if (result == true)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<bool>> CheckRoleOfUser(string userId, string role)
        {

            var result = await _userRepository.CheckUserByUserIdAndRoleExist(userId, role);
            if (result == true)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<bool>> CreateStaffForAdmin(User user)
        {
            try
            {
                var newUser = new User
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    FptEmail = user.FptEmail,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    RoleId = user.RoleId
                };
                await _userRepository.CreateAsync(newUser);
                var newStaff = new Staff
                {
                    StaffId = user.UserId
                };
                await _staffRepository.CreateAsync(newStaff);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.ToString());
            }
        }

        public async Task<ApiResult<bool>> CreateSupervisorLeaderForAdmin(Supervisor supervisorleader)
        {
            try
            {
                var newUser = new User
                {
                    UserId = supervisorleader.SupervisorId,
                    UserName = supervisorleader.SupervisorNavigation.UserName,
                    FptEmail = supervisorleader.SupervisorNavigation.FptEmail.Trim().ToLower(),
                    FullName = supervisorleader.SupervisorNavigation.FullName.Trim(),
                    Gender = supervisorleader.SupervisorNavigation.Gender,
                    RoleId = 4
                };
                await _userRepository.CreateAsync(newUser);
                var newSupervisor = new Supervisor
                {
                    SupervisorId = supervisorleader.SupervisorId,
                    IsActive = supervisorleader.IsActive,
                    FieldSetting = supervisorleader.FieldSetting,
                    FeEduEmail = supervisorleader.FeEduEmail
                };
                await _supervisorRepository.CreateAsync(newSupervisor);
                return new ApiSuccessResult<bool>(true);
            }catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.ToString());
            }
        }

        public async Task<ApiResult<bool>> DeleteUser(User user) // chức năng chỉ dành cho staff và supervisor
        {
            try{
                Expression<Func<User, bool>> expression = x => x.UserId == user.UserId;
                var findUser = await _userRepository.GetById(expression);
                if (findUser != null)
                {
                    findUser.DeletedAt = DateTime.Now;
                    await _userRepository.UpdateAsync(findUser);
                    if (user.RoleId == 3)
                    {
                        Expression<Func<Staff, bool>> expressionStaff = x => x.StaffId == user.UserId;
                        var findStaff = await _staffRepository.GetById(expressionStaff);
                        findStaff.DeletedAt = DateTime.Now;
                        await _staffRepository.UpdateAsync(findStaff);
                    }
                    else
                    {
                        Expression<Func<Supervisor, bool>> expressionSupervisor = x => x.SupervisorId == user.UserId;
                        var findSupervisor = await _supervisorRepository.GetById(expressionSupervisor);
                        findSupervisor.DeletedAt = DateTime.Now;
                        await _supervisorRepository.UpdateAsync(findSupervisor);
                    }
                    return new ApiSuccessResult<bool>(true);
                }
                return new ApiSuccessResult<bool>(false);
            }catch(Exception ex)
            {
                return new ApiErrorResult<bool>(ex.ToString());
            }
        }

        public async Task<ApiResult<List<string>>> GetListFptEmailByGroupIdeaId(int groupIdeaId)
        {
            var result = await _userRepository.GetListFptEmailByGroupIdeaId(groupIdeaId);
            return new ApiSuccessResult<List<string>>(result);
        }

        public async Task<ApiResult<List<UserWithRowNum>>> GetListUserByRoleList(List<int> roleIds)
        {
            var result = await _userRepository.GetListUserByRoleList(roleIds);
            return new ApiSuccessResult<List<UserWithRowNum>>(result);
        }

        public async Task<ApiResult<(int, int, List<UserWithRowNum>)>> GetListUserForAdminPaging(int pageNumber, string search, int role)
        {
            try
            {
                // Query đếm tổng số bản ghi phù hợp
                var query = await _userRepository.GetByCondition(u => u.DeletedAt == null &&
                                (string.IsNullOrEmpty(search) || u.FptEmail.Contains(search) || u.FullName.Contains(search)) &&
                                (role == 0 || u.RoleId == role));

                int totalRecord = query.Count(); // Đếm số bản ghi

                // Gọi hàm tính toán phân trang
                int[] pagingQueryResult = pagingQuery(totalRecord, pageNumber);
                int recordSkippedBefore = pagingQueryResult[2];
                int recordSkippedLater = pagingQueryResult[3];

                // Query lấy danh sách user theo trang
                var users = query.OrderBy(u => u.UserId)
                    .Skip(recordSkippedBefore)
                    .Take(recordSkippedLater - recordSkippedBefore)
                    .Select(u => new UserWithRowNum
                    {
                        RowNum = 0, // Sẽ tính toán sau
                        UserID = u.UserId,
                        FptEmail = u.FptEmail,
                        FullName = u.FullName ?? "",
                        Created_At = u.CreatedAt,
                        Role = new Role ()
                        {
                            RoleId = u.RoleId.Value,
                            RoleName = u.Role.RoleName == "DevHead" ? "Department Leader" : u.Role.RoleName
                        }
                    }).ToList();

                // Gán lại RowNum theo thứ tự từ 1 trở đi
                int index = recordSkippedBefore + 1;
                users.ForEach(u => u.RowNum = index++);

                return new ApiSuccessResult<(int, int, List<UserWithRowNum>)>((pagingQueryResult[0], pagingQueryResult[1], users));
            }
            catch (Exception ex)
            {
                return new ApiSuccessResult<(int, int, List<UserWithRowNum>)>((0, 0, new List<UserWithRowNum>()));
            }
        }
        private int[] pagingQuery(int totalRecord, int pageNumber, int pageSize = 10)
        {
            if (totalRecord == 0) return new int[] { 0, 0, 0, 0 };

            int totalPages = (int)Math.Ceiling((double)totalRecord / pageSize); // Tổng số trang
            pageNumber = Math.Max(1, Math.Min(pageNumber, totalPages)); // Đảm bảo pageNumber hợp lệ

            int recordSkippedBefore = (pageNumber - 1) * pageSize; // Bản ghi bị bỏ qua
            int recordSkippedLater = Math.Min(recordSkippedBefore + pageSize, totalRecord); // Giới hạn bản ghi tối đa

            return new int[] { totalPages, totalRecord, recordSkippedBefore, recordSkippedLater };
        }


        public async Task<ApiResult<string>> GetListUserIDByGroupIdeaId(int groupIdeaId)
        {
            var result = await _userRepository.GetListUserIDByGroupIdeaId(groupIdeaId);
            return new ApiSuccessResult<string>(result);
        }

        public async Task<ApiResult<string>> GetNameStudentByUserId(string userId)
        {
            var result = await _userRepository.GetStudentNameByUserId(userId);
            return new ApiSuccessResult<string>(result);
        }

        public async Task<ApiResult<User>> GetUserByFptEmail(string fptEmail, int roleLoginAs)
        {
            var result = await _userRepository.GetUserByFptEmailAsync(fptEmail,roleLoginAs);
            return new ApiSuccessResult<User>(result);
        }

        public async Task<ApiResult<User>> GetUserByID(string userId)
        {
            List<Expression<Func<User, bool>>> expressions = new List<Expression<Func<User, bool>>>();
            expressions.Add(e => e.UserId == userId);
            expressions.Add(e => e.DeletedAt == null);
            var findUser = await _userRepository.GetByConditionId(expressions);
           
            if (findUser == null)
            {
                return new ApiErrorResult<User>("Không tìm thấy đối tượng");
            }
            return new ApiSuccessResult<User>(findUser);
        }

        public async Task<ApiResult<List<User>>> GetUserByRoleID(int roleId)
        {
            List<Expression<Func<User, bool>>> expressions = new List<Expression<Func<User, bool>>>();
            expressions.Add(e => e.RoleId == roleId);
            expressions.Add(e => e.DeletedAt == null);
            var findUser = await _userRepository.GetByConditions(expressions);
           
            if (findUser == null)
            {
                return new ApiErrorResult<List<User>>("Không tìm thấy đối tượng");
            }
            return new ApiSuccessResult<List<User>>(findUser);
        }

        public async Task<ApiResult<bool>> UpdateAvatar(string avatar, string userId)
        {
            try{
                Expression<Func<User, bool>> expression = x => x.UserId == userId;
                var findUser = await _userRepository.GetById(expression);
                if (findUser == null)
                {
                    return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
                }
                findUser.Avatar = avatar;
                await _userRepository.UpdateAsync(findUser);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception)
            {
                return new ApiSuccessResult<bool>(false);
            }
            
        }
    }
}
