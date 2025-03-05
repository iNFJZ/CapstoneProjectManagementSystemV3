using Infrastructure.Entities;
using Infrastructure.Entities.Common;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.SpecialtyRepository;
using Infrastructure.Services.PrivateService.SpecialtyService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.FinalGroupService
{
    public class FinalGroupDisplayFormService : IFinalGroupDisplayFormService
    {
        private readonly ISpecialtyRepository _specialtyRepository;
        public FinalGroupDisplayFormService(ISpecialtyRepository specialtyRepository)
        {
            _specialtyRepository = specialtyRepository;
        }

        public async Task<ApiResult<List<FinalGroupDisplayForm>>> ConvertFromFinalList(List<FinalGroup> finalGroupList)
        {
            List<FinalGroupDisplayForm> finalGroupDisplayFormList = new List<FinalGroupDisplayForm>();
            if (finalGroupList == null) return new ApiSuccessResult<List<FinalGroupDisplayForm>>(finalGroupDisplayFormList);
            else
            {
                foreach (FinalGroup item in finalGroupList)
                {
                    List<Expression<Func<Specialty, bool>>> specialtyExpression = new List<Expression<Func<Specialty, bool>>>();
                    specialtyExpression.Add(s => s.SpecialtyId == item.SpecialtyId);
                    specialtyExpression.Add(s => s.DeletedAt == null);
                    var specialty = await _specialtyRepository.GetByConditionId(specialtyExpression);
                    FinalGroupDisplayForm finalGroupDisplayForm = new FinalGroupDisplayForm()
                    {
                        FinalGroupID = item.FinalGroupId,
                        GroupName = item.GroupName,
                        ProjectEnglishName = item.ProjectEnglishName,
                        SpecialtyFullName = specialty.SpecialtyFullName,
                        MaxMember = item.MaxMember.Value,
                        NumberOfMember = item.NumberOfMember.Value,
                        CreatedAt = item.CreatedAt
                    };
                    finalGroupDisplayFormList.Add(finalGroupDisplayForm);
                }
                return new ApiSuccessResult<List<FinalGroupDisplayForm>>(finalGroupDisplayFormList);
            }
        }
    }
}
