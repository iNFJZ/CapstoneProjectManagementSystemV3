using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SupervisorRepository
{
    public class SupervisorRepository : RepositoryBase<Supervisor>, ISupervisorRepository
    {
        private readonly DBContext _dbContext;
        public SupervisorRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
