using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities;

namespace Talbat.Core.Services.Contract
{
    public interface IPaymentService
    {
        Task<CustomerBasket?> CreaterUpdatePaymentAsync(string basketId);

    }
}
