using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.StaffService
{
    public interface IStaffService
    {
        Task<ApiResult<Staff>> GetUserIsStaffByRoleId(int roleId);

        Task<ApiResult<List<Staff>>> GetUsersIsStaffByRoleId(int roleId);
    }
}
