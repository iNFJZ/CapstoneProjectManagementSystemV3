using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;
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
        Task<ApiResult<List<GroupIdeaOfSupervisorDto>>> GetGroupIdeasBySupervisorIDFilterByStatusandSearchText(string SupervisorID, int filterStatus, string query);
        Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorWithRowNum>)>> GetListIdeaOfSupervisorForPaging(int pageNumber, string SupervisorID, int filterStatus, string query);
        Task<ApiResult<bool>> CreateNewGroupIdeaOfMentor(string Supervisor, List<GroupIdeaOfSupervisorProfessionDto> groupIdeaOfSupervisorProfessions, string ProjectEnglishName, string ProjetVietnameseName, string Abbreviation, string Description, string ProjectTags, int Semester, int NumberOfMember, int MaxMember);
        Task<ApiResult<List<GroupIdeaOfSupervisorProfessionDto>>> GetGroupIdeasBySupervisorID(string SupervisorID);
        Task<ApiResult<GroupIdeaOfSupervisorDto>> GetAllGroupIdeaById(int Id);
        Task<ApiResult<bool>> UpdateAllIdea(GroupIdeaOfSupervisorDto groupIdea, int semesterId);
        Task<ApiResult<bool>> DeleteGroupIdea(int groupIdeaId);
        Task<ApiResult<bool>> UpdateStatusOfIdea(int ideaid, bool status);
        Task<ApiResult<List<GroupIdeaOfSupervisorDto>>> GetGroupIdeaOfSupervisorsBySupervisorAndProfession(string supervisorID, int[] pros);
        Task<ApiResult<List<GroupIdeaOfSupervisorDto>>> GetGroupIdeaRegistedOfSupervisor(string supervisorID);
        Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorDto>)>> getGroupIdeaOfSupervisorWithPaging(int pageNumber, string supervisorId);
        Task<ApiResult<GroupIdeaOfSupervisorDto>> GetGroupIdeaOfSupervisorByGroupIdeaId(int groupIdeaId);
        Task<ApiResult<List<GroupIdeaOfSupervisorProfessionDto>>> getAllProfessionSpecialyByGroupIdeaID(int groupIdeaID);
        Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorDto>)>> getGroupIdeaOfSupervisorWithPagingForStudent(int pageNumber, int professionID);
    }
}