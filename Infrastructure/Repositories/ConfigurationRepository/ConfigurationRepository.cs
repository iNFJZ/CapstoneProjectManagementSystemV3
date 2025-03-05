using DocumentFormat.OpenXml.InkML;
using Infrastructure.Entities;
using Infrastructure.Entities.DBContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.ConfigurationRepository
{
    public class ConfigurationRepository : RepositoryBase<Configuration>, IConfigurationRepository
    {
        private readonly DBContext _dbContext;
        public ConfigurationRepository(DBContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task<List<With>> GetWithsBySpecialtyID(int specialtyID)
        {
            return await _dbContext.Specialties
                    .Where(s => s.SpecialtyId == specialtyID)
                    .SelectMany(s => s.Withs) // Lấy danh sách With liên kết với Specialty
                    .ToListAsync();
        }
        public async Task<List<With>> GetWithProfessionBySpecialityId(int specialityId)
        {
            return await _dbContext.Specialties
                    .Where(s => s.SpecialtyId == specialityId)
                    .SelectMany(s => s.Withs)
                    .GroupBy(w => w.ProfessionId) // Nhóm theo ProfessionId
                    .Select(g => g.First()) // Lấy một bản ghi đầu tiên từ mỗi nhóm
                    .ToListAsync();
        }
        public async Task<bool> InsertAsync(int specialtyId, List<With> withs, int professionId)
        {
            using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {

                    // Xóa tất cả các With có specialtyId hiện tại
                    var existingWiths = _dbContext.Withs.Where(w => w.SpecialtyId == specialtyId);
                    _dbContext.Withs.RemoveRange(existingWiths);
                    await _dbContext.SaveChangesAsync();

                    // Thêm mới danh sách With vào bảng Withs
                    foreach (var with in withs)
                    {
                        var newWith = new With
                        {
                            ProfessionId = with.Profession.ProfessionId,
                            SpecialtyId = with.Specialty.SpecialtyId
                        };
                        _dbContext.Withs.Add(newWith);
                    }
                    await _dbContext.SaveChangesAsync();

                    // Thêm With mới cho professionId
                    var newWithForProfession = new With
                    {
                        ProfessionId = professionId,
                        SpecialtyId = specialtyId
                    };
                    _dbContext.Withs.Add(newWithForProfession);
                    await _dbContext.SaveChangesAsync();

                    // Xóa các bản ghi trong Withs liên quan đến specialtyId
                    foreach (var with in withs)
                    {
                        var removeWiths = _dbContext.Withs
                            .Where(w => w.SpecialtyId == with.Specialty.SpecialtyId && w.ProfessionId == professionId);
                        _dbContext.Withs.RemoveRange(removeWiths);
                    }
                    await _dbContext.SaveChangesAsync();

                    // Thêm quan hệ mới vào Withs mà không qua bảng trung gian
                    foreach (var with in withs)
                    {
                        var newWithRelation = new With
                        {
                            ProfessionId = newWithForProfession.ProfessionId,
                            SpecialtyId = with.Specialty.SpecialtyId
                        };
                        _dbContext.Withs.Add(newWithRelation);
                    }
                    await _dbContext.SaveChangesAsync();

                    // Commit transaction
                    await transaction.CommitAsync();
                    return true;
                }

                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Console.WriteLine("Exception: " + ex.Message);
                    return false;
                }
            }
        }
    }
}
