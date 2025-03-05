using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.AffiliateAccountService
{
    public interface IAffiliateAccountService
    {
       Task<ApiResult<bool>> UpdateOTP(String BackupAccount_Id, String OTP);
       Task<ApiResult<bool>> UpdatePasswordHash(String BackupAccount_Id, String password);
       Task<ApiResult<AffiliateAccountDto>> GetAffiliateAccountById(string BackupAccount_Id);
       Task<ApiResult<AffiliateAccountDto>> GetAffiliateAccountByEmail(string email);
       Task<ApiResult<bool>> AddOTP(string affiliateAccountId, string otp);
       Task<ApiResult<bool>> UpdateIsVerifyEmail(string affiliateAccountId, string personalEmail);
       Task<ApiResult<bool>> CheckPersonalEmailExist(string personalEmail);
       Task<ApiResult<bool>> CheckVerifyPersonalEmail(string affliateAccountID);
       Task<ApiResult<bool>> CheckAffiliateAccountAndPasswordHash(string personalEmail, string passwordHash);

    }
}
