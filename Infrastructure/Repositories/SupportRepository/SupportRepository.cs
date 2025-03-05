using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.SupportRepository
{
    public class SupportRepository : RepositoryBase<Support>, ISupportRepository
    {
        private readonly DBContext _dbContext;
        public SupportRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
