using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.ConfigurationRepository;
using Infrastructure.Repositories.ProfessionRepository;
using Infrastructure.Repositories.SpecialtyRepository;
using Infrastructure.Repositories.WithRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.PrivateService.ConfigurationService
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IConfigurationRepository _configurationRepository;
        private readonly ISpecialtyRepository _specialtyRepository;
        private readonly IWithRepository _withRepository;
        private readonly IProfessionRepository _professionRepository;
        public ConfigurationService(IConfigurationRepository configurationRepository,
            ISpecialtyRepository specialtyRepository,
            IWithRepository withRepository,
            IProfessionRepository professionRepository)
            
        {
            _configurationRepository = configurationRepository;
            _specialtyRepository = specialtyRepository;
            _withRepository = withRepository;
            _professionRepository = professionRepository;
        }

        public async Task<ApiResult<List<With>>> GetWithProfessionBySpecialityId(int specialtyId)
        {
            var specialty = await _specialtyRepository.GetById(s => s.SpecialtyId == specialtyId);
            if (specialty == null)
            {
                return new ApiErrorResult<List<With>>("Specialty not found");
            }

            // Step 3: Retrieve all With entities related to the given SpecialtyId from the With table
            var withs = await _withRepository.GetByCondition(w => w.Specialty.SpecialtyId == specialtyId);

            // Step 4: Create the list of With entities and assign related Profession and Specialty information
            var listW = withs.Select(w => new With()
            {
                Profession = new Profession()
                {
                    ProfessionId = w.Profession.ProfessionId
                },
                Specialty = new Specialty()
                {
                    SpecialtyId = w.Specialty.SpecialtyId
                }
            }).ToList();

            // Step 5: Return the list wrapped in ApiResult
            return new ApiSuccessResult<List<With>>(listW);
        }


        public async Task<ApiResult<List<With>>> GetWithsBySpecialtyID(int specialtyID)
        {
            var specialty = await _specialtyRepository.GetByCondition(s => s.SpecialtyId == specialtyID);
            if (specialty == null || !specialty.Any())
            {
                return new ApiErrorResult<List<With>>("Specialty not found");
            }
            var withs = await _withRepository.GetByCondition(w => w.Specialty.SpecialtyId == specialtyID);
            var professionIds = withs
                .GroupBy(w => w.Profession.ProfessionId)
                .Select(g => g.Key)
                .ToList();
            return new ApiSuccessResult<List<With>>(withs);
        }

        public async Task<ApiResult<bool>> Insert(int Specialty, List<With> Withs, int Profession)
        {
            var specialty = await _specialtyRepository.GetByCondition(s => s.SpecialtyId == Specialty);
            if (specialty == null || specialty.Count == 0)
                throw new Exception("Specialty not found.");
            var existingWiths = await _withRepository.GetByCondition(w => w.Profession.ProfessionId == Profession && w.Specialty.SpecialtyId == Specialty);
            if (existingWiths != null && existingWiths.Count > 0)
            {
                foreach (var withItem in existingWiths)
                {
                    specialty[0].Withs.Remove(withItem);
                }
            }
            foreach (var withEntity in Withs)
            {
                var newWith = new With
                {
                    Profession = await _professionRepository.GetById(p => p.ProfessionId == Profession),
                    Specialty = specialty[0] 
                };
                _withRepository.CreateAsync(newWith);
                specialty[0].Withs.Add(newWith);
            }

            return new ApiSuccessResult<bool>(true);
        }
    }
}
