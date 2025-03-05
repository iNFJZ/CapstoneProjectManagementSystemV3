using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.PasswordHash
{
    public interface IPasswordHash
    {
        string HashPassword(string password);
        bool PasswordVerificationResult(string hashedPassword, string password);
    }
}
