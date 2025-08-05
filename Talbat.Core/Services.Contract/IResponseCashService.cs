using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talbat.Core.Services.Contract
{
    public interface IResponseCashService
    {
        Task CashResponseAsync(string CashKey,object Response, TimeSpan TimeToLive);

        Task<string?> GetCashedResonseAsync(string CashKey);
    }
}
