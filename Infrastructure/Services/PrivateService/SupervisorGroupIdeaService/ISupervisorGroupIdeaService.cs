using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories;
using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorGroupIdeaService
{
    public interface ISupervisorGroupIdeaService 
    {
        Task<ApiResult<List<GroupIdeasOfSupervisor>>> GetGroupIdeasBySupervisorIDFilterByStatusandSearchText(string SupervisorID, int filterStatus, string query);
        Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorWithRowNum>)>> GetListIdeaOfSupervisorForPaging(int pageNumber, string SupervisorID, int filterStatus, string query);
        Task<ApiResult<bool>> CreateNewGroupIdeaOfMentor(string Supervisor, List<GroupIdeaOfSupervisorProfession> groupIdeaOfSupervisorProfessions, string ProjectEnglishName, string ProjetVietnameseName, string Abbreviation, string Description, string ProjectTags, int Semester, int NumberOfMember, int MaxMember);
        Task<ApiResult<List<GroupIdeasOfSupervisor>>> GetGroupIdeasBySupervisorID(string SupervisorID);
        Task<ApiResult<GroupIdeasOfSupervisor>> GetAllGroupIdeaById(int Id);
        Task<ApiResult<bool>> UpdateAllIdea(GroupIdeasOfSupervisor groupIdea, int semesterId);
        Task<ApiResult<bool>> DeleteGroupIdea(int groupIdeaId);
        Task<ApiResult<bool>> UpdateStatusOfIdea(int ideaid, bool status);
        Task<ApiResult<List<GroupIdeasOfSupervisor>>> GetGroupIdeaOfSupervisorsBySupervisorAndProfession(string supervisorID, int[] pros);
        Task<ApiResult<List<GroupIdeasOfSupervisor>>> GetGroupIdeaRegistedOfSupervisor(string supervisorID);
        Task<ApiResult<(int, int, List<GroupIdeasOfSupervisor>)>> getGroupIdeaOfSupervisorWithPaging(int pageNumber, string supervisorId);
        Task<ApiResult<GroupIdeasOfSupervisor>> GetGroupIdeaOfSupervisorByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<List<GroupIdeaOfSupervisorProfession>>> getAllProfessionSpecialyByGroupIdeaID(int groupIdeaID);
        Task<ApiResult<(int, int, List<GroupIdeasOfSupervisor>)>> getGroupIdeaOfSupervisorWithPagingForStudent(int pageNumber, int professionID);
    }
}
