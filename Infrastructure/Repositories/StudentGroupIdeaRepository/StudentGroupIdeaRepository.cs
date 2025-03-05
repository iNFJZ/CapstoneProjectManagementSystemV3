using DocumentFormat.OpenXml.InkML;
using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentGroupIdeaRepository
{
    public class StudentGroupIdeaRepository : RepositoryBase<StudentGroupIdea>, IStudentGroupIdeaRepository
    {
        private readonly DBContext _dbContext;
        public StudentGroupIdeaRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<int>> GetListStatusOfStudentInEachGroupByFptEmail(string fptEmail)
        {
            var statusList = await _dbContext.StudentGroupIdeas
            .Where(sg => sg.Student.StudentNavigation.FptEmail == fptEmail
                      && sg.DeletedAt == null
                      && sg.Student.DeletedAt == null
                      && sg.Student.StudentNavigation.DeletedAt == null)
            .Select(sg => sg.Status ?? 0)
            .ToListAsync();
            return statusList;
        }
        public async Task<bool> DeleteRecord(string studentId, int groupIdeaId)
        {
            try
            {
                var record = await _dbContext.StudentGroupIdeas
                .FirstOrDefaultAsync(sg => sg.StudentId == studentId
                                        && sg.GroupIdeaId == groupIdeaId
                                        && sg.DeletedAt == null);

                if (record == null) return false;

                // Cập nhật trạng thái
                record.DeletedAt = DateTime.UtcNow;
                record.Status = 6;
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
      
    }
}
