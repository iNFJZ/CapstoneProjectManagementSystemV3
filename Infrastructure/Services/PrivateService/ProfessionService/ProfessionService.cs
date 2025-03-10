﻿using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Repositories.SpecialtyRepository;
using Infrastructure.Repositories.Supervisor_GroupIdeaReporitory;
using Infrastructure.Repositories.SupervisorProfessionRepository;
using Infrastructure.Repositories.SupervisorRepository;
using Infrastructure.Entities.Dto.ViewModel.StaffViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ProfessionService
{
    public class ProfessionService : IProfessionService
    {
        private readonly IProfessionRepository _professionRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly ISupervisorProfessionRepository _supervisorProfessionRepository;
        private readonly ISemesterRepository _semesterRepository;
        private readonly ISupervisorGroupIdeaReporitory _supervisorGroupIdeaReporitory;
        public ProfessionService(IProfessionRepository professionRepository,
            ISpecialtyRepository specialtyRepository,
            ISupervisorProfessionRepository supervisorProfessionRepository,
            ISemesterRepository semesterRepository,
            ISupervisorGroupIdeaReporitory supervisorGroupIdeaReporitory)
        {
            _professionRepository = professionRepository;
            _specialtyRepository = specialtyRepository;
            _supervisorProfessionRepository = supervisorProfessionRepository;
            _semesterRepository = semesterRepository;
            _supervisorGroupIdeaReporitory = supervisorGroupIdeaReporitory;
        }

        public async Task<ApiResult<int>> AddProfessionThenReturnId(ProfessionDto profession, int semesterId)
        {
            try
            {
                var newProfession = new Profession()
                {
                    ProfessionAbbreviation = profession.ProfessionAbbreviation,
                    ProfessionFullName = profession.ProfessionFullName,
                    SemesterId = profession.Semester.SemesterID
                };
                await _professionRepository.CreateAsync(newProfession);
                return new ApiSuccessResult<int>(newProfession.ProfessionId);
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<int>(ex.ToString());
            }
        }

        public async Task<ApiResult<bool>> DeleteProfession(int id)
        {
            List<Expression<Func<Profession, bool>>> expression = new List<Expression<Func<Profession, bool>>>();
            expression.Add(e => e.ProfessionId == id);
            expression.Add(e => e.DeletedAt == null);
            var result = await _professionRepository.GetByConditionId(expression);
            result.DeletedAt = DateTime.Now;
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<List<ProfessionDto>>> getAllProfession(int semesterId)
        {
            List<Expression<Func<Profession, bool>>> expression = new List<Expression<Func<Profession, bool>>>();
            expression.Add(e => e.SemesterId == semesterId);
            expression.Add(e => e.DeletedAt == null);
            var professionList = await _professionRepository.GetByConditions(expression);
            var result = new List<ProfessionDto>();
            foreach (var profession in professionList)
            {
                result.Add(new ProfessionDto()
                {
                    ProfessionID = profession.ProfessionId,
                    ProfessionAbbreviation = profession.ProfessionAbbreviation,
                    ProfessionFullName = profession.ProfessionFullName,
                    Semester = new SemesterDto()
                    {
                        SemesterID = profession.SemesterId.Value
                    }
                });
            }
            return new ApiSuccessResult<List<ProfessionDto>>(result);
        }

        public async Task<ApiResult<List<ProfessionDto>>> GetAllProfessionWithSpecialty(int semesterId)
        {
            List<Expression<Func<Profession, bool>>> expression = new List<Expression<Func<Profession, bool>>>();
            expression.Add(e => e.SemesterId == semesterId);
            expression.Add(e => e.DeletedAt == null);
            var professions = await _professionRepository.GetByConditions(expression);
            List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
            specialtyExpression.Add(e => e.SemesterId == semesterId);
            specialtyExpression.Add(e => e.DeletedAt == null);
            var specialty = await _specialtyRepository.GetByConditions(specialtyExpression);
            foreach (var profession in professions)
            {
                profession.Specialties = specialty.Where(s => s.ProfessionId == profession.ProfessionId).ToList();
            }
            var result = new List<ProfessionDto>();
            foreach (var profession in professions)
            {
                result.Add(new ProfessionDto()
                {
                    ProfessionID = profession.ProfessionId,
                    ProfessionAbbreviation = profession.ProfessionAbbreviation,
                    ProfessionFullName = profession.ProfessionFullName,
                    Semester = new SemesterDto()
                    {
                        SemesterID = profession.SemesterId.Value,
                    }
                });
            }
            return new ApiSuccessResult<List<ProfessionDto>>(result);
        }

        public async Task<ApiResult<ProfessionDto>> getProfessionById(int professionId)
        {
            List<Expression<Func<Profession, bool>>> expression = new List<Expression<Func<Profession, bool>>>();
            expression.Add(e => e.ProfessionId == professionId);
            expression.Add(e => e.DeletedAt == null);
            var profession = await _professionRepository.GetByConditionId(expression);
            if (profession == null)
            {
                return new ApiErrorResult<ProfessionDto>("Không tìm thấy dữ liệu");
            }
            else
            {
                var result = new ProfessionDto()
                {
                    ProfessionID = profession.ProfessionId,
                    ProfessionAbbreviation = profession.ProfessionAbbreviation,
                    ProfessionFullName = profession.ProfessionFullName,
                    Semester = new SemesterDto()
                    {
                        SemesterID = profession.SemesterId.Value,
                    }
                };
                return new ApiSuccessResult<ProfessionDto>(result);
            }
        }

        public async Task<ApiResult<ProfessionDto>> GetProfessionByName(string professionFullname, int semesterId)
        {
            string normalizedFullName = professionFullname.ToUpper().Replace(" ", "");
            List<Expression<Func<Profession, bool>>> expression = new List<Expression<Func<Profession, bool>>>();
            expression.Add(e => e.SemesterId == semesterId);
            expression.Add(e => EF.Functions.Like(e.ProfessionFullName.Replace(" ", "").ToUpper(), normalizedFullName));
            expression.Add(e => e.DeletedAt == null);
            var profession = await _professionRepository.GetByConditionId(expression);
            if (profession == null)
            {
                return new ApiErrorResult<ProfessionDto>("Không tìm thấy dữ liệu");
            }
            else
            {
                var result = new ProfessionDto()
                {
                    ProfessionID = profession.ProfessionId,
                    ProfessionFullName = profession.ProfessionFullName,
                };
                return new ApiSuccessResult<ProfessionDto>(result);
            }
        }

        public async Task<ApiResult<ProfessionDto>> GetProfessionBySpecialty(int specialtyId)
        {
            Expression<Func<Profession, bool>> professionExpression = s => s.Specialties.FirstOrDefault().SpecialtyId == specialtyId;
            var profession = await _professionRepository.GetById(professionExpression);
            var result = new ProfessionDto()
            {
                ProfessionID = profession.ProfessionId
            };
            return new ApiSuccessResult<ProfessionDto>(result);
        }

        public async Task<ApiResult<List<ProfessionDto>>> GetProfessionsBySupervisorIdAndIsDevHead(string supervisorId, bool isDevHead)
        {
            var currentSemester = await _semesterRepository.GetCurrentSemester();
            List<Expression<Func<SupervisorProfession, bool>>> expression = new List<Expression<Func<SupervisorProfession, bool>>>();
            expression.Add(e => e.SupervisorId == supervisorId);
            expression.Add(e => e.IsDevHead == true);
            var supervisorProfession = await _supervisorProfessionRepository.GetByConditions(expression);
            var professionIds = supervisorProfession.Select(sp => sp.ProfessionId).ToList();
            List<Expression<Func<Profession, bool>>> professionExpression = new List<Expression<Func<Profession, bool>>>();
            professionExpression.Add(p => professionIds.Contains(p.ProfessionId));
            professionExpression.Add(p => p.SemesterId == currentSemester.SemesterID);
            professionExpression.Add(p => p.DeletedAt == null);
            var professions = await _professionRepository.GetByConditions(professionExpression);
            var result = new List<ProfessionDto>();
            foreach (var profession in professions)
            {
                result.Add(new ProfessionDto()
                {
                    ProfessionID = profession.ProfessionId,
                    ProfessionAbbreviation = profession.ProfessionAbbreviation,
                    ProfessionFullName = profession.ProfessionFullName,
                    Semester = new SemesterDto()
                    {
                        SemesterID = profession.SemesterId.Value,
                    }
                });
            }
            return new ApiSuccessResult<List<ProfessionDto>>(result);
        }

        public async Task<ApiResult<bool>> RemoveProfession(int id)
        {
            Expression<Func<SupervisorProfession, bool>> expression = sp => sp.ProfessionId == id;
            var supervisorProfession = await _supervisorProfessionRepository.GetById(expression);
            await _supervisorProfessionRepository.DeleteAsync(supervisorProfession);
            Expression<Func<Profession, bool>> professionExpression = p => p.ProfessionId == id;
            var profession = await _professionRepository.GetById(professionExpression);
            profession.DeletedAt = DateTime.Now;
            await _professionRepository.UpdateAsync(profession);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateProfession(ProfessionDto profession)
        {
            List<Expression<Func<Profession, bool>>> professionExpression = new List<Expression<Func<Profession, bool>>>();
            professionExpression.Add(p => p.ProfessionId == profession.ProfessionID);
            professionExpression.Add(p => p.DeletedAt == null);
            var professionReult = await _professionRepository.GetByConditionId(professionExpression);
            professionReult.ProfessionAbbreviation = profession.ProfessionAbbreviation;
            professionReult.ProfessionFullName = profession.ProfessionFullName;
            professionReult.UpdatedAt = DateTime.Now;
            await _professionRepository.UpdateAsync(professionReult);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<int>> UpdateProfessionV2(ProfessionDto profession)
        {
            var professionResult = await _professionRepository.UpsertProfessionAsyncV2(profession.ProfessionID, profession.ProfessionAbbreviation, profession.ProfessionFullName, profession.Semester.SemesterID);
            return new ApiSuccessResult<int>(professionResult);
        }
    }
}