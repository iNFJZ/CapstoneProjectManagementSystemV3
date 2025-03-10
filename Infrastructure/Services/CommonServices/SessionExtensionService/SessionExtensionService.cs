using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.SessionExtensionService
{
    public class SessionExtensionService : ISessionExtensionService 
    {
        public T GetObjectFromJson<T>(ISession session, string key)
        {
            var value = session.Get(key);

#pragma warning disable CS8603 // Possible null reference return.
            return value == null ? default(T) :JsonSerializer.Deserialize<T>(value);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public void SetObjectAsJson<T>(ISession session, string key, T value)
        {
            session.Set(key, JsonSerializer.SerializeToUtf8Bytes(value));
        }
    }
}
