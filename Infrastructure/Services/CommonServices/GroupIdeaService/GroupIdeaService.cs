﻿using DocumentFormat.OpenXml.Office2010.Excel;
using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.GroupIdeaRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.GroupIdeaService
{
    public class GroupIdeaService : IGroupIdeaService
    {
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        public GroupIdeaService(IGroupIdeaRepository groupIdeaRepository)
        {
            _groupIdeaRepository = groupIdeaRepository;
        }

        public async Task<ApiResult<List<GroupIdea>>> GetGroupIdeaSearchList(int semester_Id, int profession_Id, int specialty_Id, string searchText, int offsetNumber, int fetchNumber)
        {
            string semesterIdToString = semester_Id.ToString();
            string professionIdToString = profession_Id.ToString();
            string specialtyIdToString = specialty_Id.ToString();
            if (semesterIdToString.Equals("0"))
            {
                semesterIdToString = "";
            }
            if (professionIdToString.Equals("0"))
            {
                professionIdToString = "";
            }
            if (specialtyIdToString.Equals("0"))
            {
                specialtyIdToString = "";
            }
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = String.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var result = await _groupIdeaRepository.GetGroupIdeaSearchList(semesterIdToString, professionIdToString, specialtyIdToString, searchText, offsetNumber, fetchNumber);
            return new ApiSuccessResult<List<GroupIdea>>(result);
        }

        public async Task<ApiResult<List<GroupIdea>>> GetGroupIdeaSearchList_2(int semester_Id, int profession_Id, int specialty_Id, string searchText, string studentId, int offsetNumber, int fetchNumber)
        {
            string semesterIdToString = semester_Id.ToString();
            string professionIdToString = profession_Id.ToString();
            string specialtyIdToString = specialty_Id.ToString();
            if (semesterIdToString.Equals("0"))
            {
                semesterIdToString = "";
            }
            if (professionIdToString.Equals("0"))
            {
                professionIdToString = "";
            }
            if (specialtyIdToString.Equals("0"))
            {
                specialtyIdToString = "";
            }
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = String.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var result = await _groupIdeaRepository.GetGroupIdeaSearchList_2(semesterIdToString, professionIdToString, specialtyIdToString, searchText ,studentId, offsetNumber, fetchNumber);
            return new ApiSuccessResult<List<GroupIdea>>(result);
        }

        public async Task<ApiResult<int>> getNumberOfResultWhenSearch(int semester_Id, int profession_Id, int specialty_Id, string searchText)
        {
            string semesterIdToString = semester_Id.ToString();
            string professionIdToString = profession_Id.ToString();
            string specialtyIdToString = specialty_Id.ToString();
            if (semesterIdToString.Equals("0"))
            {
                semesterIdToString = "";
            }
            if (professionIdToString.Equals("0"))
            {
                professionIdToString = "";
            }
            if (specialtyIdToString.Equals("0"))
            {
                specialtyIdToString = "";
            }
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = String.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var result = await _groupIdeaRepository.GetNumberOfResultWhenSearchAsync(semesterIdToString, professionIdToString, specialtyIdToString, searchText);
            return new ApiSuccessResult<int>(result);
        }

        public async Task<ApiResult<int>> getNumberOfResultWhenSearch_2(int semester_Id, int profession_Id, int specialty_Id, string searchText, string studentId)
        {
            string semesterIdToString = semester_Id.ToString();
            string professionIdToString = profession_Id.ToString();
            string specialtyIdToString = specialty_Id.ToString();
            if (semesterIdToString.Equals("0"))
            {
                semesterIdToString = "";
            }
            if (professionIdToString.Equals("0"))
            {
                professionIdToString = "";
            }
            if (specialtyIdToString.Equals("0"))
            {
                specialtyIdToString = "";
            }
            if (searchText == null)
            {
                searchText = "";
            }
            else
            {
                searchText = String.Concat("%", searchText.Trim().Replace(" ", "").ToUpper(), "%");
            }
            var result = await _groupIdeaRepository.GetNumberOfResultWhenSearch2(semesterIdToString, professionIdToString, specialtyIdToString, searchText,studentId);
            return new ApiSuccessResult<int>(result);
        }

        public async Task<ApiResult<GroupIdea>> GetGroupIdeaById(int id)
        {
            var result = await _groupIdeaRepository.GetGroupIdeaByIdAsync(id);
            return new ApiSuccessResult<GroupIdea>(result);
        }

        public Task<ApiResult<GroupIdea>> GetAllGroupIdeaById(int Id) // khong hieu nghiep vu 
        {
            throw new NotImplementedException();
        }

        public List<string> ConvertProjectTags(string projectTags)
        {
            string[] projectTagsArray = projectTags.Trim().Split(',');
            List<string> projectTagList = new List<string>();
            foreach (var tag in projectTagsArray)
            {
                projectTagList.Add(tag);
            }
            return projectTagList;
        }

        public async Task<ApiResult<bool>> UpdateNumberOfMemberWhenAdd(int groupIdeaId)
        {
            List<Expression<Func<GroupIdea, bool>>> expressions = new List<Expression<Func<GroupIdea, bool>>>();
            expressions.Add(e => e.GroupIdeaId == groupIdeaId);
            expressions.Add(e => e.DeletedAt == null);
            var findGroupIdea = await _groupIdeaRepository.GetByConditionId(expressions);
            if (findGroupIdea == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findGroupIdea.NumberOfMember += 1;
            await _groupIdeaRepository.UpdateAsync(findGroupIdea);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateNumberOfMemberWhenRemove(int groupIdeaId)
        {
            List<Expression<Func<GroupIdea, bool>>> expressions = new List<Expression<Func<GroupIdea, bool>>>();
            expressions.Add(e => e.GroupIdeaId == groupIdeaId);
            expressions.Add(e => e.DeletedAt == null);
            var findGroupIdea = await _groupIdeaRepository.GetByConditionId(expressions);
            if (findGroupIdea == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findGroupIdea.NumberOfMember -= 1;
            await _groupIdeaRepository.UpdateAsync(findGroupIdea);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> DeleteGroupIdea(int groupIdeaId)
        {
            List<Expression<Func<GroupIdea, bool>>> expressions = new List<Expression<Func<GroupIdea, bool>>>();
            expressions.Add(e => e.GroupIdeaId == groupIdeaId);
            expressions.Add(e => e.DeletedAt == null);
            var findGroupIdea = await _groupIdeaRepository.GetByConditionId(expressions);
            if (findGroupIdea == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findGroupIdea.DeletedAt = DateTime.Now;
            await _groupIdeaRepository.UpdateAsync(findGroupIdea);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateIdea(GroupIdea groupIdea, int semesterId)
        {
            List<Expression<Func<GroupIdea, bool>>> expressions = new List<Expression<Func<GroupIdea, bool>>>();
            expressions.Add(e => e.GroupIdeaId == groupIdea.GroupIdeaId);
            expressions.Add(e => e.DeletedAt == null);
            var findGroupIdea = await _groupIdeaRepository.GetByConditionId(expressions);
            if (findGroupIdea == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findGroupIdea.ProfessionId = groupIdea.ProfessionId;
            findGroupIdea.SpecialtyId = groupIdea.SpecialtyId;
            findGroupIdea.ProjectEnglishName = groupIdea.ProjectEnglishName;
            findGroupIdea.ProjectVietNameseName = groupIdea.ProjectVietNameseName;
            findGroupIdea.Abbreviation = groupIdea.Abbreviation;
            findGroupIdea.Description = groupIdea.Description;
            findGroupIdea.ProjectTags = groupIdea.ProjectTags;
            findGroupIdea.SemesterId = groupIdea.SemesterId;
            findGroupIdea.NumberOfMember = groupIdea.NumberOfMember;
            findGroupIdea.MaxMember = groupIdea.MaxMember;
            findGroupIdea.UpdatedAt = groupIdea.UpdatedAt;
            await _groupIdeaRepository.UpdateAsync(findGroupIdea);
            return new ApiSuccessResult<bool>(true);
        }

        public Task<ApiResult<bool>> UpdateAllIdea(GroupIdea groupIdea, int semesterId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<List<GroupIdea>>> GetGroupIdeasByUserID(string UserID)
        {
            var result = await _groupIdeaRepository.GetGroupIdeasByUserIDAsync(UserID);
            return new ApiSuccessResult<List<GroupIdea>>(result);
        }

        public async Task<ApiResult<bool>> UpdateStatusOfIdea(int ideaid, bool status)
        {
            List<Expression<Func<GroupIdea, bool>>> expressions = new List<Expression<Func<GroupIdea, bool>>>();
            expressions.Add(e => e.GroupIdeaId == ideaid);
            expressions.Add(e => e.DeletedAt == null);
            var findGroupIdea = await _groupIdeaRepository.GetByConditionId(expressions);
            if (findGroupIdea == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            //findGroupIdea.IsActive  = status;
            await _groupIdeaRepository.UpdateAsync(findGroupIdea);
            return new ApiSuccessResult<bool>(true);
        }

        
    }
}
