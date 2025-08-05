using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talbat.Core.Entities.Identity;

namespace Talbat.Core.Services.Contract
{
    public interface IAuthServices
    {
        Task<string> CreateTokenAsunc(AppUser User , UserManager<AppUser> userManager);
    }
}
