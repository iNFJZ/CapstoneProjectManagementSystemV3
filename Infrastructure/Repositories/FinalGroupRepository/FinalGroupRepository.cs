using DocumentFormat.OpenXml.InkML;
using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.FinalGroupRepository
{
    public class FinalGroupRepository : RepositoryBase<FinalGroup>, IFinalGroupRepository
    {
        private readonly DBContext _dbContext;
        public FinalGroupRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<FinalGroupDto>> getAllFinalGroups(int semesterId)
        {
            var result = await _dbContext.FinalGroups
            .Where(fg => fg.SemesterId == semesterId && fg.DeletedAt == null)
            .GroupJoin(_dbContext.Supervisors,
                       fg => fg.SupervisorId,
                       s => s.SupervisorId,
                       (fg, sGroup) => new { fg, sGroup })
            .SelectMany(x => x.sGroup.DefaultIfEmpty(), (x, s) => new { x.fg, s })
            .GroupJoin(_dbContext.Users,
                       x => x.s.SupervisorId,
                       u => u.UserId,
                       (x, uGroup) => new { x.fg, x.s, uGroup })
            .SelectMany(x => x.uGroup.DefaultIfEmpty(), (x, u) => new { x.fg, x.s, u })
            .OrderBy(x => x.fg.SupervisorId)
            .Select(x => new FinalGroupDto
            {
                FinalGroupId = x.fg.FinalGroupId,
                SpecialtyId = x.fg.SpecialtyId,
                SemesterId = x.fg.SpecialtyId,
                SupervisorId = x.s != null ? x.s.SupervisorId : (string?)null,
                SupervisorName = x.s != null ? x.u.FullName : null,
                UserId = x.u != null ? x.u.UserId : (string?)null,
                UserName = x.u != null ? x.u.FullName : null
            })
            .ToListAsync();

            return result;
    }

        public async Task<FinalGroupDto> getFinalGroupById(int id)
            {
            var finalGroupDtos =await _dbContext.FinalGroups
         .Where(fg => fg.FinalGroupId == id && fg.DeletedAt == null)
         .GroupJoin(_dbContext.Supervisors
                     .Where(s => s.IsActive == true && s.DeletedAt == null),
                    fg => fg.SupervisorId,
                    s => s.SupervisorId,
                    (fg, sGroup) => new { fg, sGroup })
         .SelectMany(x => x.sGroup.DefaultIfEmpty(), (x, s) => new { x.fg, s })
         .GroupJoin(_dbContext.Users,
                    x => x.s.SupervisorId,
                    u => u.UserId,
                    (x, uGroup) => new { x.fg, x.s, uGroup })
         .SelectMany(x => x.uGroup.DefaultIfEmpty(), (x, u) => new { x.fg, x.s, u })
         .Join(_dbContext.Professions,
               x => x.fg.ProfessionId,
               p => p.ProfessionId,
               (x, p) => new { x.fg, x.s, x.u, p })
         .Join(_dbContext.Specialties,
               x => x.fg.ProfessionId,
               sp => sp.ProfessionId,
               (x, sp) => new { x.fg, x.s, x.u, x.p, sp })
         .Select(x => new FinalGroupDto
         {
             FinalGroupId = x.fg.FinalGroupId,
             ProfessionId = x.fg.ProfessionId,
             ProfessionFullName = x.p.ProfessionFullName,
             SupervisorId = x.fg.SupervisorId,
             SpecialtyFullName = x.sp.SpecialtyFullName,
             ProjectEnglishName = x.fg.ProjectEnglishName,
             ProjectVietNameseName = x.fg.ProjectVietNameseName,
             Abbreviation = x.fg.Abbreviation,
             Description = x.fg.Description,
             NumberOfMember = x.fg.NumberOfMember,
             MaxMember = x.fg.MaxMember,
             IsConfirmFinalReport = x.fg.IsConfirmFinalReport,
             SupervisorName = x.u != null ? x.u.FullName : null,
             SemesterId = x.fg.SemesterId,
             CreatedAt = x.fg.CreatedAt,
             GroupName = x.fg.GroupName
         })
         .FirstOrDefaultAsync();

            return finalGroupDtos;
        }
        public async Task<List<FinalGroupDto>> GetFullMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber)
        {
            var query =  _dbContext.FinalGroups.AsQueryable();

            // Lọc theo Semester_ID
            query = query.Where(fg => fg.SemesterId == semester_Id);

            // Lọc theo NumberOfMember và MaxMember
            query = query.Where(fg => fg.NumberOfMember >= fg.MaxMember);

            // Lọc theo Profession_ID nếu có
            if (profession_Id != 0)
            {
                query = query.Where(fg => fg.ProfessionId == profession_Id);
            }

            // Lọc theo Specialty_ID nếu có
            if (specialty_Id != 0)
            {
                query = query.Where(fg => fg.SpecialtyId == specialty_Id);
            }

            // Lọc theo SearchText (tìm kiếm trong GroupName và ProjectEnglishName)
            if (!string.IsNullOrEmpty(searchText))
            {
                var formattedSearchText = searchText.Replace(" ", "").ToUpper();
                query = query.Where(fg =>
                    EF.Functions.Like(fg.GroupName.Replace(" ", "").ToUpper(), "%" + formattedSearchText + "%") ||
                    EF.Functions.Like(fg.ProjectEnglishName.Replace(" ", "").ToUpper(), "%" + formattedSearchText + "%"));
            }

            // Lọc các nhóm chưa bị xóa
            query = query.Where(fg => fg.DeletedAt == null);

            // Kết hợp với các bảng Professions và Specialties
            var result = (from fg in query
                          join p in _dbContext.Professions on fg.ProfessionId equals p.ProfessionId
                          join sp in _dbContext.Specialties on fg.SpecialtyId equals sp.SpecialtyId
                          orderby fg.CreatedAt descending
                          select new FinalGroupDto
                          {
                              FinalGroupId = fg.FinalGroupId,
                              ProfessionId = fg.ProfessionId,
                              ProfessionFullName = p.ProfessionFullName,
                              SpecialtyId = fg.SpecialtyId,
                              SpecialtyFullName = sp.SpecialtyFullName,
                              ProjectEnglishName = fg.ProjectEnglishName,
                              ProjectVietNameseName = fg.ProjectVietNameseName,
                              Abbreviation = fg.Abbreviation,
                              Description = fg.Description,
                              NumberOfMember = fg.NumberOfMember,
                              MaxMember = fg.MaxMember,
                              SupervisorId = fg.SupervisorId,
                              IsConfirmFinalReport = fg.IsConfirmFinalReport,
                              SemesterId = fg.SemesterId,
                              CreatedAt = fg.CreatedAt,
                              GroupName = fg.GroupName
                          })
                          .Skip(offsetNumber)  // Dùng OFFSET
                          .Take(fetchNumber)   // Dùng FETCH
                          .ToList();

            return result;
        }
        public async Task<List<FinalGroupDto>> GetLackOfMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber)
        {
            var query = _dbContext.FinalGroups.AsQueryable();

            // Lọc theo Semester_ID
            if (semester_Id != null)
            {
                query = query.Where(fg => fg.SemesterId == semester_Id);
            }

            // Lọc theo NumberOfMember < MaxMember
            query = query.Where(fg => fg.NumberOfMember < fg.MaxMember);

            // Lọc theo Profession_ID nếu có
            if (profession_Id != null)
            {
                query = query.Where(fg => fg.ProfessionId == profession_Id);
            }

            // Lọc theo Specialty_ID nếu có
            if (specialty_Id != null)
            {
                query = query.Where(fg => fg.SpecialtyId == specialty_Id);
            }

            // Lọc theo SearchText (tìm kiếm trong GroupName và ProjectEnglishName)
            if (!string.IsNullOrEmpty(searchText))
            {
                var formattedSearchText = searchText.Replace(" ", "").ToUpper();
                query = query.Where(fg =>
                    EF.Functions.Like(fg.GroupName.Replace(" ", "").ToUpper(), "%" + formattedSearchText + "%") ||
                    EF.Functions.Like(fg.ProjectEnglishName.Replace(" ", "").ToUpper(), "%" + formattedSearchText + "%"));
            }

            // Lọc các nhóm chưa bị xóa
            query = query.Where(fg => fg.DeletedAt == null);

            // Kết hợp với các bảng Professions và Specialties
            var result = await (from fg in query
                          join p in _dbContext.Professions on fg.ProfessionId equals p.ProfessionId
                          join sp in _dbContext.Specialties on fg.SpecialtyId equals sp.SpecialtyId
                          orderby fg.CreatedAt descending
                          select new FinalGroupDto
                          {
                              FinalGroupId = fg.FinalGroupId,
                              ProfessionId = fg.ProfessionId,
                              ProfessionFullName = p.ProfessionFullName,
                              SpecialtyId = fg.SpecialtyId,
                              SpecialtyFullName = sp.SpecialtyFullName,
                              ProjectEnglishName = fg.ProjectEnglishName,
                              ProjectVietNameseName = fg.ProjectVietNameseName,
                              Abbreviation = fg.Abbreviation,
                              Description = fg.Description,
                              NumberOfMember = fg.NumberOfMember,
                              MaxMember = fg.MaxMember,
                              SupervisorId = fg.SupervisorId,
                              IsConfirmFinalReport = fg.IsConfirmFinalReport,
                              SemesterId = fg.SemesterId,
                              CreatedAt = fg.CreatedAt,
                              GroupName = fg.GroupName
                          })
                          .Skip(offsetNumber)  // Dùng OFFSET
                          .Take(fetchNumber)   // Dùng FETCH
                          .ToListAsync();

            return result;
        }
        public async Task UpdateNumberOfMemberWhenAdd(int groupId)
        {
            var finalGroup = await _dbContext.FinalGroups
                .FirstOrDefaultAsync(fg => fg.FinalGroupId == groupId && fg.DeletedAt == null);

            if (finalGroup != null)
            {
                finalGroup.NumberOfMember += 1;
                _dbContext.SaveChanges();
            }
        }
        public async Task<FinalGroupDto> GetFinalGroupByStudentIsLeader(string studentId, int semesterId)
        {
            var result =await (from st in _dbContext.Students
                          join f in _dbContext.FinalGroups on st.FinalGroupId equals f.FinalGroupId
                          join s in _dbContext.Supervisors on f.SupervisorId equals s.SupervisorId into supervisorGroup
                          from s in supervisorGroup.DefaultIfEmpty()
                          where st.IsLeader == true
                                && st.StudentId == studentId
                                && f.SemesterId == semesterId
                                && f.DeletedAt == null
                          select new FinalGroupDto
                          {
                              GroupName = f.GroupName,
                              FinalGroupId = f.FinalGroupId,
                              SupervisorId = s != null ? s.SupervisorId : (string?)null
                          }).FirstOrDefaultAsync();

            return result;
        }
        public async Task<int> GetMaxMemberOfFinalGroupByGroupName(string groupName, int semesterId)
        {
           int finalGroup = (int)await _dbContext.FinalGroups
                .Where(fg => fg.GroupName == groupName 
                          && fg.SemesterId == semesterId
                          && fg.DeletedAt == null)
                .Select(fg => fg.MaxMember)
                .FirstOrDefaultAsync();
            return finalGroup;
        }
        public async Task<FinalGroupDto> GetOldTopicByFinalGroupId(int finalGroupId)
        {
            return await _dbContext.FinalGroups
                .Where(fg => fg.FinalGroupId == finalGroupId && fg.DeletedAt == null)
                .Select(fg => new FinalGroupDto
                {
                    ProjectEnglishName = fg.ProjectEnglishName,
                    ProjectVietNameseName = fg.ProjectVietNameseName,
                    Abbreviation = fg.Abbreviation
                })
                .FirstOrDefaultAsync();
        }
        public async Task<List<StudentDto>> GetListCurrentMemberHaveFinalGroupByGroupName(string groupName, int semesterId)
        {
            var students = await (from s in _dbContext.Students
                            join f in _dbContext.FinalGroups on s.FinalGroupId equals f.FinalGroupId
                            join u in _dbContext.Users on s.StudentId equals u.UserId
                            join p in _dbContext.Professions on s.ProfessionId equals p.ProfessionId
                            join sp in _dbContext.Specialties on s.SpecialtyId equals sp.SpecialtyId
                            where f.GroupName == groupName
                                  && f.SemesterId == semesterId
                                  && s.DeletedAt == null
                                  && u.DeletedAt == null
                                  && f.DeletedAt == null
                            select new StudentDto
                            {
                                StudentId = s.StudentId,
                                EmailAddress = u.FptEmail,
                                Avatar = u.Avatar,
                                IsLeader = s.IsLeader,
                                ProfessionFullName = p.ProfessionFullName,
                                SpecialtyFullName = sp.SpecialtyFullName
                            }).ToListAsync();

            return students;
        }
        public async Task<string> GetLatestGroupName(string codeOfGroupName)
        {
            var latestGroupName = await (from f in _dbContext.FinalGroups
                                   where f.GroupName.Contains(codeOfGroupName)
                                         && f.SemesterId == (_dbContext.Semesters
                                                              .Where(s => s.StatusCloseBit == true)
                                                              .Select(s => s.SemesterId)
                                                              .FirstOrDefault())
                                         && f.DeletedAt == null
                                   orderby f.CreatedAt descending
                                   select f.GroupName)
                                  .FirstOrDefaultAsync();

            return latestGroupName;
        }
        public async Task<List<FinalGroupDto>> GetListFinalGroupBySupervisorID(string supervisorId, int semesterId)
        {
            var lastSemesterId = await _dbContext.Semesters
                                         .OrderByDescending(s => s.SemesterId)
                                         .Select(s => s.SemesterId)
                                         .FirstOrDefaultAsync(); // Lấy học kỳ mới nhất

            var finalGroups =await (from fg in _dbContext.FinalGroups
                               join sp in _dbContext.Specialties on fg.SpecialtyId equals sp.SpecialtyId
                               join sem in _dbContext.Semesters on fg.SemesterId equals sem.SemesterId
                               where (fg.SemesterId == lastSemesterId || fg.SemesterId == semesterId)
                                     && fg.SupervisorId == supervisorId
                               select new FinalGroupDto
                               {
                                   FinalGroupId = fg.FinalGroupId,
                                   GroupName = fg.GroupName,
                                   ProjectEnglishName = fg.ProjectEnglishName,
                                   SpecialtyId = fg.SpecialtyId,
                                   SpecialtyFullName = sp.SpecialtyFullName,
                                   IsConfirmFinalReport = fg.IsConfirmFinalReport,
                                   SemesterId = sem.SemesterId,
                                   SemesterCode = sem.SemesterCode
                               }).ToListAsync();

            return finalGroups;
        }
        public async Task<bool> ConfirmFinalReport(FinalGroupDto finalGroup)
        {
            var group = await _dbContext.FinalGroups
                                 .FirstOrDefaultAsync(fg => fg.FinalGroupId == finalGroup.FinalGroupId);

            if (group != null)
            {
                group.IsConfirmFinalReport = finalGroup.IsConfirmFinalReport;
                _dbContext.SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<FinalGroupDto> GetFinalGroupDetailForSupervisor(int finalGroupId)
        {
            var finalGroup = await(from fg in _dbContext.FinalGroups
                              join sp in _dbContext.Specialties on fg.SpecialtyId equals sp.SpecialtyId
                              join p in _dbContext.Professions on fg.ProfessionId equals p.ProfessionId
                              join s in _dbContext.Supervisors on fg.SupervisorId equals s.SupervisorId into supervisors
                              from supervisor in supervisors.DefaultIfEmpty()
                              join u in _dbContext.Users on supervisor.SupervisorId equals u.UserId into users
                              from user in users.DefaultIfEmpty()
                              where fg.FinalGroupId == finalGroupId
                              select new FinalGroupDto
                              {
                                  FinalGroupId = fg.FinalGroupId,
                                  ProfessionId = fg.ProfessionId,
                                  ProfessionFullName = p.ProfessionFullName,
                                  SpecialtyId = fg.SpecialtyId,
                                  SpecialtyFullName = sp.SpecialtyFullName,
                                  ProjectEnglishName = fg.ProjectEnglishName,
                                  ProjectVietNameseName = fg.ProjectVietNameseName,
                                  Abbreviation = fg.Abbreviation,
                                  Description = fg.Description,
                                  NumberOfMember = fg.NumberOfMember,
                                  MaxMember = fg.MaxMember,
                                  SupervisorId = fg.SupervisorId,
                                  IsConfirmFinalReport = fg.IsConfirmFinalReport,
                                  UserName = user != null ? user.FullName : null, // Handle null for user
                                  SemesterId = fg.SemesterId,
                                  CreatedAt = fg.CreatedAt,
                                  GroupName = fg.GroupName
                              }).FirstOrDefaultAsync();

            return finalGroup;
        }
        public async Task<List<StudentDto>> GetListMemberByFinalGroupId(int finalGroupId)
        {
            var students = await (from s in _dbContext.Students
                            join f in _dbContext.FinalGroups on s.FinalGroupId equals f.FinalGroupId
                            join u in _dbContext.Users on s.StudentId equals u.UserId
                            join p in _dbContext.Professions on s.ProfessionId equals p.ProfessionId
                            join sp in _dbContext.Specialties on s.SpecialtyId equals sp.SpecialtyId
                            where s.FinalGroupId == finalGroupId
                            select new StudentDto
                            {
                                StudentId = s.StudentId,
                                EmailAddress = u.FptEmail,
                                Avatar = u.Avatar,
                                IsLeader = s.IsLeader,
                                ProfessionFullName = p.ProfessionFullName,
                                SpecialtyFullName = sp.SpecialtyFullName
                            }).ToListAsync();

            return students;
        }
    }
}

