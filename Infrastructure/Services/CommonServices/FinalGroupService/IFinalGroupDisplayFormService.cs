using Infrastructure.Entities.Common;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Entities.Common.ApiResult;

namespace Infrastructure.Services.CommonServices.FinalGroupService
{
    public interface IFinalGroupDisplayFormService
    {
        Task<ApiResult<List<FinalGroupDisplayForm>>> ConvertFromFinalList(List<FinalGroup> finalGroupList);
    }
}
