using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.NewsRepository
{
    public interface INewsRepository : IRepositoryBase<News>
    {
    }
}
