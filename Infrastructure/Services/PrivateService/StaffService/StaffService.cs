using DocumentFormat.OpenXml.Spreadsheet;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.StaffRepository;
using Infrastructure.Repositories.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.StaffService
{
    public class StaffService : IStaffService
    {
        private readonly StaffRepository _staffRepository;
        private readonly UserRepository _userRepository;
        public StaffService(StaffRepository staffRepository,
            UserRepository userRepository)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
        }
        public async Task<ApiResult<StaffDto>> GetUserIsStaffByRoleId(int roleId)
        {
            Expression<Func<User, bool>> expression = x => x.RoleId == roleId; //Có thể config == 3 (auto staff)
            var user = await _userRepository.GetById(expression);
            var staff = new Staff()
            {
                StaffId = user.UserId,
                StaffNavigation = new User()
                {
                    FptEmail = user.FptEmail,
                }
            };
            var result = new StaffDto()
            {
                StaffID = staff.StaffId,
                User = new UserDto()
                {
                    FptEmail = staff.StaffNavigation.FptEmail
                }
            };
            return new ApiSuccessResult<StaffDto>(result);
        }

        public Task<ApiResult<List<StaffDto>>> GetUsersIsStaffByRoleId(int roleId)
        {
            throw new NotImplementedException();
        }
    }
}
