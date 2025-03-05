using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.NewsRepository
{
    public class NewsRepository : RepositoryBase<News>,INewsRepository
    {
        private readonly DBContext _dbContext;
        public NewsRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
