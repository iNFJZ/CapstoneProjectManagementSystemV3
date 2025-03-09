using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SemesterRepository
{
    public interface ISemesterRepository : IRepositoryBase<Semester>
    {
        Task<SemesterDto> GetCurrentSemester();
        Task<SemesterDto> GetSemesterById(int semesterId);
        Task<List<SemesterDto>> GetAllSemester();
        Task<bool> CloseSemesterCurrent();
    }
}
