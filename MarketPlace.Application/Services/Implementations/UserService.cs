using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs;
using MarketPlace.DataLayer.Entities.Account;
using MarketPlace.DataLayer.Repository;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region constractore

        private readonly IGenericRepository<User> _usesRepository;
        private readonly IPasswordHelper _passwordHelper;
        public UserService(IGenericRepository<User> usesRepository, IPasswordHelper passwordHelper)
        {
            _usesRepository = usesRepository;
            _passwordHelper = passwordHelper;
        }
        #endregion

        #region dispose

        public async ValueTask DisposeAsync()
        {
            await _usesRepository.DisposeAsync();
        }

        #endregion

        #region account
        public async Task<RegisterUserResult> RegisterUser(RegisterUserDTO register)
        {
            if (!await IsUserExistsByMobileNumber(register.Mobile))
            {
                var user = new User
                {
                    FirstName = register.FirstName,
                    LastName = register.LastName,
                    Mobile = register.Mobile,
                    Password = _passwordHelper.EncodePasswordMd5(register.Password)
                };
                await _usesRepository.AddEntity(user);
                await _usesRepository.SaveChanges();
                // todo : send sms here
                return RegisterUserResult.Success;                                                       
            }
            else
            {
                return RegisterUserResult.MobileExists;
            }

            return RegisterUserResult.Error;
        }

        public async Task<bool> IsUserExistsByMobileNumber(string mobile)
        {
            return await _usesRepository.GetQuery().AsQueryable().AnyAsync(x => x.Mobile == mobile);
        }
        #endregion

    }
}
