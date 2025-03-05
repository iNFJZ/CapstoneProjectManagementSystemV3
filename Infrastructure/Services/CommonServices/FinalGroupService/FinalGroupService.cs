using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.StudentDto;
using Infrastructure.Repositories.FinalGroupRepository;
using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.FinalGroupService
{
    public class FinalGroupService : IFinalGroupService
    {
        private readonly IFinalGroupRepository _finalGroupRepository;
        public FinalGroupService(IFinalGroupRepository finalGroupRepository)
        {
           _finalGroupRepository = finalGroupRepository;
            //them vao sau 
        }

        public Task<ApiResult<bool>> AddRegisteredGroupToFinalGroup(RegisteredGroupRequest registeredGroupRequest, string groupName)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<bool>> ConfirmFinalReport(FinalGroupDto finalGroup)
        {

            await _finalGroupRepository.ConfirmFinalReport(finalGroup);
            return new ApiSuccessResult<bool>(true);    
        }

        public async Task<ApiResult<bool>> CreateFinalGroup(int semesterId, int professionId, int specilatyId, string groupName, string englishName, string abbreviation, string vietnameseName, int maxMember, int numberOfMember)
        {
            var finalGroup = new FinalGroup
            {
                ProfessionId = professionId,
                SpecialtyId = specilatyId,
                GroupName = groupName,
                ProjectEnglishName = englishName,
                ProjectVietNameseName = vietnameseName,
                Abbreviation = abbreviation,
                SemesterId = semesterId,
                MaxMember = maxMember,
                NumberOfMember = numberOfMember
            };
            await _finalGroupRepository.CreateAsync(finalGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteFinalGroup(int finalGroupId)
        {
            List<Expression<Func<FinalGroup, bool>>> expressions = new List<Expression<Func<FinalGroup, bool>>>();
            expressions.Add(fg => fg.FinalGroupId == finalGroupId);
            expressions.Add(fg => fg.DeletedAt == null);
            var finalGroup = await _finalGroupRepository.GetByConditionId(expressions);
            if(finalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            finalGroup.DeletedAt = DateTime.Now;
            await _finalGroupRepository.UpdateAsync(finalGroup);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<FinalGroupDto>>> getAllFinalGroups(int semester_Id)
        {
            var finalGroups =  await _finalGroupRepository.getAllFinalGroups(semester_Id);
            return new ApiSuccessResult<List<FinalGroupDto>>(finalGroups);
        }

        public async Task<ApiResult<FinalGroupDto>> getFinalGroupById(int id)
        {
            var finalGroup = await _finalGroupRepository.getFinalGroupById(id);
            return new ApiSuccessResult<FinalGroupDto>(finalGroup);
        }

        public async Task<ApiResult<FinalGroupDto>> GetFinalGroupByStudentIsLeader(string studentId, int semesterId)
        {
            var finalGroup = await _finalGroupRepository.GetFinalGroupByStudentIsLeader(studentId,semesterId);
            return new ApiSuccessResult<FinalGroupDto>(finalGroup);
        }

        public async Task<ApiResult<FinalGroupDto>> getFinalGroupDetailForSupervisor(int finalGroupId)
        {
            var finalGroup = await _finalGroupRepository.GetFinalGroupDetailForSupervisor(finalGroupId);
            return new ApiSuccessResult<FinalGroupDto>(finalGroup);
        }

        public async Task<ApiResult<List<FinalGroupDto>>> GetFullMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber)
        {
            var finalGroups = await _finalGroupRepository.GetFullMemberFinalGroupSearchList(semester_Id,profession_Id,specialty_Id,searchText,offsetNumber,fetchNumber);
            return new ApiSuccessResult<List<FinalGroupDto>>(finalGroups);
        }

        public async Task<ApiResult<List<FinalGroupDto>>> GetLackOfMemberFinalGroupSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber)
        {
            var finalGroups = await _finalGroupRepository.GetLackOfMemberFinalGroupSearchList(semester_Id, profession_Id, specialty_Id, searchText, offsetNumber, fetchNumber);
            return new ApiSuccessResult<List<FinalGroupDto>>(finalGroups);
        }

        public async Task<ApiResult<string>> GetLatestGroupName(string codeOfGroupName)
        {
            var finalGroup = await _finalGroupRepository.GetLatestGroupName(codeOfGroupName);
            return new ApiSuccessResult<string>(finalGroup);
        }

        public async Task<ApiResult<List<StudentDto>>> GetListCurrentMemberHaveFinalGroupByGroupName(string groupName, int semesterId)
        {
            var finalGroups = await _finalGroupRepository.GetListCurrentMemberHaveFinalGroupByGroupName(groupName, semesterId);
            return new ApiSuccessResult<List<StudentDto>>(finalGroups);
        }

        public async Task<ApiResult<List<FinalGroupDto>>> GetListFinalGroupBySupervisorID(string supervisorId, int semesterId)
        {
            var finalGroups = await _finalGroupRepository.GetListFinalGroupBySupervisorID(supervisorId, semesterId);
            return new ApiSuccessResult<List<FinalGroupDto>>(finalGroups);
        }

        public async Task<ApiResult<List<StudentDto>>> GetListMemberByFinalGroupId(int finalGroupId)
        {
            var finalGroups = await _finalGroupRepository.GetListMemberByFinalGroupId(finalGroupId);
            return new ApiSuccessResult<List<StudentDto>>(finalGroups);
        }
        public async Task<ApiResult<int>> GetMaxMemberOfFinalGroupByGroupName(string groupName, int semesterId)
        {
            var finalGroups = await _finalGroupRepository.GetMaxMemberOfFinalGroupByGroupName(groupName, semesterId);
            return new ApiSuccessResult<int>(finalGroups);
        }

        public async Task<ApiResult<FinalGroupDto>> GetOldTopicByGroupName(int finalGroupId)
        {
            var finalGroups = await _finalGroupRepository.GetOldTopicByFinalGroupId(finalGroupId);
            return new ApiSuccessResult<FinalGroupDto>(finalGroups);
        }

        public async Task<ApiResult<bool>> UpdateGroupName(int groupId, string groupName)
        {
            List<Expression<Func<FinalGroup, bool>>> expressions = new List<Expression<Func<FinalGroup, bool>>>();
            expressions.Add(fg => fg.FinalGroupId == groupId);
            expressions.Add(fg => fg.DeletedAt == null);
            var finalGroup = await _finalGroupRepository.GetByConditionId(expressions);
            if (finalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            finalGroup.GroupName = groupName;
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateNewTopicForFinalGroup(ChangeTopicRequest changeTopicRequest)
        {
            List<Expression<Func<FinalGroup, bool>>> expressions = new List<Expression<Func<FinalGroup, bool>>>();
            expressions.Add(fg => fg.FinalGroupId == changeTopicRequest.FinalGroupId);
            var finalGroup = await _finalGroupRepository.GetByConditionId(expressions);
            if (finalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            finalGroup.ProjectEnglishName = changeTopicRequest.NewTopicNameEnglish;
            finalGroup.ProjectVietNameseName = changeTopicRequest.NewTopicNameVietNamese;
            finalGroup.Abbreviation = changeTopicRequest.NewAbbreviation;
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateNumberOfMemberWhenAdd(int groupId)
        {
            List<Expression<Func<FinalGroup, bool>>> expressions = new List<Expression<Func<FinalGroup, bool>>>();
            expressions.Add(fg => fg.FinalGroupId == groupId);
            expressions.Add(fg => fg.DeletedAt == null);
            var finalGroup = await _finalGroupRepository.GetByConditionId(expressions);
            if (finalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            finalGroup.NumberOfMember += 1 ;
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateNumberOfMemberWhenRemove(int groupId)
        {
            List<Expression<Func<FinalGroup, bool>>> expressions = new List<Expression<Func<FinalGroup, bool>>>();
            expressions.Add(fg => fg.FinalGroupId == groupId);
            expressions.Add(fg => fg.DeletedAt == null);
            var finalGroup = await _finalGroupRepository.GetByConditionId(expressions);
            if (finalGroup == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            finalGroup.NumberOfMember -= 1;
            return new ApiSuccessResult<bool>(true);
        }
    }
}
