using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.AffiliateAccountRepository
{
    public class AffiliateAccountRepository : RepositoryBase<AffiliateAccount>, IAffiliateAccountRepository
    {
        private readonly DBContext _dbContext;
        public AffiliateAccountRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        
    }
}
