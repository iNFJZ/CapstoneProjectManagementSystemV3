using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Entities.Dto.ViewModel.StaffViewModel;
using Infrastructure.Repositories.GroupIdeaRepository;
using Infrastructure.Repositories.SpecialtyRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SpecialtyService
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IGroupIdeaRepository _groupIdeaRepository;
        public SpecialtyService(ISpecialtyRepository specialtyRepository,
            IGroupIdeaRepository groupIdeaRepository)
        {
            _specialtyRepository = specialtyRepository;
            _groupIdeaRepository = groupIdeaRepository;
        }

        public async Task<ApiResult<int>> AddSpecialtyThenReturnId(SpecialtyDto specialty, int semesterId)
        {
            var newSpecialty = new Specialty()
            {
                SpecialtyAbbreviation = specialty.SpecialtyAbbreviation,
                SpecialtyFullName = specialty.SpecialtyFullName,
                ProfessionId = specialty.Profession.ProfessionID,
                MaxMember = specialty.MaxMember,
                CodeOfGroupName = specialty.CodeOfGroupName,
                SemesterId = semesterId,
            };
            await _specialtyRepository.CreateAsync(newSpecialty);
            return new ApiSuccessResult<int>(newSpecialty.SpecialtyId);
        }

        public async Task<ApiResult<List<SpecialtyDto>>> getAllSpecialty(int semesterId)
        {
            List<Expression<Func<Specialty, bool>>> expressions = new List<Expression<Func<Specialty, bool>>>();
            expressions.Add(s => s.SemesterId == semesterId);
            expressions.Add(s => s.DeletedAt == null);
            var specialties = await _specialtyRepository.GetByConditions(expressions);
            var result = new List<SpecialtyDto>();
            foreach (var specialty in specialties)
            {
                result.Add(new SpecialtyDto()
                {
                    SpecialtyID = specialty.SpecialtyId,
                    SpecialtyAbbreviation = specialty.SpecialtyAbbreviation,
                    SpecialtyFullName = specialty.SpecialtyFullName,
                    MaxMember = specialty.MaxMember.Value,
                    CodeOfGroupName = specialty.CodeOfGroupName,
                    Profession = new ProfessionDto()
                    {
                        ProfessionID = specialty.ProfessionId
                    },
                    Semester = new SemesterDto()
                    {
                        SemesterID = specialty.SemesterId.Value
                    }
                });
            }
            return new ApiSuccessResult<List<SpecialtyDto>>(result);
        }

        public (int, int, List<SpecialtyWithRowNum>) GetAllSpecialtyForPaging(int semesterId, int pageNumber, string query)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResult<string>> GetCodeOfGroupNameByGroupIdeaId(int groupIdeaId)
        {
            List<Expression<Func<GroupIdea, bool>>> expressions = new List<Expression<Func<GroupIdea, bool>>>();
            expressions.Add(s => s.GroupIdeaId == groupIdeaId);
            expressions.Add(s => s.DeletedAt == null);
            var groupIdea = await _groupIdeaRepository.GetByConditionId(expressions);
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(s => s.SpecialtyId == groupIdea.SpecialtyId);
            specialtyExpression.Add(s => s.DeletedAt == null);
            var specialty = await _specialtyRepository.GetByConditionId(specialtyExpression);
            return new ApiSuccessResult<string>(specialty.CodeOfGroupName);
        }

        public async Task<ApiResult<List<SpecialtyDto>>> getSpecialtiesByProfessionId(int professionId, int semesterId)
        {
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(s => s.ProfessionId == professionId);
            specialtyExpression.Add(s => s.SemesterId == semesterId);
            specialtyExpression.Add(s => s.DeletedAt == null);
            var specialties = await _specialtyRepository.GetByConditions(specialtyExpression);
            var result = new List<SpecialtyDto>();
            foreach (var specialty in specialties)
            {
                result.Add(new SpecialtyDto()
                {
                    SpecialtyID = specialty.SpecialtyId,
                    SpecialtyAbbreviation = specialty.SpecialtyAbbreviation,
                    SpecialtyFullName = specialty.SpecialtyFullName,
                    MaxMember = specialty.MaxMember.Value,
                    CodeOfGroupName = specialty.CodeOfGroupName,
                    Profession = new ProfessionDto()
                    {
                        ProfessionID = specialty.ProfessionId
                    },
                    Semester = new SemesterDto()
                    {
                        SemesterID = specialty.SemesterId.Value
                    }
                });
            }
            return new ApiSuccessResult<List<SpecialtyDto>>(result);
        }

        public async Task<ApiResult<SpecialtyDto>> getSpecialtyById(int specialtyId)
        {
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(s => s.SpecialtyId == specialtyId);
            specialtyExpression.Add(s => s.DeletedAt == null);
            var specialty = await _specialtyRepository.GetByConditionId(specialtyExpression);
            var result = new SpecialtyDto()
            {
                SpecialtyID = specialty.SpecialtyId,
                SpecialtyAbbreviation = specialty.SpecialtyAbbreviation,
                SpecialtyFullName = specialty.SpecialtyFullName,
                MaxMember = specialty.MaxMember.Value,
                CodeOfGroupName = specialty.CodeOfGroupName,
                Profession = new ProfessionDto()
                {
                    ProfessionID = specialty.ProfessionId
                },
                Semester = new SemesterDto()
                {
                    SemesterID = specialty.SemesterId.Value
                }
            };
            return new ApiSuccessResult<SpecialtyDto>(result);
        }

        public async Task<ApiResult<SpecialtyDto>> GetSpecialtyByName(string specialtyFullname, int semesterId)
        {
            string normalizedFullName = specialtyFullname.ToUpper().Replace(" ", "");
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(s => s.SemesterId == semesterId);
            specialtyExpression.Add(s => s.DeletedAt == null);
            specialtyExpression.Add(s => EF.Functions.Like(s.SpecialtyFullName.Replace(" ", "").ToUpper(), normalizedFullName));
            var specialty = await _specialtyRepository.GetByConditionId(specialtyExpression);
            var result = new SpecialtyDto()
            {
                SpecialtyID = specialty.SpecialtyId
            };
            return new ApiSuccessResult<SpecialtyDto>(result);
        }

        public async Task<ApiResult<SpecialtyDto>> GetSpecialtyByName(string specialtyFullname, int semesterId, int professionId)
        {
            string normalizedFullName = specialtyFullname.ToUpper().Replace(" ", "");
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(s => s.SemesterId == semesterId);
            specialtyExpression.Add(s => s.ProfessionId == professionId);
            specialtyExpression.Add(s => s.DeletedAt == null);
            specialtyExpression.Add(s => EF.Functions.Like(s.SpecialtyFullName.Replace(" ", "").ToUpper(), normalizedFullName));
            var specialty = await _specialtyRepository.GetByConditionId(specialtyExpression);
            var result = new SpecialtyDto()
            {
                SpecialtyID = specialty.SpecialtyId
            };
            return new ApiSuccessResult<SpecialtyDto>(result);
        }

        public async Task<ApiResult<bool>> RemoveSpecialty(int id)
        {
            Expression<Func<Specialty, bool>> expression = s => s.SpecialtyId == id;
            var specialty = await _specialtyRepository.GetById(expression);
            await _specialtyRepository.DeleteAsync(specialty);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateSpecialty(SpecialtyDto specialty)
        {
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(s => s.SpecialtyId == specialty.SpecialtyID);
            specialtyExpression.Add(s => s.DeletedAt == null);
            var specialtyUpdate = await _specialtyRepository.GetByConditionId(specialtyExpression);
            specialtyUpdate.SpecialtyAbbreviation = specialty.SpecialtyAbbreviation;
            specialtyUpdate.SpecialtyFullName = specialty.SpecialtyFullName;
            specialtyUpdate.ProfessionId = specialty.Profession.ProfessionID;
            specialtyUpdate.MaxMember = specialty.MaxMember;
            specialtyUpdate.CodeOfGroupName = specialty.CodeOfGroupName;
            specialtyUpdate.UpdatedAt = DateTime.Now;
            await _specialtyRepository.UpdateAsync(specialtyUpdate);
            return new ApiSuccessResult<bool>(true);
        }

        public Task<ApiResult<bool>> UpdateSpecialtyV2(SpecialtyDto specialty)
        {
            throw new NotImplementedException(); // chưa làm được vì bảng configuruationScanPro
        }
    }
}
