using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Infrastructure.Services.CommonServices.DataRetrievalService
{
    public class DataRetrievalService : IDataRetrievalService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMemoryCache _memoryCache;

        public DataRetrievalService(IHttpContextAccessor httpContextAccessor, IMemoryCache memoryCache)
        {
            _httpContextAccessor = httpContextAccessor;
            _memoryCache = memoryCache;
        }

        /// <summary>
        /// Lấy dữ liệu từ Header hoặc Cache
        /// </summary>
        public T GetData<T>(string key, bool useHeader = true, bool useCache = true)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return default;
            }

            // Kiểm tra lấy dữ liệu từ Header
            if (useHeader && httpContext.Request.Headers.TryGetValue(key, out var headerValue))
            {
                return JsonSerializer.Deserialize<T>(headerValue);
            }

            // Kiểm tra lấy dữ liệu từ Cache
            if (useCache && _memoryCache.TryGetValue(key, out T cacheValue))
            {
                return cacheValue;
            }

            return default;
        }

        /// <summary>
        /// Lưu dữ liệu vào Cache
        /// </summary>
        public void SetData<T>(string key, T value, TimeSpan? expiration = null)
        {
            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromMinutes(30)
            };

            _memoryCache.Set(key, value, cacheOptions);
        }
    }
}
