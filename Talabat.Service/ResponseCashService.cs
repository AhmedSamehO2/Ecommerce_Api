using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core;

namespace Talabat.Service
{
    public class ResponseCashService : IResponseCashService
    {
        private readonly IDatabase _database;
        public ResponseCashService(IConnectionMultiplexer Redis)
        {
            _database = Redis.GetDatabase();
        }
        public async Task CashResponseAsync(string CashKey, object Response, TimeSpan ExpireDate)
        {
            if (Response is null) return;
            var Options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var cirilzedResponse = JsonSerializer.Serialize(Response, Options);
          await  _database.StringSetAsync(CashKey, cirilzedResponse,ExpireDate);
        }

        public async Task<string?> GetCashedResponse(string CashKey)
        {
           var CashedKey = await _database.StringGetAsync(CashKey);
            if (CashedKey.IsNullOrEmpty) return null;
            return CashedKey;

           
        }
    }
}
