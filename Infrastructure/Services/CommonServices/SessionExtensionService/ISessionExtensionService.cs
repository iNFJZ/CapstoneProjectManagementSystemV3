using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.SessionExtensionService
{
    public interface ISessionExtensionService
    {
        public T GetObjectFromJson<T>(ISession session, string key);
        public void SetObjectAsJson<T>(ISession session, string key, T value);
    }
}
