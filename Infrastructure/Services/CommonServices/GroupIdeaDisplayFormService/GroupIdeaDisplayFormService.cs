using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SpecialtyRepository;
using Infrastructure.Repositories.StudentGroupIdeaRepository;
using Infrastructure.Repositories.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.GroupIdeaDisplayFormService
{
    public class GroupIdeaDisplayFormService : IGroupIdeaDisplayFormService
    {
        private readonly IUserRepository _userRepository;
        private readonly IStudentGroupIdeaRepository _studentGroupIdeaRepository;
        private readonly IProfessionRepository _professionRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        public GroupIdeaDisplayFormService(IUserRepository userRepository,
            IStudentGroupIdeaRepository studentGroupIdeaRepository,
            IProfessionRepository professionRepository,
            ISpecialtyRepository specialtyRepository,
            IGroupIdeaRepository groupIdeaRepository)
        {
            _userRepository = userRepository;
            _studentGroupIdeaRepository = studentGroupIdeaRepository;
            _professionRepository = professionRepository;
            _specialtyRepository = specialtyRepository;
            _groupIdeaRepository = groupIdeaRepository;
        }

        public async Task<ApiResult<List<GroupIdeaDisplayForm>>> ConvertFromGroupIdeaList(List<GroupIdea> groupList)
        {
            List<GroupIdeaDisplayForm> groupIdeaDisplayForms = new List<GroupIdeaDisplayForm>();
            if (groupList == null) return new ApiSuccessResult<List<GroupIdeaDisplayForm>>(groupIdeaDisplayForms);
            else
            {
                foreach (GroupIdea item in groupList)
                {
                    string[] projectTagsArray = item.ProjectTags.Trim().Split(',');
                    List<string> projectTagList = new List<string>();
                    foreach (var tag in projectTagsArray)
                    {
                        projectTagList.Add(tag);
                    };
                    var studentGroupIdea = await _studentGroupIdeaRepository.GetById(sg => sg.GroupIdeaId == item.GroupIdeaId && sg.Status == 1 && sg.DeletedAt == null);
                    var user = await _userRepository.GetById(u => u.UserId == studentGroupIdea.StudentId);
                    var profession = await _professionRepository.GetById(p => p.ProfessionId == item.Profession.ProfessionId && p.DeletedAt == null);
                    var specialty = await _specialtyRepository.GetById(s => s.SpecialtyId == item.Specialty.SpecialtyId && s.DeletedAt == null);
                    GroupIdeaDisplayForm groupIdeaDisplayForm = new GroupIdeaDisplayForm()
                    {
                        GroupIdeaID = item.GroupIdeaId,
                        ProjectEnglishName = item.ProjectEnglishName,
                        LeaderFullName = user.FullName,
                        Avatar = user.Avatar,
                        CreatedAt = item.CreatedAt.ToString(),
                        ProjectTags = projectTagList,
                        ProfessionFullName = profession.ProfessionFullName,
                        SpecialtyFullName = specialty.SpecialtyFullName,
                        Description = item.Description,
                        AvailableSlot = (item.MaxMember - item.NumberOfMember),
                        Semester_Id = item.Semester.SemesterId
                    };
                    groupIdeaDisplayForms.Add(groupIdeaDisplayForm);
                }
                return new ApiSuccessResult<List<GroupIdeaDisplayForm>>(groupIdeaDisplayForms);
            }
        }
    }
}
