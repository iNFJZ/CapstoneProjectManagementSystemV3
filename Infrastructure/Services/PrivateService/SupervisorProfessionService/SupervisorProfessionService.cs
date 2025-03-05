using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SupervisorProfessionRepository;
using Infrastructure.Repositories.SupervisorRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.SupervisorProfessionService
{
    public class SupervisorProfessionService : ISupervisorProfessionService
    {
        private readonly ISupervisorProfessionRepository _supervisorProfessionRepository;
        private readonly ISupervisorRepository _supervisorRepository;
        private readonly IProfessionRepository _professionRepository;
        public SupervisorProfessionService(ISupervisorProfessionRepository supervisorProfessionRepository,
            ISupervisorRepository supervisorRepository,
            IProfessionRepository professionRepository)
        {
            _supervisorProfessionRepository = supervisorProfessionRepository;
            _supervisorRepository = supervisorRepository;
            _professionRepository = professionRepository;
        }

        public async Task<ApiResult<List<Profession>>> GetProfessionsBySupervisorID(string supervisorId, int semesterId)
        {
            // 🔹 Lọc danh sách Supervisor_Profession theo Supervisor_ID
            var supervisors = await _supervisorRepository.GetByCondition(s => s.SupervisorId == supervisorId);


            if (supervisors == null || !supervisors.Any())
            {
                return new ApiErrorResult<List<Profession>>( "Không tìm thấy Supervisor hoặc không có Profession nào liên kết");
            }

            // 🔹 Lọc danh sách Profession theo danh sách Profession_ID đã lấy được
            var supervisorsIds = supervisors.Select(sp => sp.SupervisorId).ToList();
            var supervisorProfessions = await _supervisorProfessionRepository.GetByCondition(sp => supervisorsIds.Contains(sp.SupervisorId));
            var professionIds = supervisorProfessions.Select(sp => sp.ProfessionId).ToList();
            var professions = await _professionRepository.GetByCondition(p =>
                professionIds.Contains(p.ProfessionId) &&
                p.SemesterId == semesterId &&
                p.DeletedAt == null);

            if (professions == null || !professions.Any())
            {
                return new ApiErrorResult<List<Profession>>("Không có bản ghi nào");
            }

            return new ApiSuccessResult<List<Profession>>(professions);
        }

    }
}
