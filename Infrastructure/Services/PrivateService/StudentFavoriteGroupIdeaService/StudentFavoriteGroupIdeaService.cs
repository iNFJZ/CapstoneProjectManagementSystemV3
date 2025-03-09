using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.Student_FavoriteGroupIdeaRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.Student_FavoriteGroupIdeaService
{
    public class StudentFavoriteGroupIdeaService : IStudentFavoriteGroupIdeaService
    {
        private readonly IStudentFavoriteGroupIdeaRepository _studentFavoriteGroupIdeaRepository;
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        public StudentFavoriteGroupIdeaService(IStudentFavoriteGroupIdeaRepository studentFavoriteGroupIdeaRepository,
            IGroupIdeaRepository groupIdeaRepository)
        {
            _studentFavoriteGroupIdeaRepository = studentFavoriteGroupIdeaRepository;
            _groupIdeaRepository = groupIdeaRepository;
        }

        public async Task<ApiResult<bool>> AddRecord(string studentId, int groupId)
        {
            try
            {
                var newRecord = new StudentFavoriteGroupIdea
                {
                    StudentId = studentId,
                    GroupIdeaId = groupId
                };

                await _studentFavoriteGroupIdeaRepository.CreateAsync(newRecord);

                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Lỗi khi thêm bản ghi: {ex.Message}");
            }
        }

        public async Task<ApiResult<bool>> DeleteAllRecordOfAGroupIdea(int groupIdeaId)
        {
            try
            {
                var records = (await _studentFavoriteGroupIdeaRepository
                    .GetByCondition(s => s.GroupIdeaId == groupIdeaId && s.DeletedAt == null))
                    .ToList();

                if (!records.Any())
                {
                    return new ApiErrorResult<bool>("Không tìm thấy bản ghi nào để xóa.");
                }

                foreach (var record in records)
                {
                    record.DeletedAt = DateTime.UtcNow;
                    await _studentFavoriteGroupIdeaRepository.UpdateAsync(record);
                }
                return new ApiSuccessResult<bool>(true);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<bool>($"Lỗi khi xóa bản ghi: {ex.Message}");
            }
        }

        public async Task<ApiResult<bool>> DeleteRecord(string studentId, int groupIdeaId)
        {
            var record = await _studentFavoriteGroupIdeaRepository.GetById(s => s.StudentId == studentId && s.GroupIdeaId == groupIdeaId && s.DeletedAt == null);
            record.DeletedAt = DateTime.Now;
            await _studentFavoriteGroupIdeaRepository.UpdateAsync(record);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<StudentFavoriteGroupIdeaDto>>> GetFavoriteIdeaListByStudentId(string studentId)
        {
            try
            {
                var favoriteIdeas = await _studentFavoriteGroupIdeaRepository.GetByCondition(s => s.StudentId == studentId && s.DeletedAt == null);

                if (!favoriteIdeas.Any())
                {
                    return new ApiErrorResult<List<StudentFavoriteGroupIdeaDto>>("Không tìm thấy ý tưởng yêu thích nào.");
                }

                var groupIdeaIds = favoriteIdeas.Select(s => s.GroupIdeaId).ToList();
                var groupIdeas = await _groupIdeaRepository.GetByCondition(g => groupIdeaIds.Contains(g.GroupIdeaId));

                // Map GroupIdea vào từng StudentFavoriteGroupIdea
                foreach (var idea in favoriteIdeas)
                {
                    idea.GroupIdea = groupIdeas.FirstOrDefault(g => g.GroupIdeaId == idea.GroupIdeaId);
                }
                var result = new List<StudentFavoriteGroupIdeaDto>();
                foreach (var idea in favoriteIdeas)
                {
                    result.Add(new StudentFavoriteGroupIdeaDto()
                    {
                        StudentID = idea.StudentId,
                        GroupIdeaID = idea.GroupIdeaId,
                        GroupIdea = new GroupIdeaDto()
                        {
                            GroupIdeaID = idea.GroupIdeaId,
                        }
                    });
                }
                return new ApiSuccessResult<List<StudentFavoriteGroupIdeaDto>>(result);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<List<StudentFavoriteGroupIdeaDto>>($"Lỗi khi lấy danh sách: {ex.Message}");
            }
        }
        public async Task<ApiResult<StudentFavoriteGroupIdeaDto>> GetRecord(string studentId, int groupId)
        {
            var record = await _studentFavoriteGroupIdeaRepository.GetById(s => s.StudentId == studentId && s.GroupIdeaId == groupId && s.DeletedAt == null);
            if (record == null)
            {
                return new ApiErrorResult<StudentFavoriteGroupIdeaDto>("Không tìm thấy bản ghi phù hợp");
            }
            var result = new StudentFavoriteGroupIdeaDto()
            {
                StudentID = record.StudentId,
                GroupIdeaID = record.GroupIdeaId,
                GroupIdea = new GroupIdeaDto()
                {
                    GroupIdeaID = record.GroupIdeaId,
                }
            };
            return new ApiSuccessResult<StudentFavoriteGroupIdeaDto>(result);
        }
    }
}
