using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region account

        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        Task<bool> IsUserExistsByMobileNumber(string mobile);

        #endregion
    }
}
