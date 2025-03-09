using DocumentFormat.OpenXml.InkML;
using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Infrastructure.Entities.Dto.ViewModel.AdminViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.UserRepository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly DBContext _dbContext;
        public UserRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> AddUser(User user, int roleId, int professionId, int specialtyId)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Thêm vào bảng Users
                    var newUser = new User
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        FptEmail = user.FptEmail,
                        FullName = user.FullName,
                        RoleId = roleId
                    };

                    await _dbContext.Users.AddAsync(newUser);
                    await _dbContext.SaveChangesAsync();

                    // Xử lý RollNumber
                    string rollNumber = GetRollNumber(user.UserName);

                    // Lấy Semester_ID của kỳ học đang mở
                    int? semesterId = _dbContext.Semesters
                        .Where(s => s.StatusCloseBit == true)
                        .Select(s => s.SemesterId)
                        .FirstOrDefault();

                    // Thêm vào bảng Students
                    var newStudent = new Student
                    {
                        StudentId = user.UserId,
                        RollNumber = rollNumber,
                        SemesterId = semesterId ?? 0, // Nếu không có, để 0 hoặc xử lý ngoại lệ
                        ProfessionId = professionId,
                        SpecialtyId = specialtyId == 0 ? (int?)null : specialtyId
                    };

                    await _dbContext.Students.AddAsync(newStudent);
                    await _dbContext.SaveChangesAsync();

                    transaction.Commit(); // Xác nhận giao dịch
                    return true;
                }
                catch (Exception)
                {
                    transaction.Rollback(); // Hoàn tác nếu có lỗi
                    throw;
                }
            }
        }
        private string GetRollNumber(string userName)
        {
            int countAccount = userName.TakeWhile(c => char.IsDigit(c)).Count();

            if (countAccount == 5)
                return userName.Substring(userName.Length - 7).ToUpper();
            if (countAccount == 6)
                return userName.Substring(userName.Length - 8).ToUpper();
            return "";
        }
        public async Task<bool> CheckProfileUserHaveAttributeIsNullByUserId(string userId)
        {
            return await _dbContext.Students
                .Join(_dbContext.Users,
                    s => s.StudentId,
                    u => u.UserId,
                    (s, u) => new { s, u })
                .Join(_dbContext.AffiliateAccounts,
                    su => su.u.UserId,
                    a => a.AffiliateAccountId,
                    (su, a) => new { su.s, su.u, a })
                .Where(x => x.u.UserId == userId &&
                            x.a.AffiliateAccountId != null &&
                            x.s.SelfDiscription != null &&
                            x.s.ExpectedRoleInGroup != null &&
                            x.s.PhoneNumber != null &&
                            x.s.LinkFacebook != null &&
                            x.u.DeletedAt == null &&
                            x.s.DeletedAt == null)
                .AnyAsync(); // Trả về `true` nếu có bản ghi phù hợp, `false` nếu không có
        }
        public async Task<bool> CheckReferenceDUserData(UserDto user)
        {
            if (user.Role.Role_ID == 3)
            {
                // Kiểm tra số lượng bản ghi trong bảng News với Staff_ID
                return await _dbContext.News
                    .Where(n => n.StaffId == user.UserID)
                    .AnyAsync();
            }
            else
            {
                // Kiểm tra số lượng bản ghi trong nhiều bảng với Supervisor_ID
                var hasFinalGroups = await _dbContext.FinalGroups
                    .Where(fg => fg.SupervisorId == user.UserID)
                .AnyAsync();

                var hasSupervisorProfessions = await _dbContext.SupervisorProfessions
                    .Where(sp => sp.SupervisorId == user.UserID)
                .AnyAsync();

                var hasRegisteredGroups = await _dbContext.RegisterdGroupSupervisors
                    .Where(rg => rg.SupervisorId == user.UserID)
                .AnyAsync();

                var hasGroupIdeas = await _dbContext.GroupIdeasOfSupervisors
                    .Where(gi => gi.SupervisorId == user.UserID)
                    .AnyAsync();

                // Trả về true nếu bất kỳ bảng nào có dữ liệu
                return hasFinalGroups || hasSupervisorProfessions || hasRegisteredGroups || hasGroupIdeas;
            }
        }
        public async Task<bool> CheckUserByUserIdAndRoleExist(string userId, string role)
        {
            return await _dbContext.Users
                .Join(_dbContext.Roles,
                    u => u.RoleId,
                    r => r.RoleId,
                    (u, r) => new { User = u, Role = r })
                .Where(ur => ur.User.UserId == userId
                          && ur.Role.RoleName == role
                          && ur.User.DeletedAt == null
                          && ur.Role.DeletedAt == null)
                .AnyAsync(); // Kiểm tra có bản ghi nào thỏa mãn không
        }
        public async Task<List<string>> GetListFptEmailByGroupIdeaId(int groupIdeaId)
        {
            return await _dbContext.StudentGroupIdeas
                .Where(sg => sg.GroupIdeaId == groupIdeaId &&
                             (sg.Status == 1 || sg.Status == 2) &&
                             sg.DeletedAt == null)
                .Join(_dbContext.Students,
                      sg => sg.StudentId,
                      s => s.StudentId,
                      (sg, s) => new { s.StudentId })
                .Join(_dbContext.Users,
                      s => s.StudentId,
                      u => u.UserId,
                      (s, u) => u.FptEmail)
                .ToListAsync();
        }
        public async Task<List<UserWithRowNum>> GetListUserByRoleList(List<int> roleIds)
        {
            if (roleIds == null || !roleIds.Any())
            {
                return new List<UserWithRowNum>();
            }

            var users = await _dbContext.Users
                .Where(u => roleIds.Contains((int)u.RoleId) && u.DeletedAt == null)
                .Join(_dbContext.Roles,
                      u => u.RoleId,
                      r => r.RoleId,
                      (u, r) => new UserWithRowNum
                      {
                          UserID = u.UserId,
                          UserName = u.UserName,
                          FptEmail = u.FptEmail,
                          FullName = u.FullName,
                          Avatar = u.Avatar,
                          Gender = u.Gender,
                          RoleID = r.RoleId,
                          Created_At = u.CreatedAt,
                          RoleName = r.RoleName,
                      })
                .ToListAsync();

            return users.Select((u, index) => new UserWithRowNum
            {
                RowNum = index + 1,
                UserID = u.UserID,
                UserName = u.UserName,
                FptEmail = u.FptEmail,
                FullName = u.FullName ?? "",
                Avatar = u.Avatar,
                Gender = u.Gender,
                Created_At = u.Created_At,
                Role = new Role()
                {
                    RoleId = u.RoleID,
                    RoleName = u.RoleName == "DevHead" ? "Department Leader" : u.RoleName
                }
            }).ToList();
        }
        public async Task<(int TotalPages, int TotalRecords, List<UserWithRowNum> Users)> GetListUserForAdminPaging(int pageNumber, string search, int role)
        {
            int pageSize = 10; // Số lượng bản ghi trên mỗi trang
            int skipRecords = (pageNumber - 1) * pageSize;

            // Bước 1: Đếm tổng số bản ghi
            var totalRecords = await _dbContext.Users
                .Where(u => u.DeletedAt == null &&
                       (string.IsNullOrEmpty(search) || u.FptEmail.Contains(search) || u.FullName.Contains(search)) &&
                       (role == 0 || u.RoleId == role))
                .CountAsync();

            if (totalRecords == 0)
                return (0, 0, new List<UserWithRowNum>());

            int totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Bước 2: Lấy danh sách người dùng với phân trang
            var users = await _dbContext.Users
                .Where(u => u.DeletedAt == null &&
                       (string.IsNullOrEmpty(search) || u.FptEmail.Contains(search) || u.FullName.Contains(search)) &&
                       (role == 0 || u.RoleId == role))
                .Join(_dbContext.Roles,
                      user => user.RoleId,
                      role => role.RoleId,
                      (user, role) => new
                      {
                          user.UserId,
                          user.UserName,
                          user.FptEmail,
                          user.FullName,
                          user.Avatar,
                          user.Gender,
                          user.CreatedAt,
                          role.RoleId,
                          role.RoleName
                      })
                .OrderBy(u => u.UserId)
                .Skip(skipRecords)
                .Take(pageSize)
                .ToListAsync();

            // Bước 3: Chuyển đổi sang danh sách UserWithRowNum
            var userList = users.Select((u, index) => new UserWithRowNum
            {
                RowNum = skipRecords + index + 1,
                UserID = u.UserId,
                UserName = u.UserName,
                FptEmail = u.FptEmail,
                FullName = u.FullName ?? "",
                Avatar = u.Avatar,
                Gender = u.Gender,
                Created_At = u.CreatedAt,
                Role = new Role
                {
                    RoleId = u.RoleId,
                    RoleName = u.RoleName == "DevHead" ? "Department Leader" : u.RoleName
                }
            }).ToList();

            return (totalPages, totalRecords, userList);
        }
        public async Task<string> GetListUserIDByGroupIdeaId(int groupIdeaId)
        {
            var userIds = await _dbContext.StudentGroupIdeas
                .Where(sg => sg.GroupIdeaId == groupIdeaId &&
                            (sg.Status == 1 || (sg.Status == 2 && sg.DeletedAt == null)))
                .Join(_dbContext.Students,
                      sg => sg.StudentId,
                      s => s.StudentId,
                      (sg, s) => new { s.StudentId })
                .Join(_dbContext.Users,
                      s => s.StudentId,
                      u => u.UserId,
                      (s, u) => u.UserId)
                .FirstOrDefaultAsync();

            return userIds;
        }
        public async Task<string> GetStudentNameByUserId(string userId)
        {
            var nameStudent = await _dbContext.Users
                .Where(u => u.UserId == userId && u.DeletedAt == null)
                .Join(_dbContext.Students.Where(s => s.DeletedAt == null),
                      u => u.UserId,
                      s => s.StudentId,
                      (u, s) => new { FullName = u.FullName, RollNumber = s.RollNumber })
                .Select(result => result.FullName + " (" + result.RollNumber + ")")
                .FirstOrDefaultAsync();

            return nameStudent;
        }
        public async Task<User> GetUserByFptEmailAsync(string fptEmail, int roleLoginAs)
        {
            var query = _dbContext.Users
                .Where(u => u.FptEmail == fptEmail && u.DeletedAt == null);

            if (roleLoginAs == 3) // staff
            {
                query = query.Where(u => u.RoleId == 1);
            }
            else if (roleLoginAs == 5) // admin
            {
                query = query.Where(u => new[] { 1, 2, 3, 4 }.Contains(u.RoleId.Value));
            }

            var user = await query
                .Select(u => new User
                {
                    UserId = u.UserId,
                    UserName = u.UserName ?? "",
                    FptEmail = u.FptEmail ?? "",
                    Avatar = u.Avatar ?? "",
                    FullName = u.FullName ?? "",
                    Role = new Role
                    {
                        RoleId = u.RoleId.Value
                    }
                })
                .FirstOrDefaultAsync();

            return user;
        }

        Task<List<UserWithRowNum>> IUserRepository.GetListUserForAdminPaging(int pageNumber, string search, int role)
        {
            throw new NotImplementedException();
        }
    }
}
