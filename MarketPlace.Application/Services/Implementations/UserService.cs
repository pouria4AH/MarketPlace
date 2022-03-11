using System;
using System.Linq;
using System.Threading.Tasks;
using MarketPlace.Application.Services.interfaces;
using MarketPlace.DataLayer.DTOs.Account;
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
        private readonly ISmsService _smsService;
        public UserService(IGenericRepository<User> usesRepository, IPasswordHelper passwordHelper, ISmsService smsService)
        {
            _usesRepository = usesRepository;
            _passwordHelper = passwordHelper;
            _smsService = smsService;
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
                    Password = _passwordHelper.EncodePasswordMd5(register.Password),
                    MobileActiveCode = new Random().Next(10000, 99999).ToString(),
                    EmailActiveCode = Guid.NewGuid().ToString("N")
                };
                await _usesRepository.AddEntity(user);
                await _usesRepository.SaveChanges();
                //await _smsService.SendVerfiySms(user.Mobile,user.MobileActiveCode);
                Console.WriteLine(user.MobileActiveCode);
                return RegisterUserResult.Success;
            }
            return RegisterUserResult.MobileExists;

            //return RegisterUserResult.Error;
        }

        public async Task<bool> IsUserExistsByMobileNumber(string mobile)
        {
            return await _usesRepository.GetQuery().AsQueryable().AnyAsync(x => x.Mobile == mobile);
        }

        public async Task<LoginUserResult> GetUserForLogin(LoginUserDTO login)
        {
            var user = await _usesRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == login.Mobile);
            if (user == null) return LoginUserResult.NotFound;
            if (!user.IsMobileActive) return LoginUserResult.NotActivated;
            if (user.Password != _passwordHelper.EncodePasswordMd5(login.Password)) return LoginUserResult.NotFound;
            return LoginUserResult.Success;
        }

        public async Task<User> GetUserByMobile(string mobile)
        {
            return await _usesRepository.GetQuery().AsQueryable().SingleOrDefaultAsync(x => x.Mobile == mobile);
        }

        public async Task<ForgotPassUserResult> RecoverUserPassword(ForgotPassUserDTO forgot)
        {
            var user = await _usesRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == forgot.Mobile);
            if (user == null) return ForgotPassUserResult.NotFound;
            var newPass = new Random().Next(100000, 999999).ToString();
            user.Password = _passwordHelper.EncodePasswordMd5(newPass);
            //await _smsService.SendUserPassword(user.Mobile, newPass);
            Console.WriteLine(newPass);
            await _usesRepository.SaveChanges();
            return ForgotPassUserResult.Success;
        }

        public async Task<bool> ActiveMobile(ActivateMobileDTO activate)
        {
            var user = await _usesRepository.GetQuery().AsQueryable()
                .SingleOrDefaultAsync(x => x.Mobile == activate.Mobile);
            if (user != null)
            {
                if (user.MobileActiveCode == activate.MobileActiveCode)
                {
                    user.IsMobileActive = true;
                    user.MobileActiveCode = new Random().Next(100000, 999999).ToString();
                    await _usesRepository.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> ChangePassword(ChangePasswordDTO changePasswordDto, long UserId)
        {
            var user = await _usesRepository.GetEntityById(UserId);
            if (user != null)
            {
                var newPass = _passwordHelper.EncodePasswordMd5(changePasswordDto.NewPassword);
                if (newPass != user.Password)
                {
                    user.Password = newPass;
                    _usesRepository.EditEntity(user);
                    await _usesRepository.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        public async Task<EditProfileDTO> GetProfileForEdit(long id)
        {
            var user = await _usesRepository.GetEntityById(id);
            if (user == null) return null;
            return new EditProfileDTO
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Avatar = user.Avatar
            };
        }

        public async Task<EditUserProfileResult> EditUserProfile(EditProfileDTO dto, long userId)
        {
            var user = await _usesRepository.GetEntityById(userId);
            if (user == null) return EditUserProfileResult.NotFound;

            if (user.IsBlocked) return EditUserProfileResult.IsBlocked;
            if (!user.IsMobileActive) return EditUserProfileResult.IsNotActive;

            user.FirstName = dto.FirstName;
            user.LastName = dto.LastName;
            _usesRepository.EditEntity(user);
            await _usesRepository.SaveChanges();
            return EditUserProfileResult.Success;
        }

        #endregion

    }
}
