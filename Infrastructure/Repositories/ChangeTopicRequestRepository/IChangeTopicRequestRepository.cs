using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ChangeTopicRequestRepository
{
    public interface IChangeTopicRequestRepository : IRepositoryBase<ChangeTopicRequest>
    {
        Task<ChangeTopicRequest> GetDetailChangeTopicRequestsByRequestId(int requestId);
        Task<bool> UpdateStatusOfChangeTopicRequest(int changeTopicRequestId, int status, string staffComment);
        Task<List<ChangeTopicRequest>> GetChangeTopicRequestsBySupervisorEmail(string supervisorEmail, string searchText, int[] statuses, int semesterId, int offsetNumber, int fetchNumber);
        Task<List<ChangeTopicRequest>> GetChangeTopicRequestsByProfessionId(int[] professions, string searchText, int[] statuses, int semesterId, int offsetNumber, int fetchNumber, string supervisorEmails);
    }
};
