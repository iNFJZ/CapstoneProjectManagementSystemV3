using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorProfessionService
{
    public interface ISupervisorProfessionService
    {
        Task<ApiResult<List<Profession>>> GetProfessionsBySupervisorID(string supervisorId, int semesterId);
    }
}
