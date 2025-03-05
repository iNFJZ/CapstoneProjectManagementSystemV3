using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ConfigurationRepository
{
    public interface IConfigurationRepository : IRepositoryBase<Configuration>
    {
        Task<List<With>> GetWithsBySpecialtyID(int specialtyID);
    }
}
