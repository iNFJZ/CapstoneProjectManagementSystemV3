using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.NotificationRepository
{
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {
        private readonly DBContext _dbContext;
        public NotificationRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
