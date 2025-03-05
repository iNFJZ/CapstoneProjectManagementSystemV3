using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Infrastructure.Repositories.Student_FavoriteGroupIdeaRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.StudentFavoriteGroupIdeaRepository
{
    public class StudentFavoriteGroupIdeaRepository : RepositoryBase<StudentFavoriteGroupIdea>, IStudentFavoriteGroupIdeaRepository
    {
        private readonly DBContext _dBContext;
        public StudentFavoriteGroupIdeaRepository(DBContext dBContext) : base(dBContext) 
        {
            _dBContext = dBContext;
        }
    }
}
