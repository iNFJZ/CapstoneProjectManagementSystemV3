using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.GroupIdeaOfSupervisorProfessionRepository
{
    public class GroupIdeaOfSupervisorProfessionRepository : RepositoryBase<GroupIdeaOfSupervisorProfession>,IGroupIdeaOfSupervisorProfessionRepository
    {
        private readonly DBContext _dbContext;
        public GroupIdeaOfSupervisorProfessionRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
