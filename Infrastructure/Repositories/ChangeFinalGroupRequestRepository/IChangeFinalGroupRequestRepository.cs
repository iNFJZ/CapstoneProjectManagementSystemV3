using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ChangeFinalGroupRequestRepository
{
    public interface IChangeFinalGroupRequestRepository : IRepositoryBase<ChangeFinalGroupRequest>
    {
        Task<int> CountRecordChangeFinalGroupBySearchText(string searchText, int status, int semesterId);
        Task<List<ChangeFinalGroupRequest>> GetListChangeFinalGroupRequestBySearchText(string searchText, int status, int semesterId, int offsetNumber, int fetchNumber);
    }
}
