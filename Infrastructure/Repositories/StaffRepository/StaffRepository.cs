using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StaffRepository
{
    public class StaffRepository : RepositoryBase<Staff>, IStaffRepository
    {
        private readonly DBContext _dbContext;
        public StaffRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
