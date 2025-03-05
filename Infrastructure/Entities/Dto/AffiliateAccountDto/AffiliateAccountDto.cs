using Infrastructure.Entities.Dto.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Entities
{
    public class AffiliateAccountDto : CommonProperty
    {
        public string AffiliateAccountID { get; set; }
        public string PersonalEmail { get; set; }
        public string PasswordHash { get; set; }
        public bool? IsVerifyEmail { get; set; }
        public string OneTimePassword { get; set; }
        public DateTime OtpRequestTime { get; set; }
        public User User { get; set; }
    }
}
