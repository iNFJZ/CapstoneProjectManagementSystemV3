using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.GroupIdeaDisplayFormService
{
    public interface IGroupIdeaDisplayFormService
    {
        Task<ApiResult<List<GroupIdeaDisplayForm>>> ConvertFromGroupIdeaList(List<GroupIdea> groupList);
    }
}
