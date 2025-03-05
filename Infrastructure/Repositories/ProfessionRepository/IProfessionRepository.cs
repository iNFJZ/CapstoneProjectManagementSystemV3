using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ProfessionRepository
{
    public interface IProfessionRepository : IRepositoryBase<Profession>
    {
        Task<int> UpsertProfessionAsyncV2(int id, string abbreviation, string fullName, int semesterId);
    }
}
