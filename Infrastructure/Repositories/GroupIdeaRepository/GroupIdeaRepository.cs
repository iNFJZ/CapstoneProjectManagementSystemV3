using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.GroupIdeaRepository
{
    public class GroupIdeaRepository : RepositoryBase<GroupIdea>, IGroupIdeaRepository
    {
        private readonly DBContext _dbContext;
        public GroupIdeaRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<GroupIdea>> GetGroupIdeaSearchList(string semester_Id,string profession_Id,string specialty_Id, string searchText,int offsetNumber, int fetchNumber)
        {
            var query = _dbContext.GroupIdeas
                    .Where(g => g.SemesterId == int.Parse(semester_Id) && g.NumberOfMember < g.MaxMember && g.DeletedAt == null);

            // Lọc theo Profession_ID nếu có
            if (!string.IsNullOrEmpty(profession_Id))
            {
                query = query.Where(g => g.ProfessionId == int.Parse(profession_Id));
            }

            // Lọc theo Specialty_ID nếu có
            if (!string.IsNullOrEmpty(specialty_Id))
            {
                query = query.Where(g => g.SpecialtyId == int.Parse(specialty_Id));
            }

            // Tìm kiếm theo SearchText
            if (!string.IsNullOrEmpty(searchText))
            {
                string searchNormalized = searchText.ToUpper().Replace(" ", "");
                query = query.Where(g =>
                    g.ProjectTags.ToUpper().Replace(" ", "").Contains(searchNormalized) ||
                    g.ProjectEnglishName.ToUpper().Replace(" ", "").Contains(searchNormalized) ||
                    g.Abbreviation.ToUpper().Replace(" ", "").Contains(searchNormalized)
                );
            }

            // Phân trang (OFFSET + FETCH)
            var result = await query
                .OrderBy(g => g.GroupIdeaId)
                .Skip(offsetNumber)
                .Take(fetchNumber)
                .Select(g => new GroupIdea
                {
                    GroupIdeaId = g.GroupIdeaId,
                    ProjectEnglishName = g.ProjectEnglishName,
                    ProjectTags = g.ProjectTags,
                    Profession = new Profession { ProfessionId = g.ProfessionId },
                    Specialty = new Specialty { SpecialtyId = g.SpecialtyId },
                    Description = g.Description,
                    NumberOfMember = g.NumberOfMember,
                    MaxMember = g.MaxMember,
                    CreatedAt = g.CreatedAt,
                    Semester = new Semester { SemesterId = g.SemesterId }
                })
                .ToListAsync();

            return result;
        }
        public async Task<List<GroupIdea>> GetGroupIdeaSearchList_2(string semesterId, string professionId, string specialtyId, string searchText, string studentId, int offsetNumber, int fetchNumber)
        {
            // Sử dụng _dbContext đã được inject
            var excludedGroupIdeaIds = await _dbContext.StudentGroupIdeas
                .Where(sg => sg.StudentId == studentId && (sg.Status == 1 || sg.Status == 2) && sg.DeletedAt == null)
                .Select(sg => sg.GroupIdeaId)
                .ToListAsync();

            var query = _dbContext.GroupIdeas
                .Where(g => g.SemesterId ==int.Parse(semesterId) &&
                            g.NumberOfMember < g.MaxMember &&
                            !excludedGroupIdeaIds.Contains(g.GroupIdeaId) &&
                            g.DeletedAt == null);

            if (!string.IsNullOrEmpty(professionId))
                query = query.Where(g => g.ProfessionId.ToString() == professionId);

            if (!string.IsNullOrEmpty(specialtyId))
                query = query.Where(g => g.SpecialtyId.ToString() == specialtyId);

            if (!string.IsNullOrEmpty(searchText))
            {
                string normalizedSearch = searchText.Replace(" ", "").ToUpper();
                query = query.Where(g =>
                    g.ProjectTags.Replace(" ", "").ToUpper().Contains(normalizedSearch) ||
                    g.ProjectEnglishName.Replace(" ", "").ToUpper().Contains(normalizedSearch) ||
                    g.Abbreviation.Replace(" ", "").ToUpper().Contains(normalizedSearch)
                );
            }

            var result = await query
                .OrderBy(g => g.GroupIdeaId)
                .Skip(offsetNumber)
                .Take(fetchNumber)
                .Select(g => new GroupIdea
                {
                    GroupIdeaId = g.GroupIdeaId,
                    ProjectEnglishName = g.ProjectEnglishName,
                    ProjectTags = g.ProjectTags,
                    Profession = new Profession
                    {
                        ProfessionId = g.Profession.ProfessionId
                    },
                    Specialty = new Specialty
                    {
                        SpecialtyId = g.Specialty.SpecialtyId
                    },
                    Description = g.Description,
                    NumberOfMember = g.NumberOfMember,
                    MaxMember = g.MaxMember,
                    CreatedAt = g.CreatedAt,
                    Semester = new Semester
                    {
                        SemesterId = g.Semester.SemesterId
                    }
                })
                .ToListAsync();

            return result;
        }
        public async Task<int> GetNumberOfResultWhenSearchAsync(string semester_Id, string profession_Id, string specialty_Id, string searchText)
        {
            var query = _dbContext.GroupIdeas
                                  .Where(gi => gi.SemesterId == Convert.ToInt32(semester_Id) &&
                                               gi.NumberOfMember < gi.MaxMember &&
                                               (string.IsNullOrEmpty(profession_Id) || gi.ProfessionId == Convert.ToInt32(profession_Id)) &&
                                               (string.IsNullOrEmpty(specialty_Id) || gi.SpecialtyId == Convert.ToInt32(specialty_Id)) &&
                                               (string.IsNullOrEmpty(searchText) ||
                                                EF.Functions.Like(gi.ProjectTags.Replace(" ", "").ToUpper(), "%" + searchText.Replace(" ", "").ToUpper()) ||
                                                EF.Functions.Like(gi.ProjectEnglishName.Replace(" ", "").ToUpper(), "%" + searchText.Replace(" ", "").ToUpper()) ||
                                                EF.Functions.Like(gi.Abbreviation.Replace(" ", "").ToUpper(), "%" + searchText.Replace(" ", "").ToUpper()))
                                  );
            int count = await query.CountAsync();

            return count;
        }
        public async Task<int> GetNumberOfResultWhenSearch2(string semester_Id, string profession_Id, string specialty_Id, string searchText, string studentId)
        {
            var query = _dbContext.GroupIdeas
                .Where(gi => gi.SemesterId.ToString() == semester_Id // Điều kiện Semester_ID
                             && gi.NumberOfMember < gi.MaxMember // Điều kiện số thành viên
                             && (string.IsNullOrEmpty(profession_Id) || gi.ProfessionId.ToString() == profession_Id) // Điều kiện Profession_ID
                             && (string.IsNullOrEmpty(specialty_Id) || gi.SpecialtyId.ToString() == specialty_Id) // Điều kiện Specialty_ID
                             && (string.IsNullOrEmpty(searchText) ||
                                  gi.ProjectTags.ToUpper().Replace(" ", "").Contains(searchText.ToUpper().Replace(" ", "")) ||
                                  gi.ProjectEnglishName.ToUpper().Replace(" ", "").Contains(searchText.ToUpper().Replace(" ", "")) ||
                                  gi.Abbreviation.ToUpper().Replace(" ", "").Contains(searchText.ToUpper().Replace(" ", ""))) // Điều kiện SearchText
                             && gi.DeletedAt == null) // Điều kiện Deleted_At
                // Kiểm tra xem GroupIdea có trong bảng Student_GroupIdea với Student_ID = studentId không
                .Where(gi => !_dbContext.StudentGroupIdeas
                                      .Where(sgi => sgi.StudentId == studentId && (sgi.Status == 1 || sgi.Status == 2) && sgi.DeletedAt == null)
                                      .Select(sgi => sgi.GroupIdeaId)
                                      .Contains(gi.GroupIdeaId)) // Đảm bảo không có GroupIdea_ID trùng
                .CountAsync(); // Trả về số lượng kết quả

            return await query;
        }
        public async Task<GroupIdea> GetGroupIdeaByIdAsync(int id)
        {
            var groupIdea = await _dbContext.GroupIdeas
                .Where(g => g.GroupIdeaId == id && g.DeletedAt == null)
                .Select(g => new GroupIdea
                {
                    SemesterId = g.SemesterId,
                    Profession = new Profession()
                    {
                        SemesterId = g.SemesterId
                    },
                    Specialty = new Specialty()
                    {
                        SemesterId = g.SemesterId
                    },
                    ProjectEnglishName = g.ProjectEnglishName,
                    ProjectVietNameseName = g.ProjectVietNameseName,
                    Abbreviation = g.Abbreviation,
                    Description = g.Description,
                    ProjectTags = g.ProjectTags,
                    Semester = new Semester()
                    {
                        SemesterId = g.SemesterId
                    },
                    NumberOfMember = g.NumberOfMember,
                    MaxMember = g.MaxMember,
                    CreatedAt = g.CreatedAt
                })
                .FirstOrDefaultAsync();

            return groupIdea;
        }
        public async Task<List<GroupIdea>> GetGroupIdeasByUserIDAsync(string userID)
        {
            var groupIdeas = await _dbContext.StudentGroupIdeas
                .Where(sgi => sgi.StudentId == userID && sgi.GroupIdea.DeletedAt == null) // Điều kiện với Student và GroupIdea
                .Select(sgi => new GroupIdea
                {
                    GroupIdeaId = sgi.GroupIdea.GroupIdeaId,
                    ProjectEnglishName = sgi.GroupIdea.ProjectEnglishName ?? "",
                    ProjectVietNameseName = sgi.GroupIdea.ProjectVietNameseName ?? "",
                    Abbreviation = sgi.GroupIdea.Abbreviation ?? "",
                    Description = sgi.GroupIdea.Description ?? "",
                    ProjectTags = sgi.GroupIdea.ProjectTags ?? "",
                    NumberOfMember = sgi.GroupIdea.NumberOfMember,
                    MaxMember = sgi.GroupIdea.MaxMember,
                    CreatedAt = sgi.GroupIdea.CreatedAt ?? DateTime.MinValue,
                    Semester = new Semester()
                    {
                        SemesterId = sgi.GroupIdea.SemesterId
                    },
                    Profession = new Profession()
                    {
                        ProfessionId = sgi.GroupIdea.ProfessionId
                    }
                })
                .ToListAsync();

            return groupIdeas;
        }
    }
}
