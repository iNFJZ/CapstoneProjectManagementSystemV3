using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.AdminViewModel;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Repositories.UserRepository;
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
            var result = await _userRepository.AddUser(user, roleId, professionId, specId);
            if (result == true)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }
        public async Task<ApiResult<bool>> checkDuplicateUser(string userID)
        {
            var findUser = await _userRepository.GetById(x => x.UserId == userID);
            if (findUser == null)
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

        public async Task<ApiResult<bool>> CheckReferenceDUserData(UserDto user)
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

        public async Task<ApiResult<bool>> CreateStaffForAdmin(UserCreateRequest user)
        {
            try
            {
                var newUser = new User
                {
                    UserId = user.UserID,
                    UserName = user.UserName,
                    FptEmail = user.FptEmail,
                    FullName = user.FullName,
                    Gender = user.Gender,
                    RoleId = user.RoleID
                };
                await _userRepository.CreateAsync(newUser);
                var newStaff = new Staff
                {
                    StaffId = user.UserID
                };
                await _staffRepository.CreateAsync(newStaff);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.ToString());
            }
        }

        public async Task<ApiResult<bool>> CreateSupervisorLeaderForAdmin(SupervisorDto supervisorleader)
        {
            try
            {
                var newUser = new User
                {
                    UserId = supervisorleader.SupervisorID,
                    UserName = supervisorleader.SupervisorNavigation.UserName,
                    FptEmail = supervisorleader.SupervisorNavigation.FptEmail.Trim().ToLower(),
                    FullName = supervisorleader.SupervisorNavigation.FullName.Trim(),
                    Gender = supervisorleader.SupervisorNavigation.Gender,
                    RoleId = 4
                };
                await _userRepository.CreateAsync(newUser);
                var newSupervisor = new Supervisor
                {
                    SupervisorId = supervisorleader.SupervisorID,
                    IsActive = supervisorleader.IsActive,
                    FieldSetting = supervisorleader.FieldSetting,
                    FeEduEmail = supervisorleader.FeEduEmail
                };
                await _supervisorRepository.CreateAsync(newSupervisor);
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.ToString());
            }
        }

        public async Task<ApiResult<bool>> DeleteUser(UserDto user) // chức năng chỉ dành cho staff và supervisor
        {
            try
            {
              
                var findUser = await _userRepository.GetById(x => x.UserId == user.UserID);
                if (findUser != null)
                {
                    findUser.DeletedAt = DateTime.Now;
                    await _userRepository.UpdateAsync(findUser);
                    if (user.RoleID == 3)
                    {
                        var findStaff = await _staffRepository.GetById(x => x.StaffId == user.UserID);
                        findStaff.DeletedAt = DateTime.Now;
                        await _staffRepository.UpdateAsync(findStaff);
                    }
                    else
                    {
                        var findSupervisor = await _supervisorRepository.GetById(x => x.SupervisorId == user.UserID);
                        findSupervisor.DeletedAt = DateTime.Now;
                        await _supervisorRepository.UpdateAsync(findSupervisor);
                    }
                    return new ApiSuccessResult<bool>(true);
                }
                return new ApiSuccessResult<bool>(false);
            }
            catch (Exception ex)
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
                        Role = new Role()
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

        public async Task<ApiResult<UserDto>> GetUserByFptEmail(string fptEmail, int roleLoginAs)
        {
            var user = await _userRepository.GetUserByFptEmailAsync(fptEmail, roleLoginAs);
            var result = new UserDto()
            {
                UserID = user.UserId,
                UserName = user.UserName,
                FptEmail = user.FptEmail,
                Avatar = user.Avatar,
                FullName = user.FullName,
                Role = new RoleDto()
                {
                    Role_ID = user.RoleId.Value
                }
            };
#pragma warning restore CS8629 // Nullable value type may be null.
            return new ApiSuccessResult<UserDto>(result);
        }

        public async Task<ApiResult<UserDto>> GetUserByID(string userId)
        {
            var findUser = await _userRepository.GetById(e => e.UserId == userId && e.DeletedAt == null);
            if (findUser == null)
            {
                return new ApiErrorResult<UserDto>("Không tìm thấy đối tượng");
            }
            else
            {
                var result = new UserDto()
                {
                    UserID = findUser.UserId,
                    UserName = findUser.UserName,
                    FptEmail = findUser.FptEmail,
                    Avatar = findUser.Avatar,
                    FullName = findUser.FullName,
                    RoleID = findUser.RoleId.Value,
                    Role = new RoleDto()
                    {
                        Role_ID = findUser.RoleId.Value
                    }
                };
                return new ApiSuccessResult<UserDto>(result);
            }
        }

        public async Task<ApiResult<List<UserDto>>> GetUserByRoleID(int roleId)
        {
            var findUser = await _userRepository.GetByCondition(e => e.RoleId == roleId && e.DeletedAt == null);
            if (findUser == null)
            {
                return new ApiErrorResult<List<UserDto>>("Không tìm thấy đối tượng");
            }
            else
            {
                var result = new List<UserDto>();
                foreach (var item in findUser)
                {
                    result.Add(new UserDto()
                    {
                        UserID = item.UserId,
                        UserName = item.UserName,
                        FptEmail = item.FptEmail,
                        Avatar = item.Avatar,
                        FullName = item.FullName,
                        Role = new RoleDto()
                        {
                            Role_ID = item.RoleId.Value
                        }
                    });
                }
                return new ApiSuccessResult<List<UserDto>>(result);
            }
        }

        public async Task<ApiResult<bool>> UpdateAvatar(string avatar, string userId)
        {
            try
            {
                var findUser = await _userRepository.GetById(x => x.UserId == userId);
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
