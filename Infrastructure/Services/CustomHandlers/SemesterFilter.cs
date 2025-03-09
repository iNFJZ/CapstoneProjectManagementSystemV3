using Infrastructure.Entities;
using Infrastructure.Repositories.SemesterRepository;
using Infrastructure.Services.CommonServices.SemesterService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CustomHandlers
{
    public class SemesterFilter : ISemesterRepository
    {
        private readonly ISemesterRepository _semesterRepository;
        public SemesterFilter(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }

        public Task<bool> CloseSemesterCurrent()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> CountAsync(Expression<Func<Semester, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Semester entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Semester entity)
        {
            throw new NotImplementedException();
        }

        public Task<Semester> FindByName(Expression<Func<Semester, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<List<Semester>> GetAll(int? pageSize, int? pageIndex, Expression<Func<Semester, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Semester>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Semester>> GetAll(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Semester>> GetAll(int? pageSize, int? pageIndex)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<Semester>> GetAllAsQueryable()
        {
            throw new NotImplementedException();
        }

        public Task<List<SemesterDto>> GetAllSemester()
        {
            throw new NotImplementedException();
        }

        public Task<List<Semester>> GetByCondition(Expression<Func<Semester, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Semester> GetByConditionId(List<Expression<Func<Semester, bool>>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<List<Semester>> GetByConditions(List<Expression<Func<Semester, bool>>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Semester> GetById(Expression<Func<Semester, bool>> expression)
        {
            throw new NotImplementedException();
        }

        public Task<Semester> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<SemesterDto> GetCurrentSemester()
        {
            throw new NotImplementedException();
        }

        public Task<SemesterDto> GetSemesterById(int semesterId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Semester entity)
        {
            throw new NotImplementedException();
        }
    }
}
