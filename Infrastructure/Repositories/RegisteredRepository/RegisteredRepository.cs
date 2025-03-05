using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.RegisteredRepository
{
    public class RegisteredRepository : RepositoryBase<RegisteredGroup>,IRegisteredRepository
    {
        private readonly DBContext _dbContext;
        public RegisteredRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<RegisteredGroup> GetRegisteredGroupByGroupIdeaId(int groupIdeaId)
        {
            var registeredGroup = await _dbContext.RegisteredGroups
                    .Where(rg => rg.GroupIdeaId == groupIdeaId && rg.DeletedAt == null)
                    .Select(rg => new RegisteredGroup
                    {
                        RegisteredGroupId = rg.RegisteredGroupId,
                        StudentsRegistraiton = rg.StudentsRegistraiton,
                        GroupIdea = new GroupIdea
                        {
                            GroupIdeaId = rg.GroupIdeaId
                        },
                        StudentComment = rg.StudentComment,
                        Status = rg.Status,
                        StaffComment = rg.StaffComment,
                        RegisterdGroupSupervisors = rg.RegisterdGroupSupervisors
                            .Select(rgs => new RegisterdGroupSupervisor
                            {
                                Supervisor = new Supervisor
                                {
                                    SupervisorId = rgs.Supervisor.SupervisorId,
                                    SupervisorNavigation = new User
                                    {
                                        FullName = rgs.Supervisor.SupervisorNavigation.FullName
                                    }
                                }
                            }).ToList()
                    }).FirstOrDefaultAsync();
                return registeredGroup;
            }
        }
}
