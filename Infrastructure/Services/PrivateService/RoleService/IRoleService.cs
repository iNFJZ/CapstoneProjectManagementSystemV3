using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.RoleService
{
    public interface IRoleService
    {
        Task<ApiResult<List<RoleDto>>> GetRoles();
    }
}
