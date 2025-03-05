using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ProfessionRepository
{
    public class ProfessionRepository : RepositoryBase<Profession>, IProfessionRepository
    {
        private readonly DBContext _dbContext;
        public ProfessionRepository(DBContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> UpsertProfessionAsyncV2(int id, string abbreviation, string fullName, int semesterId)
        {
            if (id > 0)
            {
                // 1️⃣ Tìm profession có ID tương ứng
                var existingProfession = await _dbContext.Professions
                    .FirstOrDefaultAsync(p => p.ProfessionId == id);

                if (existingProfession != null)
                {
                    // 2️⃣ Nếu tồn tại, cập nhật thông tin
                    existingProfession.ProfessionAbbreviation = abbreviation;
                    existingProfession.ProfessionFullName = fullName;
                    existingProfession.SemesterId = semesterId;
                    existingProfession.UpdatedAt = DateTime.UtcNow;

                    await _dbContext.SaveChangesAsync();
                    return existingProfession.ProfessionId; // Trả về ID đã cập nhật
                }
            }

            // 3️⃣ Nếu không có ID hoặc không tìm thấy, thêm mới
            var newProfession = new Profession
            {
                ProfessionAbbreviation = abbreviation,
                ProfessionFullName = fullName,
                SemesterId = semesterId,
                CreatedAt = DateTime.UtcNow
            };

            _dbContext.Professions.Add(newProfession);
            await _dbContext.SaveChangesAsync();
            return newProfession.ProfessionId; // Trả về ID mới
        }
    }
}
