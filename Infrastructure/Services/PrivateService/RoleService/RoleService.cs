using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.RoleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.RoleService
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<ApiResult<List<RoleDto>>> GetRoles()
        {
            var roles = (await _roleRepository.GetAll()).ToList();
            var result = new List<RoleDto>();
            foreach (var role in roles)
            {
                result.Add(new RoleDto()
                {
                    Role_ID = role.RoleId,
                    RoleName = role.RoleName
                });
            }
            return new ApiSuccessResult<List<RoleDto>>(result);
        }
    }
}
