using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Infrastructure.Repositories.RegisteredGroupSupervisorRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RegisteredGroupSupervisor
{
    public class RegisteredGroupSupervisorRepository : RepositoryBase<RegisterdGroupSupervisor>,IRegisteredGroupSupervisorRepository
    {
        private readonly DBContext _dbContext;
        public RegisteredGroupSupervisorRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
