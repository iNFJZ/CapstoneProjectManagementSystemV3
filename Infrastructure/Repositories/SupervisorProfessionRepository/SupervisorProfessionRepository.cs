using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SupervisorProfessionRepository
{
    public class SupervisorProfessionRepository : RepositoryBase<SupervisorProfession>, ISupervisorProfessionRepository
    {
        private readonly DBContext _dbContext;
        public SupervisorProfessionRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
