using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Infrastructure.ViewModel.SupervisorViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Supervisor_GroupIdeaReporitory
{
    public class SupervisorGroupIdeaReporitory : RepositoryBase<GroupIdeasOfSupervisor>, ISupervisorGroupIdeaReporitory
    {
        private readonly DBContext _dbContext;
        public SupervisorGroupIdeaReporitory (DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
