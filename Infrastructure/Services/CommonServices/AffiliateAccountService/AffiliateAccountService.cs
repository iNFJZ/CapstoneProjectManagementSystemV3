using Infrastructure.Entities;
using Infrastructure.Entities.Common.ApiResult;
using Infrastructure.Repositories.AffiliateAccountRepository;
using Infrastructure.Repositories.PasswordHash;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace Infrastructure.Services.CommonServices.AffiliateAccountService
{
    public class AffiliateAccountService : IAffiliateAccountService
    {
        private readonly IAffiliateAccountRepository _affiliateAccountRepository;
        private readonly IPasswordHash _passwordHash;
        public AffiliateAccountService(IAffiliateAccountRepository affiliateAccountRepository,
            IPasswordHash passwordHash)
        {
            _affiliateAccountRepository = affiliateAccountRepository;
            _passwordHash = passwordHash;
        }

        public async Task<ApiResult<bool>> AddOTP(string affiliateAccountId, string otp)
        {
            var newAffiliateAccount = new AffiliateAccount()
            {
                AffiliateAccountId = affiliateAccountId,
                PersonalEmail = "",
                OneTimePassword = otp,
                OtpRequestTime = DateTime.Now
            };
            await _affiliateAccountRepository.CreateAsync(newAffiliateAccount);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> CheckAffiliateAccountAndPasswordHash(string personalEmail, string passwordHash)
        {
            List<Expression<Func<AffiliateAccount, bool>>> expressions = new List<Expression<Func<AffiliateAccount, bool>>>();
            expressions.Add(e => e.PersonalEmail.Contains(personalEmail));
            expressions.Add(e => e.DeletedAt == null);
            var affiliateAccount = await _affiliateAccountRepository.GetByConditionId(expressions);
            if (affiliateAccount != null)
            {
                var check = _passwordHash.PasswordVerificationResult(affiliateAccount.PasswordHash, passwordHash);
                if (check == true)
                    return new ApiSuccessResult<bool>(true);
                else
                    return new ApiSuccessResult<bool>(false);
            }
            else
            {
                return new ApiSuccessResult<bool>(false);
            }
        }

        public async Task<ApiResult<bool>> CheckPersonalEmailExist(string personalEmail)
        {
            List<Expression<Func<AffiliateAccount, bool>>> expressions = new List<Expression<Func<AffiliateAccount, bool>>>();
            expressions.Add(e => e.PersonalEmail.Contains(personalEmail));
            expressions.Add(e => e.DeletedAt == null);
            expressions.Add(e => e.IsVerifyEmail == true);
            var affiliateAccount = await _affiliateAccountRepository.GetByConditionId(expressions);
            if(affiliateAccount != null)
            {
                return new ApiSuccessResult<bool>  (true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<bool>> CheckVerifyPersonalEmail(string affliateAccountID)
        {
            List<Expression<Func<AffiliateAccount, bool>>> expressions = new List<Expression<Func<AffiliateAccount, bool>>>();
            expressions.Add(e => e.AffiliateAccountId.Contains(affliateAccountID));
            expressions.Add(e => e.DeletedAt == null);
            var affiliateAccount = await _affiliateAccountRepository.GetByConditionId(expressions);
            if (affiliateAccount != null)
            {
                return new ApiSuccessResult<bool>(true);
            }
            return new ApiSuccessResult<bool>(false);
        }

        public async Task<ApiResult<AffiliateAccountDto>> GetAffiliateAccountByEmail(string email)
        {
            try
            {
                if (email != null)
                {
                    List<Expression<Func<AffiliateAccount, bool>>> expressions = new List<Expression<Func<AffiliateAccount, bool>>>();
                    expressions.Add(e => e.PersonalEmail.Contains(email));
                    expressions.Add(e => e.DeletedAt == null);
                    var affiliateAccount = await _affiliateAccountRepository.GetByConditionId(expressions);
                    var affiliateAccountDto = new AffiliateAccountDto()
                    {
                        AffiliateAccountID = affiliateAccount.AffiliateAccountId,
                        PersonalEmail = affiliateAccount.PersonalEmail,
                        PasswordHash = affiliateAccount.PasswordHash,
                        IsVerifyEmail = affiliateAccount.IsVerifyEmail,
                        OneTimePassword = affiliateAccount.OneTimePassword,
                        User = affiliateAccount.AffiliateAccountNavigation
                    };
                    return new ApiSuccessResult<AffiliateAccountDto>(affiliateAccountDto);
                }
                return new ApiErrorResult<AffiliateAccountDto>("Tham số truyền vào trống");
            }
            catch (Exception ex)
            {
                return new ApiErrorResult<AffiliateAccountDto>("Tham số truyền vào trống");
            }
        }

        public async Task<ApiResult<AffiliateAccountDto>> GetAffiliateAccountById(string BackupAccount_Id)
        {
            List<Expression<Func<AffiliateAccount, bool>>> expressions = new List<Expression<Func<AffiliateAccount, bool>>>();
            expressions.Add(e => e.AffiliateAccountId.Contains(BackupAccount_Id));
            expressions.Add(e => e.DeletedAt == null);
            var affiliateAccount = await _affiliateAccountRepository.GetByConditionId(expressions);
            var affiliateAccountDto = new AffiliateAccountDto()
            {
                AffiliateAccountID = affiliateAccount.AffiliateAccountId,
                PersonalEmail = affiliateAccount.PersonalEmail,
                PasswordHash = affiliateAccount.PasswordHash,
                IsVerifyEmail = affiliateAccount.IsVerifyEmail,
                OneTimePassword = affiliateAccount.OneTimePassword,
                User = affiliateAccount.AffiliateAccountNavigation
            };
            return new ApiSuccessResult<AffiliateAccountDto>(affiliateAccountDto);
        }

        public async Task<ApiResult<bool>> UpdateIsVerifyEmail(string affiliateAccountId, string personalEmail)
        {
            Expression<Func<AffiliateAccount, bool>> expression = x => x.AffiliateAccountId == affiliateAccountId;
            var findObj = await _affiliateAccountRepository.GetById(expression);
            if (findObj == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findObj.IsVerifyEmail = true;
            findObj.PersonalEmail = personalEmail;
            await _affiliateAccountRepository.UpdateAsync(findObj);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdateOTP(string BackupAccount_Id, string OTP)
        {
            Expression<Func<AffiliateAccount, bool>> expression = x => x.AffiliateAccountId == BackupAccount_Id;
            var findObj = await _affiliateAccountRepository.GetById(expression);
            if(findObj == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findObj.OneTimePassword = OTP;
            findObj.OtpRequestTime = DateTime.Now;
            await _affiliateAccountRepository.UpdateAsync(findObj);
            return new ApiSuccessResult<bool>(true);
        }

        public async Task<ApiResult<bool>> UpdatePasswordHash(string BackupAccount_Id, string password)
        {
            Expression<Func<AffiliateAccount, bool>> expression = x => x.AffiliateAccountId == BackupAccount_Id;
            var findObj = await _affiliateAccountRepository.GetById(expression);
            if (findObj == null)
            {
                return new ApiErrorResult<bool>("Không tìm thấy đối tượng");
            }
            findObj.PasswordHash = password;
            findObj.OtpRequestTime = DateTime.Now;
            await _affiliateAccountRepository.UpdateAsync(findObj);
            return new ApiSuccessResult<bool>(true);
        }
    }
}
