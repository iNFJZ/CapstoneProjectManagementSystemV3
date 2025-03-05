using System;
using System.Collections.Generic;

namespace Infrastructure.Entities
{
    public partial class AffiliateAccount
    {
        public string AffiliateAccountId { get; set; } = null!;
        public string? PersonalEmail { get; set; }
        public string? PasswordHash { get; set; }
        public string? OneTimePassword { get; set; }
        public DateTime? OtpRequestTime { get; set; }
        public bool? IsVerifyEmail { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual User AffiliateAccountNavigation { get; set; } = null!;
    }
}
