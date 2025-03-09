using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ConfigurationService
{
    public interface IConfigurationService
    {
        Task<ApiResult<bool>> Insert(int Specialty, List<WithDto> Withs, int Profession);
        Task<ApiResult<List<WithDto>>> GetWithsBySpecialtyID(int specialtyID);

        Task<ApiResult<List<WithDto>>> GetWithProfessionBySpecialityId(int specialityId);
    }
}
