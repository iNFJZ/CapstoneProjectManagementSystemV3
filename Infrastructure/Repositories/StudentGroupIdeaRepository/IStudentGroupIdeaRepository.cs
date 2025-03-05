using Infrastructure.Entities;
using Infrastructure.Entities.Common;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentGroupIdeaRepository
{
    public interface IStudentGroupIdeaRepository : IRepositoryBase<StudentGroupIdea>
    {
        Task<List<int>> GetListStatusOfStudentInEachGroupByFptEmail(string fptEmail);
        Task<bool> DeleteRecord(string studentId, int groupIdeaId);
    }
}
