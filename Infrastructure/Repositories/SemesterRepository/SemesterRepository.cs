using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Infrastructure.Entities.Dto.SemesterDto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SemesterRepository
{
    public class SemesterRepository : RepositoryBase<Semester> , ISemesterRepository
    {
        private readonly DBContext _dbContext;
        public SemesterRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<SemesterDto> GetCurrentSemester()
        {
            var semester = await _dbContext.Semesters
                .Where(s => s.StatusCloseBit == true && s.DeletedAt == null)
                .Select(s => new SemesterDto
                {
                    SemesterID = s.SemesterId,
                    SemesterName = s.SemesterName,
                    SemesterCode = s.SemesterCode,
                    ShowGroupName = s.ShowGroupName,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    DeadlineChangeIdea = s.DeadlineChangeIdea,
                    IsConfirmationOfDevHeadNeeded = s.IsConfirmationOfDevHeadNeeded,
                    DeadlineRegisterGroup = s.DeadlineRegisterGroup,
                    SubjectMailTemplate = s.SubjectMailTemplate,
                    BodyMailTemplate = s.BodyMailTemplate
                })
                .FirstOrDefaultAsync(); 

            return semester;
        }
        public async Task<SemesterDto> GetSemesterById(int semesterId)
        {
            var semester = await _dbContext.Semesters
                .Where(s => s.SemesterId == semesterId)
                .Select(s => new SemesterDto
                {
                    SemesterID = s.SemesterId,
                    SemesterName = s.SemesterName,
                    SemesterCode = s.SemesterCode,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .FirstOrDefaultAsync();

            return semester;
        }
        public async Task<List<SemesterDto>> GetAllSemester()
        {
            return await _dbContext.Semesters
                .OrderByDescending(s => s.SemesterId)
                .Select(s => new SemesterDto
                {
                    SemesterID = s.SemesterId,
                    SemesterName = s.SemesterName,
                    SemesterCode = s.SemesterCode,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .ToListAsync();
        }

        public async Task<SemesterDto> GetLastSemester()
        {
            var semester = await _dbContext.Semesters
                .Where(s => s.StatusCloseBit == true && s.DeletedAt != null)
                .OrderByDescending(s => s.SemesterId)
                .Select(s => new SemesterDto
                {
                    SemesterID = s.SemesterId,
                    SemesterName = s.SemesterName,
                    SemesterCode = s.SemesterCode,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime
                })
                .FirstOrDefaultAsync();

            return semester;
        }
        public async Task<bool> CloseSemesterCurrent()
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                DateTime currentTime = DateTime.UtcNow;

                // Cập nhật Deleted_At cho từng bảng
                await _dbContext.StudentGroupIdeas
                    .Where(sg => sg.DeletedAt == null)
                    .ForEachAsync(sg => sg.DeletedAt = currentTime);

                await _dbContext.RegisteredGroups
                    .Where(rg => rg.DeletedAt == null)
                    .ForEachAsync(rg => rg.DeletedAt = currentTime);

                await _dbContext.StudentFavoriteGroupIdeas
                    .Where(sfg => sfg.DeletedAt == null)
                    .ForEachAsync(sfg => sfg.DeletedAt = currentTime);

                await _dbContext.GroupIdeas
                    .Where(gi => gi.DeletedAt == null)
                    .ForEachAsync(gi => gi.DeletedAt = currentTime);

                await _dbContext.ChangeTopicRequests
                    .Where(ct => ct.DeletedAt == null)
                    .ForEachAsync(ct => ct.DeletedAt = currentTime);

                await _dbContext.FinalGroups
                    .Where(fg => fg.DeletedAt == null)
                    .ForEachAsync(fg => fg.DeletedAt = currentTime);

                await _dbContext.ChangeFinalGroupRequests
                    .Where(cfr => cfr.DeletedAt == null)
                    .ForEachAsync(cfr => cfr.DeletedAt = currentTime);

                await _dbContext.Notifications
                    .Where(n => n.DeletedAt == null)
                    .ForEachAsync(n => n.DeletedAt = currentTime);

                await _dbContext.Supports
                    .Where(s => s.DeletedAt == null)
                    .ForEachAsync(s => s.DeletedAt = currentTime);

                await _dbContext.AffiliateAccounts
                    .Where(aa => aa.DeletedAt == null &&
                                 _dbContext.Users.Any(u => u.UserId == aa.AffiliateAccountId && !new[] { 2, 3, 4, 5 }.Contains((int)u.RoleId)))
                    .ForEachAsync(aa => aa.DeletedAt = currentTime);

                await _dbContext.News
                    .Where(n => n.DeletedAt == null && n.TypeSupport == false)
                    .ForEachAsync(n => n.DeletedAt = currentTime);

                await _dbContext.Users
                    .Where(u => u.DeletedAt == null &&
                                _dbContext.Students.Any(s => s.StudentId == u.UserId && s.DeletedAt == null) &&
                                !new[] { 2, 3, 4, 5 }.Contains((int)u.RoleId))
                    .ForEachAsync(u => u.DeletedAt = currentTime);

                await _dbContext.Students
                    .Where(s => s.DeletedAt == null)
                    .ForEachAsync(s => s.DeletedAt = currentTime);

                await _dbContext.Semesters
                    .Where(s => s.DeletedAt == null)
                    .ForEachAsync(s =>
                    {
                        s.StatusCloseBit = false;
                        s.DeletedAt = currentTime;
                    });

                //_dbContext.ConfigurationSpecandPro.RemoveRange(_dbContext.ConfigurationSpecandPro);
                _dbContext.Withs.RemoveRange(_dbContext.Withs);

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return false;
            }
        }


    }
}
