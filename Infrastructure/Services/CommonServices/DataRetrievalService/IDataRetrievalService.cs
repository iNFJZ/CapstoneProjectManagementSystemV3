using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.CommonServices.DataRetrievalService
{
    public interface IDataRetrievalService
    {
        T GetData<T>(string key, bool useHeader = true, bool useCache = true);
        void SetData<T>(string key, T value, TimeSpan? expiration = null);
    }
}
