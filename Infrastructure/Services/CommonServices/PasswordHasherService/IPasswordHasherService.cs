using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.PasswordHasherService
{
    public interface IPasswordHasherService
    {
        string HashPassword(string password);
        bool PasswordVerificationResult(string hashedPassword, string password);
    }

}
