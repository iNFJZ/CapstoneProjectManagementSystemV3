using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.WithRepository
{
    public class WithRepository : RepositoryBase<With>, IWithRepository
    {
        private readonly DBContext _dbContext;
        public WithRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
