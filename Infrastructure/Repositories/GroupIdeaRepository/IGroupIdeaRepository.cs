using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.GroupIdeaRepository
{
    public interface IGroupIdeaRepository : IRepositoryBase<GroupIdea>
    {
        Task<List<GroupIdea>> GetGroupIdeaSearchList(string semester_Id, string profession_Id, string specialty_Id, string searchText, int offsetNumber, int fetchNumber);
        Task<List<GroupIdea>> GetGroupIdeaSearchList_2(string semesterId, string professionId, string specialtyId, string searchText, string studentId, int offsetNumber, int fetchNumber);
        Task<int> GetNumberOfResultWhenSearchAsync(string semester_Id, string profession_Id, string specialty_Id, string searchText);
        Task<int> GetNumberOfResultWhenSearch2(string semester_Id, string profession_Id, string specialty_Id, string searchText, string studentId);
        Task<GroupIdea> GetGroupIdeaByIdAsync(int id);
        Task<List<GroupIdea>> GetGroupIdeasByUserIDAsync(string userID);
    }
}
