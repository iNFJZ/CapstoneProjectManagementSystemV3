using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ChangeFinalGroupRequestRepository
{
    public class ChangeFinalGroupRequestRepository : RepositoryBase<ChangeFinalGroupRequest>, IChangeFinalGroupRequestRepository
    {
        private readonly DBContext _dbContext;
        public ChangeFinalGroupRequestRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> CountRecordChangeFinalGroupBySearchText(string searchText, int status, int semesterId)
        {

            // Truy vấn LINQ để đếm số bản ghi phù hợp
            var count = await _dbContext.ChangeFinalGroupRequests
                .Where(cfr => cfr.StatusOfTo == 1 &&
                              cfr.StatusOfStaff == status &&
                              cfr.DeletedAt == null &&
                              cfr.FromStudent.FinalGroup.SemesterId == semesterId &&
                              cfr.ToStudent.FinalGroup.SemesterId == semesterId &&
                              cfr.FromStudent.FinalGroup.DeletedAt == null &&
                              cfr.ToStudent.FinalGroup.DeletedAt == null &&
                              (searchText == "" ||
                               cfr.ToStudent.FinalGroup.GroupName.Replace(" ", "").ToUpper().Contains(searchText) ||
                               cfr.FromStudent.FinalGroup.GroupName.Replace(" ", "").ToUpper().Contains(searchText) ||
                               cfr.FromStudent.StudentNavigation.FptEmail.Replace(" ", "").ToUpper().Contains(searchText) ||
                               cfr.ToStudent.StudentNavigation.FptEmail.Replace(" ", "").ToUpper().Contains(searchText)))
                .CountAsync();

            return count;
        }
        public async Task<List<ChangeFinalGroupRequest>> GetListChangeFinalGroupRequestBySearchText( string searchText, int status, int semesterId, int offsetNumber, int fetchNumber)
        {
            searchText = string.IsNullOrEmpty(searchText) ? "" : searchText.ToUpper().Replace(" ", "");

            var requests = await (from cfr in _dbContext.ChangeFinalGroupRequests
                                  join fromStudent in _dbContext.Students on cfr.FromStudentId equals fromStudent.StudentId
                                  join fromUser in _dbContext.Users on fromStudent.StudentId equals fromUser.UserId
                                  join fromGroup in _dbContext.FinalGroups on fromStudent.FinalGroupId equals fromGroup.FinalGroupId
                                  join toStudent in _dbContext.Students on cfr.ToStudentId equals toStudent.StudentId
                                  join toUser in _dbContext.Users on toStudent.StudentId equals toUser.UserId
                                  join toGroup in _dbContext.FinalGroups on toStudent.FinalGroupId equals toGroup.FinalGroupId
                                  where cfr.StatusOfTo == 1 &&
                                        cfr.StatusOfStaff == status &&
                                        cfr.DeletedAt == null &&
                                        fromGroup.SemesterId == semesterId &&
                                        toGroup.SemesterId == semesterId &&
                                        fromGroup.DeletedAt == null &&
                                        toGroup.DeletedAt == null &&
                                        (string.IsNullOrEmpty(searchText) ||
                                         fromGroup.GroupName.ToUpper().Replace(" ", "").Contains(searchText) ||
                                         toGroup.GroupName.ToUpper().Replace(" ", "").Contains(searchText) ||
                                         fromUser.FptEmail.ToUpper().Replace(" ", "").Contains(searchText) ||
                                         toUser.FptEmail.ToUpper().Replace(" ", "").Contains(searchText))
                                  orderby cfr.CreatedAt descending
                                  select new ChangeFinalGroupRequest
                                  {
                                      ChangeFinalGroupRequestId = cfr.ChangeFinalGroupRequestId,
                                      FromStudent = fromStudent,
                                      ToStudent = toStudent,
                                      StaffComment = cfr.StaffComment,
                                      StatusOfStaff = cfr.StatusOfStaff
                                  })
                                 .Skip(offsetNumber)
                                 .Take(fetchNumber)
                                 .ToListAsync();

            return requests;
        }
    }
    }
