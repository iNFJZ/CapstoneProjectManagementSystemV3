using DocumentFormat.OpenXml.InkML;
using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ChangeTopicRequestRepository
{
    public class ChangeTopicRequestRepository : RepositoryBase<ChangeTopicRequest>, IChangeTopicRequestRepository
    {
        private readonly DBContext _dbContext;
        public ChangeTopicRequestRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ChangeTopicRequest> GetDetailChangeTopicRequestsByRequestId(int requestId)
        {
            return await _dbContext.ChangeTopicRequests
                .Where(c => c.RequestId == requestId && c.DeletedAt == null)
                .Select(c => new ChangeTopicRequest
                {
                    RequestId = c.RequestId,
                    OldTopicNameEnglish = c.OldTopicNameEnglish,
                    OldTopicNameVietNamese = c.OldTopicNameVietNamese,
                    OldAbbreviation = c.OldAbbreviation,
                    NewTopicNameEnglish = c.NewTopicNameEnglish,
                    NewTopicNameVietNamese = c.NewTopicNameVietNamese,
                    NewAbbreviation = c.NewAbbreviation,
                    EmailSuperVisor = c.EmailSuperVisor,
                    Status = c.Status,
                    ReasonChangeTopic = c.ReasonChangeTopic,
                    StaffComment = c.StaffComment,
                    FinalGroup = new FinalGroup
                    {
                        FinalGroupId = c.FinalGroup.FinalGroupId,
                        GroupName = c.FinalGroup.GroupName,
                        Profession = new Profession
                        {
                            ProfessionId = c.FinalGroup.Profession.ProfessionId
                        }
                    }
                })
                .FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateStatusOfChangeTopicRequest(int changeTopicRequestId, int status, string staffComment)
        {
            var changeTopicRequest = await _dbContext.ChangeTopicRequests
                .FirstOrDefaultAsync(c => c.RequestId == changeTopicRequestId);

            if (changeTopicRequest == null)
            {
                return false;
            }

            // Cập nhật trạng thái
            changeTopicRequest.Status = status;

            // Nếu có StaffComment thì cập nhật
            if (!string.IsNullOrEmpty(staffComment))
            {
                changeTopicRequest.StaffComment = staffComment;
            }

            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<ChangeTopicRequest>> GetChangeTopicRequestsBySupervisorEmail(string supervisorEmail,string searchText, int[] statuses,int semesterId,int offsetNumber,int fetchNumber)
        {
            return await _dbContext.ChangeTopicRequests
                .Where(ctr =>
                    ctr.FinalGroup.SemesterId == semesterId &&
                    statuses.Contains(ctr.Status.Value) &&
                    ctr.DeletedAt == null &&
                    ctr.EmailSuperVisor == supervisorEmail &&
                    (string.IsNullOrEmpty(searchText) ||
                     EF.Functions.Like(ctr.FinalGroup.GroupName.Replace(" ", "").ToUpper(), $"%{searchText}%"))
                )
                .OrderByDescending(ctr => ctr.CreatedAt)
                .Skip(offsetNumber)
                .Take(fetchNumber)
                .ToListAsync();
        }
        public async Task<List<ChangeTopicRequest>> GetChangeTopicRequestsByProfessionId(int[] professions, string searchText,int[] statuses,int semesterId,int offsetNumber, int fetchNumber,string supervisorEmails)
        {
            // Chuyển đổi danh sách email thành mảng (loại bỏ khoảng trắng)
            var emailList = supervisorEmails
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(e => e.Trim())
                .ToList();

            return await _dbContext.ChangeTopicRequests
                .Where(ctr =>
                    ctr.FinalGroup.SemesterId == semesterId &&
                    statuses.Cast<int?>().Contains(ctr.Status) && 
                    ctr.DeletedAt == null &&
                    professions.Contains(ctr.FinalGroup.ProfessionId) &&
                    (string.IsNullOrEmpty(searchText) ||
                     EF.Functions.Like(ctr.FinalGroup.GroupName.Replace(" ", "").ToUpper(), $"%{searchText}%")) &&
                    (string.IsNullOrEmpty(supervisorEmails) || emailList.Contains(ctr.EmailSuperVisor)) // ✅ Lọc theo Supervisor Email
                )
                .OrderByDescending(ctr => ctr.CreatedAt)
                .Skip(offsetNumber)
                .Take(fetchNumber)
                .ToListAsync();
        }

    }
}
