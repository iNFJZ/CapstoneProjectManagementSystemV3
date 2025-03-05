using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RoleRepository
{
    public class RoleRepository : RepositoryBase<Role>, IRoleRepository
    {
        private readonly DBContext _dbContext;
        public RoleRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
