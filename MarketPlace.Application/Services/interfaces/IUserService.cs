using System;
using System.Threading.Tasks;
using MarketPlace.DataLayer.DTOs;
using MarketPlace.DataLayer.DTOs.Account;
using MarketPlace.DataLayer.Entities.Account;
using Microsoft.AspNetCore.Http;

namespace MarketPlace.Application.Services.interfaces
{
    public interface IUserService : IAsyncDisposable
    {
        #region account

        Task<RegisterUserResult> RegisterUser(RegisterUserDTO register);
        Task<bool> IsUserExistsByMobileNumber(string mobile);
        Task<LoginUserResult> GetUserForLogin(LoginUserDTO login);
        Task<User> GetUserByMobile(string mobile);
        Task<ForgotPassUserResult> RecoverUserPassword(ForgotPassUserDTO forgot);
        Task<bool> ActiveMobile(ActivateMobileDTO activate);
        Task<bool> ChangePassword(ChangePasswordDTO changePasswordDto, long UserId);
        Task<EditProfileDTO> GetProfileForEdit(long id);
        Task<EditUserProfileResult> EditUserProfile(EditProfileDTO editProfileDto,long userId ,IFormFile avatarImage);

        #endregion
    }
}
