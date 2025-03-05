using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.GroupIdeaService
{
    public interface IGroupIdeaService
    {
        Task<ApiResult<List<GroupIdea>>> GetGroupIdeaSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber);
        Task<ApiResult<List<GroupIdea>>> GetGroupIdeaSearchList_2(int semester_Id, int profession_Id, int specialty_Id, string searchText, string studentId, int offsetNumber, int fetchNumber);
        Task<ApiResult<int>> getNumberOfResultWhenSearch(int semester_Id, int profession_Id, int specialty_Id, string searchText);
        Task<ApiResult<int>> getNumberOfResultWhenSearch_2(int semester_Id, int profession_Id, int specialty_Id, string searchText, string studentId);
        Task<ApiResult<GroupIdea>> GetGroupIdeaById(int id);
        Task<ApiResult<GroupIdea>> GetAllGroupIdeaById(int Id);
        List<string> ConvertProjectTags(string projectTags);
        Task<ApiResult<bool>> UpdateNumberOfMemberWhenAdd(int groupIdeaId);
        Task<ApiResult<bool>> UpdateNumberOfMemberWhenRemove(int groupIdeaId);
        Task<ApiResult<bool>> DeleteGroupIdea(int groupIdeaId);
        Task<ApiResult<bool>> UpdateIdea(GroupIdea groupIdea, int semesterId);

        Task<ApiResult<bool>> UpdateAllIdea(GroupIdea groupIdea, int semesterId);
        Task<ApiResult<List<GroupIdea>>> GetGroupIdeasByUserID(string UserID);

        //Task<ApiResult<List<GroupIdea>>> GetGroupIdeasByUserIDFilterByStatusandSearchText(string UserID, int filterStatus, string query);

        //Task<ApiResult<bool>> CreateNewGroupIdeaOfMentor(string Owner, int Profession, int Specialty, string ProjectEnglishName, string ProjetVietnameseName, string Abbreviation, string Description, string ProjectTags, int Semester, int NumberOfMember, int MaxMember);

        Task<ApiResult<bool>> UpdateStatusOfIdea(int ideaid, bool status);
    }
}
