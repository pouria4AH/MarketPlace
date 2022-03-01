﻿using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs;
using MarketPlace.DataLayer.Entities.Account;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region account

        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        Task<bool> IsUserExistsByMobileNumber(string mobile);
        Task<LoginUserResult> GetUserForLogin(LoginUserDTO login);
        Task<User> GetUserByMobile(string mobile);

        #endregion
    }
}