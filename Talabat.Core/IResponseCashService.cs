using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core
{
    public interface IResponseCashService
    {
        //cash Data
        Task CashResponseAsync(string CashKey, object Response, TimeSpan ExpireDate);
        Task<string?> GetCashedResponse(string CashKey);
    }
}
