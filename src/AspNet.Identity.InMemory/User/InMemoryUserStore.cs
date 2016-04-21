using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AspNet.Identity.InMemory
{
    public class InMemoryUserStore<TUser> :
        IQueryableUserStore<TUser>,
        IUserClaimStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserLoginStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IUserRoleStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserStore<TUser>,
        IUserTwoFactorStore<TUser>
        where TUser : class, IIdentityUser
    {
        private readonly static IList<TUser> _users = new List<TUser>();
        private readonly static ClaimEqualityComparer _claimEqualityComparer = new ClaimEqualityComparer();

        #region IQueryableUserStore

        public IQueryable<TUser> Users
        {
            get
            {
                return _users.AsQueryable();
            }
        }

        #endregion

        #region IUserClaimStore

        public Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Claims);
        }

        public async Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                await AddClaimAsync(user, claim, cancellationToken);
            }
        }

        public async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            await RemoveClaimAsync(user, claim, cancellationToken);
            if (newClaim != null)
            {
                await AddClaimAsync(user, newClaim, cancellationToken);
            }
        }

        private async Task AddClaimAsync(TUser user, Claim claim, CancellationToken cancellationToken)
        {
            await RemoveClaimAsync(user, claim, cancellationToken);
            user.Claims.Add(claim);
        }

        private Task RemoveClaimAsync(TUser user, Claim claim, CancellationToken cancellationToken)
        {
            var existingClaim = user.Claims.SingleOrDefault(x => _claimEqualityComparer.Equals(x, claim));
            if (existingClaim != null)
            {
                user.Claims.Remove(existingClaim);
            }
            return Task.FromResult(0);
        }

        public async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            foreach (var claim in claims)
            {
                await RemoveClaimAsync(user, claim, cancellationToken);
            }
        }

        public Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.Where(x => x.Claims.Contains(claim, _claimEqualityComparer)).ToList() as IList<TUser>);
        }

        #endregion

        #region IUserEmailStore

        public Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
        {
            user.Email = email;
            return Task.FromResult(0);
        }

        public Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Email);
        }

        public Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.EmailConfirmed = confirmed;
            return Task.FromResult(0);
        }

        public Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.NormalizedEmail == normalizedEmail));
        }

        public Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedEmail);
        }

        public Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            user.NormalizedEmail = normalizedEmail;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserLockoutStore

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEndDate);
        }

        public Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            user.LockoutEndDate = lockoutEnd;
            return Task.FromResult(0);
        }

        public Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            user.AccessFailedCount = 0;
            return Task.FromResult(0);
        }

        public Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.AccessFailedCount);
        }

        public Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.LockoutEnabled);
        }

        public Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserLoginStore

        public async Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken)
        {
            await RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey, cancellationToken);
            user.Logins.Add(login);
        }

        public Task<TUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.Logins.Any(y => y.LoginProvider == loginProvider && y.ProviderKey == providerKey)));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Logins);
        }

        public Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            var existingLogin = user.Logins.SingleOrDefault(x => x.LoginProvider == loginProvider && x.ProviderKey == providerKey);
            if (existingLogin != null)
            {
                user.Logins.Remove(existingLogin);
            }
            return Task.FromResult(0);
        }

        #endregion

        #region IUserPasswordStore

        public Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }

        public Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserPhoneNumberStore

        public Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumber);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
        {
            user.PhoneNumber = phoneNumber;
            return Task.FromResult(0);
        }

        public Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
        {
            user.PhoneNumberConfirmed = confirmed;
            return Task.FromResult(0);
        }

        #endregion

        #region IUserRoleStore

        public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            await RemoveFromRoleAsync(user, roleName, cancellationToken);
            user.Roles.Add(roleName);
        }

        public Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles);
        }

        public Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.Where(x => x.Roles.Contains(roleName)).ToList() as IList<TUser>);
        }

        public Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Roles.Contains(roleName));
        }

        public Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
        {
            if (user.Roles.Contains(roleName))
            {
                user.Roles.Remove(roleName);
            }
            return Task.FromResult(0);
        }

        #endregion

        #region ISecurityStampStore

        public Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken)
        {
            user.SecurityStamp = stamp;
            return Task.FromResult(0);
        }

        public Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.SecurityStamp);
        }

        #endregion

        #region IUserStore

        public Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
        {
            lock (_users)
            {
                var existingUser = FindUser(user);
                if (existingUser != null)
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "User already exists" }));
                }
                _users.Add(user);
            }
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
        {
            lock (_users)
            {
                var existingUser = FindUser(user);
                if (existingUser == null)
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "User not found" }));
                }
                _users.Remove(existingUser);
            }
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.UserId == userId));
        }

        public Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_users.SingleOrDefault(x => x.NormalizedUserName == normalizedUserName));
        }

        public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserId);
        }

        public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
        {
            lock (_users)
            {
                var existingUser = FindUser(user);
                if (existingUser == null)
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "User not found" }));
                }
                _users.Remove(existingUser);
                _users.Add(user);
            }
            return Task.FromResult(IdentityResult.Success);
        }

        private TUser FindUser(TUser user)
        {
            return
                _users.SingleOrDefault(x => x.UserId == user.UserId) ??
                _users.SingleOrDefault(x => x.NormalizedUserName == user.NormalizedUserName);
        }

        #endregion

        #region IUserTwoFactorStore

        public Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
        {
            user.TwoFactorEnabled = enabled;
            return Task.FromResult(0);
        }

        #endregion

        public void Clear()
        {
            _users.Clear();
        }
    }
}
