using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.SemesterService
{
    public interface ISemesterService
    {
        Task<ApiResult<SemesterDto>> GetCurrentSemester();
        Task<ApiResult<SemesterDto>> GetSemesterById(int semesterId);
        Task<ApiResult<List<SemesterDto>>> GetAllSemester();
        Task<ApiResult<SemesterDto>> GetLastSemester();
        Task<ApiResult<bool>> UpdateCurrentSemester(Semester semester);
        Task<ApiResult<bool>> AddNewSemester(Semester semester);
        Task<ApiResult<bool>> CloseCurrentSemester();
        Task<ApiResult<bool>> ChangeShowGroupNameStatus(int semesterId, int status);

        Task<ApiResult<string>> generateGroupInformationMailContent(string groupName, List<StudentGroupIdea> students, string projectEnglishName, string projectVietnamese, string abbreviation, Supervisor supervisor);

        Task<ApiResult<SemesterDto>> GetLastSemesterDeleteAt();
    }
}
