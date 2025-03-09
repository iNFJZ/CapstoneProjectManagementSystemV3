using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.SemesterDto;
using Infrastructure.Entities.Dto.SpecialtyDto;
using Infrastructure.Entities.Dto.ViewModel.SupervisorViewModel;
using Infrastructure.Repositories;
using Infrastructure.Repositories.GroupIdeaOfSupervisorProfessionRepository;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SpecialtyRepository;
using Infrastructure.Repositories.Supervisor_GroupIdeaReporitory;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Services.PrivateService.SupervisorGroupIdeaService;
using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorGroupIdeaService
{
    public class SupervisorGroupIdeaService : ISupervisorGroupIdeaService
    {
        private readonly ISupervisorGroupIdeaReporitory _supervisorGroupIdeaReporitory;
        private readonly IGroupIdeaOfSupervisorProfessionRepository _groupIdeaOfSupervisorProfessionRepository;
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        public SupervisorGroupIdeaService(ISupervisorGroupIdeaReporitory supervisorGroupIdeaReporitory,
            IGroupIdeaOfSupervisorProfessionRepository groupIdeaOfSupervisorProfessionRepository,
            IGroupIdeaRepository groupIdeaRepository,
            IProfessionRepository professionRepository,
            ISpecialtyRepository specialtyRepository,
            ISupervisorRepository supervisorRepository)
        {
            _supervisorGroupIdeaReporitory = supervisorGroupIdeaReporitory;
            _groupIdeaOfSupervisorProfessionRepository = groupIdeaOfSupervisorProfessionRepository;
            _groupIdeaRepository = groupIdeaRepository;
            _professionRepository = professionRepository;
            _specialtyRepository = specialtyRepository;
            _supervisorRepository = supervisorRepository;
        }

        public async Task<ApiResult<bool>> CreateNewGroupIdeaOfMentor(string Supervisor, List<GroupIdeaOfSupervisorProfessionDto> groupIdeaOfSupervisorProfessions, string ProjectEnglishName, string ProjetVietnameseName, string Abbreviation, string Description, string ProjectTags, int Semester, int NumberOfMember, int MaxMember)
        {
            try
            {
                var groupIdea = new GroupIdeasOfSupervisor
                {
                    SupervisorId = Supervisor,
                    ProjectEnglishName = ProjectEnglishName,
                    ProjectVietNameseName = ProjetVietnameseName,
                    Abbreviation = Abbreviation,
                    Description = Description,
                    ProjectTags = ProjectTags,
                    SemesterId = Semester,
                    NumberOfMember = NumberOfMember,
                    MaxMember = MaxMember,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    GroupIdeaOfSupervisorProfessions = groupIdeaOfSupervisorProfessions.Select(dto => new GroupIdeaOfSupervisorProfession
                    {
                        GroupIdeaId = dto.GroupIdea.GroupIdeaID,
                        ProfessionId = dto.Profession.ProfessionID,
                        SpecialtyId = dto.Specialty.SpecialtyID
                    }).ToList()
                };

                await _supervisorGroupIdeaReporitory.CreateAsync(groupIdea);


                foreach (var profession in groupIdeaOfSupervisorProfessions)
                {
                    var groupIdeaProfession = new GroupIdeaOfSupervisorProfession
                    {
                        GroupIdeaId = groupIdea.GroupIdeaId,
                        ProfessionId = profession.Profession.ProfessionID,
                        SpecialtyId = profession.Specialty.SpecialtyID
                    };
                    await _groupIdeaOfSupervisorProfessionRepository.CreateAsync(groupIdeaProfession);
                }

                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>(ex.Message);
            }
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> DeleteGroupIdea(int groupIdeaId)
        {
            var result = await _supervisorGroupIdeaReporitory.GetById(sg => sg.GroupIdeaId == groupIdeaId && sg.DeletedAt == null);
            if (result == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy dữ liệu");
            }
            result.DeletedAt = DateTime.Now;
            await _supervisorGroupIdeaReporitory.UpdateAsync(result);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<GroupIdeaOfSupervisorDto>> GetAllGroupIdeaById(int Id)
        {
            var groupIdea = await _supervisorGroupIdeaReporitory.GetById(g => g.GroupIdeaId == Id);
            if (groupIdea == null)
            {
                return new ApiErrorResult<GroupIdeaOfSupervisorDto>("Không tìm thấy Group Idea.");
            }

            // Truy vấn lấy danh sách các ngành nghề liên quan từ GroupIdeaOfSupervisor_Profession
            var professions = await _groupIdeaOfSupervisorProfessionRepository.GetByCondition(p => p.GroupIdeaId == Id);

            // Truy vấn lấy thông tin ngành nghề từ bảng Professions
            foreach (var profession in professions)
            {
                var professionDetail = await _professionRepository.GetById(p => p.ProfessionId == profession.ProfessionId);
                profession.Profession = professionDetail;
            }

            var professionDtos = professions.Select(profession => new GroupIdeaOfSupervisorProfessionDto
            {
                GroupIdea = new GroupIdeaOfSupervisorDto()
                {
                    GroupIdeaID = profession.GroupIdeaId,
                },
                Profession = new ProfessionDto()
                {
                    ProfessionID = profession.ProfessionId,
                    ProfessionFullName = profession.Profession.ProfessionFullName,
                },
                Supervisor = new SupervisorDto()
                {
                    SupervisorID = profession.GroupIdea.SupervisorId,
                },
                Specialty = new SpecialtyDto()
                {
                    SpecialtyID = profession.SpecialtyId.Value,
                }
            }).ToList();

            // Gán danh sách ngành nghề vào GroupIdeasOfSupervisor
            groupIdea.GroupIdeaOfSupervisorProfessions = professions.ToList();
            var result = new GroupIdeaOfSupervisorDto()
            {
                GroupIdeaID = groupIdea.GroupIdeaId,
                Supervisor = new SupervisorDto()
                {
                    SupervisorID = groupIdea.SupervisorId,
                },
                GroupIdeaOfSupervisorProfessions = professionDtos,
                ProjectEnglishName = groupIdea.ProjectEnglishName,
                ProjectVietNameseName = groupIdea.ProjectVietNameseName,
                Abrrevation = groupIdea.Abbreviation,
                Description = groupIdea.Description,
                ProjectTags = groupIdea.ProjectTags,
                Semester = new SemesterDto()
                {
                    SemesterID = groupIdea.SemesterId,
                },
                NumberOfMember = groupIdea.NumberOfMember,
                MaxMember = groupIdea.MaxMember,
                CreatedAt = groupIdea.CreatedAt,
                IsActive = groupIdea.IsActive,
            };
            return new ApiSuccessResult<GroupIdeaOfSupervisorDto>(result);
        }

        public async Task<ApiResult<List<GroupIdeaOfSupervisorProfessionDto>>> getAllProfessionSpecialyByGroupIdeaID(int groupIdeaID)
        {
            var groupIdeas = await _groupIdeaOfSupervisorProfessionRepository.GetByCondition(gp => gp.GroupIdeaId == groupIdeaID);

            if (groupIdeas == null || !groupIdeas.Any())
            {
                return new ApiSuccessResult<List<GroupIdeaOfSupervisorProfessionDto>>(new List<GroupIdeaOfSupervisorProfessionDto>());
            }

            var result = groupIdeas.Select(gp => new GroupIdeaOfSupervisorProfessionDto()
            {
                Profession = new ProfessionDto()
                {
                    ProfessionFullName = gp.Profession?.ProfessionFullName ?? ""
                },
                Specialty = new SpecialtyDto()
                {
                    SpecialtyFullName = gp.Specialty?.SpecialtyFullName ?? ""
                }
            }).ToList();

            return new ApiSuccessResult<List<GroupIdeaOfSupervisorProfessionDto>>(result);
        }

        public async Task<ApiResult<GroupIdeaOfSupervisorDto>> GetGroupIdeaOfSupervisorByGroupIdeaId(int groupIdeaId)
        {
            var groupIdea = await _supervisorGroupIdeaReporitory.GetById(g => g.GroupIdeaId == groupIdeaId);

            if (groupIdea == null)
            {
                return new ApiErrorResult<GroupIdeaOfSupervisorDto>("Không tìm thấy Group Idea.");
            }

            // Lấy danh sách ngành nghề và chuyên ngành liên quan đến GroupIdea_ID
            var groupIdeaProfessions = await _groupIdeaOfSupervisorProfessionRepository.GetByCondition(gp => gp.GroupIdeaId == groupIdeaId);

            var professionsWithSpecialties = new List<GroupIdeaOfSupervisorProfession>();

            foreach (var groupIdeaProfession in groupIdeaProfessions)
            {
                // Lấy thông tin ngành nghề từ bảng Professions
                var profession = await _professionRepository.GetById(p => p.ProfessionId == groupIdeaProfession.ProfessionId);
                if (profession != null)
                {
                    groupIdeaProfession.Profession = profession;
                }

                // Lấy thông tin chuyên ngành từ bảng Specialties
                var specialty = await _specialtyRepository.GetById(s => s.SpecialtyId == groupIdeaProfession.SpecialtyId);
                if (specialty != null)
                {
                    groupIdeaProfession.Specialty = specialty;
                }

                professionsWithSpecialties.Add(groupIdeaProfession);
            }

            groupIdea.GroupIdeaOfSupervisorProfessions = professionsWithSpecialties;
            var result = new GroupIdeaOfSupervisorDto()
            {
                GroupIdeaID = groupIdeaId,
                ProjectEnglishName = groupIdea.ProjectEnglishName ?? "",
                ProjectVietNameseName = groupIdea.ProjectVietNameseName ?? "",
                Abrrevation = groupIdea.Abbreviation ?? "",
                Description = groupIdea.Description ?? "",
                CreatedAt = groupIdea.CreatedAt,
                Supervisor = new SupervisorDto()
                {
                    SupervisorID = groupIdea.SupervisorId,
                },
                //Specialty = new SpecialtyDto()
                //{
                //    SpecialtyFullName = groupIdea.
                //},
                //Profession = new ProfessionDto()
                //{
                //    ProfessionFullName
                //}
            };
            return new ApiSuccessResult<GroupIdeaOfSupervisorDto>(result);
        }

        public async Task<ApiResult<List<GroupIdeaOfSupervisorDto>>> GetGroupIdeaOfSupervisorsBySupervisorAndProfession(string supervisorID, int[] pros)
        {
            var groupIdeaIds = (await _groupIdeaOfSupervisorProfessionRepository.GetByCondition(gp =>
            gp.Profession.SupervisorProfessions.Select(p => p.SupervisorId).FirstOrDefault() == supervisorID &&
            pros.Contains(gp.ProfessionId))).Select(gp => gp.GroupIdeaId).ToList();

            if (groupIdeaIds == null || !groupIdeaIds.Any())
            {
                return new ApiErrorResult<List<GroupIdeaOfSupervisorDto>>("Không tìm thấy Group Idea nào.");
            }

            // Lấy danh sách GroupIdeasOfSupervisor theo danh sách GroupIdea_ID đã lấy được
            var groupIdeas = (await _supervisorGroupIdeaReporitory
                .GetByCondition(g => groupIdeaIds.Contains(g.GroupIdeaId) && g.IsActive && g.DeletedAt == null))
                .Select(g => new GroupIdeasOfSupervisor
                {
                    GroupIdeaId = g.GroupIdeaId,
                    ProjectEnglishName = g.ProjectEnglishName
                }).ToList();
            var result = new List<GroupIdeaOfSupervisorDto>();
            foreach (var groupIdea in groupIdeas)
            {
                result.Add(new GroupIdeaOfSupervisorDto()
                {
                    GroupIdeaID = groupIdea.GroupIdeaId,
                    ProjectEnglishName = groupIdea.ProjectEnglishName,
                });
            }
            return new ApiSuccessResult<List<GroupIdeaOfSupervisorDto>>(result);
        }

        public async Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorDto>)>> getGroupIdeaOfSupervisorWithPaging(int pageNumber, string supervisorId)
        {
            List<GroupIdeaOfSupervisorDto> listGroupIdea = new List<GroupIdeaOfSupervisorDto>();
            var supervisors = await _supervisorRepository.GetById(sp => sp.SupervisorId == supervisorId);
            var groupIdeas = (await _groupIdeaOfSupervisorProfessionRepository
                        .GetByCondition(gp => gp.ProfessionId == supervisors.SupervisorProfessions.Select(s => s.ProfessionId).FirstOrDefault()))
                        .Select(gp => gp.GroupIdeaId);
            // Tính tổng số bản ghi
            int totalRecord = (await _supervisorGroupIdeaReporitory
                .GetByCondition(g => g.IsActive == true && g.DeletedAt == null && g.Supervisor.IsActive == true &&
                    g.SupervisorId != supervisorId && groupIdeas.Contains(g.GroupIdeaId))).Count();
            if (totalRecord == 0)
            {
                return new ApiSuccessResult<(int, int, List<GroupIdeaOfSupervisorDto>)>((0, 0, listGroupIdea));
            }

            // Tính phân trang
            int[] pagingQueryResult = PagingQuery(totalRecord, pageNumber);
            int recordSkippedBefore = pagingQueryResult[2];
            int recordSkippedLater = pagingQueryResult[3];

            // Lấy dữ liệu có phân trang
            var supervisorGroupIdeas = (await _supervisorGroupIdeaReporitory.GetByCondition(g => g.IsActive == true && g.DeletedAt == null && g.Supervisor.IsActive == true &&
                   g.SupervisorId != supervisorId && groupIdeas.Contains(g.GroupIdeaId)))
               .OrderByDescending(g => g.CreatedAt)
                .Skip(recordSkippedBefore)
                .Take(recordSkippedLater - recordSkippedBefore)
                .Select(g => new GroupIdeasOfSupervisor
                {
                    GroupIdeaId = g.GroupIdeaId,
                    ProjectEnglishName = g.ProjectEnglishName ?? "",
                    ProjectVietNameseName = g.ProjectVietNameseName ?? "",
                    Abbreviation = g.Abbreviation ?? "",
                    Description = g.Description ?? "",
                    ProjectTags = g.ProjectTags ?? "",
                    NumberOfMember = g.NumberOfMember,
                    IsActive = g.IsActive,
                    CreatedAt = g.CreatedAt ?? DateTime.MinValue,
                    Supervisor = new Supervisor
                    {
                        SupervisorId = g.SupervisorId ?? ""
                    }
                }).ToList();
            var result = new List<GroupIdeaOfSupervisorDto>();
            foreach (var idea in supervisorGroupIdeas)
            {
                result.Add(new GroupIdeaOfSupervisorDto()
                {
                    GroupIdeaID = idea.GroupIdeaId,
                    ProjectEnglishName = idea.ProjectEnglishName ?? "",
                    ProjectVietNameseName = idea.ProjectVietNameseName ?? "",
                    Abrrevation = idea.Abbreviation ?? "",
                    Description = idea.Description ?? "",
                    ProjectTags = idea.ProjectTags ?? "",
                    NumberOfMember = idea.NumberOfMember,
                    IsActive = idea.IsActive,
                    CreatedAt = idea.CreatedAt ?? DateTime.MinValue,
                    Supervisor = new SupervisorDto
                    {
                        SupervisorID = idea.SupervisorId ?? ""
                    }
                });
            }
            return new ApiSuccessResult<(int, int, List<GroupIdeaOfSupervisorDto>)>((pagingQueryResult[0], pagingQueryResult[1], result));
        }
        private int[] PagingQuery(int totalRecord, int pageNumber, int pageSize = 10)
        {
            // Tổng số trang (làm tròn lên)
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);

            // Nếu trang yêu cầu nhỏ hơn 1, đặt về 1
            if (pageNumber < 1) pageNumber = 1;
            // Nếu trang yêu cầu lớn hơn tổng số trang, đặt về trang cuối cùng
            if (pageNumber > totalPage) pageNumber = totalPage;

            // Tính toán giới hạn bản ghi
            int recordSkippedBefore = (pageNumber - 1) * pageSize;
            int recordSkippedLater = recordSkippedBefore + pageSize;
            return new int[] { totalPage, pageNumber, recordSkippedBefore, recordSkippedLater };
        }

        public Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorDto>)>> getGroupIdeaOfSupervisorWithPagingForStudent(int pageNumber, int professionID)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<GroupIdeaOfSupervisorDto>>> GetGroupIdeaRegistedOfSupervisor(string supervisorID)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<GroupIdeaOfSupervisorProfessionDto>>> GetGroupIdeasBySupervisorID(string SupervisorID)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<List<GroupIdeaOfSupervisorDto>>> GetGroupIdeasBySupervisorIDFilterByStatusandSearchText(string SupervisorID, int filterStatus, string query)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<(int, int, List<GroupIdeaOfSupervisorWithRowNum>)>> GetListIdeaOfSupervisorForPaging(int pageNumber, string SupervisorID, int filterStatus, string query)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateAllIdea(GroupIdeaOfSupervisorDto groupIdea, int semesterId)
        {
            throw new NotImplementedException();
        }

        public Task<ApiResult<bool>> UpdateStatusOfIdea(int ideaid, bool status)
        {
            throw new NotImplementedException();
        }
    }
}