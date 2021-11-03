using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNet.DAL.EF.Models.Security;
using Microsoft.AspNet.Identity;

namespace Bex.Common.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        IUser CreateUser(string email, string userName);
        Task<IdentityResult> DeleteAsync(IUser user);
        Task<IdentityResult> RegisterUserAsync(IUser user);
        Task<IdentityResult> RegisterUserAsync(IUser user, string password);
        Task<bool> HasPasswordAsync(string userId);
        Task<bool> IsEmailConfirmedAsync(string userId);
        Task<IUser> FindUserByIdAsync(string userId);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<IUser> FindUserByEmailAsync(string userName);
        Task<string> GenerateEmailConfirmationTokenAsync(string userId);
        Task SendEmailAsync(string userId, string subject, string body);
        Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
        Task<string> GeneratePasswordResetTokenAsync(string userId);
        Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
        Task<string> GetPhoneNumberAsync(string userId);
        Task<bool> IsPhoneNumberConfirmedAsync(string userId);
        Task<IdentityResult> SetPhoneNumberAsync(string userId, string phoneNumber);
        Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber);
        Task SendSmsAsync(string userId, string message);
        Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token);
        Task<bool> GetTwoFactorEnabledAsync(string userId);
        Task<IdentityResult> SetTwoFactorEnabledAsync(string userId, bool enabled);
        Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId);
        Task<string> GenerateTwoFactorTokenAsync(string userId, string twoFactorProvider);
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
        Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
        Task<string> GetUserPasswordHashAsync(string userId);
        Task<IdentityResult> AddPasswordAsync(string userId, string newPassword);
        Task<bool> IsInRoleAsync(string userId, string role);

        Task<IList<Claim>> GetClaimsAsync(string userId);
        Task<IdentityResult> AddClaimAsync(string userId, Claim claim);
        Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim);

        Task<List<ApplicationUser>> GetUsersAsync();

        Task<bool> DeleteClaim(string userId, int claimId);

    }
}
