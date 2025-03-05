using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.Student_FavoriteGroupIdeaService
{
    public interface IStudentFavoriteGroupIdeaService
    {
        Task<ApiResult<List<StudentFavoriteGroupIdea>>> GetFavoriteIdeaListByStudentId(string studentId);
        Task<ApiResult<StudentFavoriteGroupIdea>> GetRecord(string studentId, int groupId);
        Task<ApiResult<bool>> AddRecord(string studentId, int groupId);
        Task<ApiResult<bool>> DeleteRecord(string studentId, int groupIdeaId);
        Task<ApiResult<bool>> DeleteAllRecordOfAGroupIdea(int groupIdeaId);
    }
}
