using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Services.Contract;
using System.Text.Json;
namespace Talbat.Service
{
    public class ResponseCashService : IResponseCashService
    {
        private readonly IDatabase _database;

        public ResponseCashService(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task CashResponseAsync(string CashKey, object Response, TimeSpan TimeToLive)
        {
            if (Response is null)
                return;
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var serializedResponse = JsonSerializer.Serialize(Response,serializeOptions);

            await _database.StringSetAsync(CashKey, serializedResponse, TimeToLive);
        }

        public async Task<string?> GetCashedResonseAsync(string CashKey)
        {
            var cachedResponse = await _database.StringGetAsync(CashKey);

            if(cachedResponse.IsNullOrEmpty)
                return null;
            return cachedResponse.ToString();
        }
    }
}
