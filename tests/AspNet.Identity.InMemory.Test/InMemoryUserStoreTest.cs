using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace AspNet.Identity.InMemory.Test
{
    public class InMemoryUserStoreTest
    {
        private readonly IdentityUser TestUser = new IdentityUser()
        {
            AccessFailedCount = 2,
            Email = "Test@Example.com",
            EmailConfirmed = true,
            LockoutEnabled = true,
            LockoutEndDate = new DateTime(2016, 1, 1),
            NormalizedEmail = "test@example.com",
            NormalizedUserName = "test",
            PasswordHash = "aaaaa",
            PhoneNumber = "555-555-5555",
            PhoneNumberConfirmed = true,
            SecurityStamp = "bbbbb",
            TwoFactorEnabled = true,
            UserId = "1",
            UserName = "Test"
        };

        public InMemoryUserStoreTest()
        {
            TestUser.Logins.Add(new UserLoginInfo("Provider1", "ProviderKey1", "DisplayName1"));
            TestUser.Logins.Add(new UserLoginInfo("Provider2", "ProviderKey2", "DisplayName2"));
            TestUser.Roles.Add("Role1");
            TestUser.Roles.Add("Role2");
            TestUser.Roles.Add("Role3");
            TestUser.Claims.Add(new Claim(ClaimTypes.Name, "Test"));
            TestUser.Claims.Add(new Claim(ClaimTypes.Email, "test@example.com"));
        }

        private InMemoryUserStore<IdentityUser> NewUserStore()
        {
            var userStore = new InMemoryUserStore<IdentityUser>();
            userStore.Clear();
            return userStore;
        }

        [Fact]
        public async void CreateUserTest()
        {
            var userStore = NewUserStore();
            var result = await userStore.CreateAsync(TestUser, CancellationToken.None);
            Assert.True(result.Succeeded);
            Assert.False(result.Errors.Any());
            Assert.Equal(1, userStore.Users.Count());            
        }
       
        [Fact]
        public async void FindUserTest()
        {
            var userStore = NewUserStore();            
            await userStore.CreateAsync(TestUser, CancellationToken.None);
            
            // Find by ID
            var u = await userStore.FindByIdAsync(TestUser.UserId, CancellationToken.None);
            Assert.Equal(TestUser.UserId, u.UserId);

            // Find by name
            u = await userStore.FindByNameAsync(TestUser.NormalizedUserName, CancellationToken.None);
            Assert.Equal(TestUser.UserId, u.UserId);

            // Find by email
            u = await userStore.FindByEmailAsync(TestUser.NormalizedEmail, CancellationToken.None);
            Assert.Equal(TestUser.UserId, u.UserId);

            // Find by login
            u = await userStore.FindByLoginAsync("Provider1", "ProviderKey1", CancellationToken.None);
            Assert.Equal(TestUser.UserId, u.UserId);
            u = await userStore.FindByLoginAsync("Provider2", "ProviderKey2", CancellationToken.None);
            Assert.Equal(TestUser.UserId, u.UserId);            

            // Finding users that do not exist
            Assert.Null(await userStore.FindByIdAsync("0", CancellationToken.None));
            Assert.Null(await userStore.FindByNameAsync("NotThere", CancellationToken.None));
            Assert.Null(await userStore.FindByEmailAsync("notthere@example.com", CancellationToken.None));
            Assert.Null(await userStore.FindByLoginAsync("Provider1", "ProviderKey2", CancellationToken.None));
            Assert.Null(await userStore.FindByLoginAsync("Provider2", "ProviderKey1", CancellationToken.None));
            Assert.Null(await userStore.FindByLoginAsync("Provider1", "", CancellationToken.None));
            Assert.Null(await userStore.FindByLoginAsync("", "ProviderKey1", CancellationToken.None));
        }

        [Fact]
        public async void GetTest()
        {
            var userStore = NewUserStore();
            Assert.Equal(TestUser.AccessFailedCount, await userStore.GetAccessFailedCountAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.Claims.Count, (await userStore.GetClaimsAsync(TestUser, CancellationToken.None)).Count);
            Assert.Equal(TestUser.Email, await userStore.GetEmailAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.EmailConfirmed, await userStore.GetEmailConfirmedAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.LockoutEnabled, await userStore.GetLockoutEnabledAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.LockoutEndDate, await userStore.GetLockoutEndDateAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.Logins.Count, (await userStore.GetLoginsAsync(TestUser, CancellationToken.None)).Count);
            Assert.Equal(TestUser.NormalizedEmail, await userStore.GetNormalizedEmailAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.NormalizedUserName, await userStore.GetNormalizedUserNameAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.PasswordHash, await userStore.GetPasswordHashAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.PhoneNumber, await userStore.GetPhoneNumberAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.PhoneNumberConfirmed, await userStore.GetPhoneNumberConfirmedAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.Roles.Count, (await userStore.GetRolesAsync(TestUser, CancellationToken.None)).Count);
            Assert.Equal(TestUser.SecurityStamp, await userStore.GetSecurityStampAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.TwoFactorEnabled, await userStore.GetTwoFactorEnabledAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.UserId, await userStore.GetUserIdAsync(TestUser, CancellationToken.None));
            Assert.Equal(TestUser.UserName, await userStore.GetUserNameAsync(TestUser, CancellationToken.None));
        }

        [Fact]
        public async void SetTest()
        {
            var userStore = NewUserStore();
            var u = new IdentityUser();
            
            await userStore.SetEmailAsync(u, TestUser.Email, CancellationToken.None);
            Assert.Equal(TestUser.Email, u.Email);

            await userStore.SetEmailConfirmedAsync(u, TestUser.EmailConfirmed, CancellationToken.None);
            Assert.Equal(TestUser.EmailConfirmed, u.EmailConfirmed);

            await userStore.SetLockoutEnabledAsync(u, TestUser.LockoutEnabled, CancellationToken.None);
            Assert.Equal(TestUser.LockoutEnabled, u.LockoutEnabled);

            await userStore.SetLockoutEndDateAsync(u, TestUser.LockoutEndDate, CancellationToken.None);
            Assert.Equal(TestUser.LockoutEndDate, u.LockoutEndDate);

            await userStore.SetNormalizedEmailAsync(u, TestUser.NormalizedEmail, CancellationToken.None);
            Assert.Equal(TestUser.NormalizedEmail, u.NormalizedEmail);

            await userStore.SetNormalizedUserNameAsync(u, TestUser.NormalizedUserName, CancellationToken.None);
            Assert.Equal(TestUser.NormalizedUserName, u.NormalizedUserName);

            await userStore.SetPasswordHashAsync(u, TestUser.PasswordHash, CancellationToken.None);
            Assert.Equal(TestUser.PasswordHash, u.PasswordHash);

            await userStore.SetPhoneNumberAsync(u, TestUser.PhoneNumber, CancellationToken.None);
            Assert.Equal(TestUser.PhoneNumber, u.PhoneNumber);

            await userStore.SetPhoneNumberConfirmedAsync(u, TestUser.PhoneNumberConfirmed, CancellationToken.None);
            Assert.Equal(TestUser.PhoneNumberConfirmed, u.PhoneNumberConfirmed);

            await userStore.SetSecurityStampAsync(u, TestUser.SecurityStamp, CancellationToken.None);
            Assert.Equal(TestUser.SecurityStamp, u.SecurityStamp);

            await userStore.SetTwoFactorEnabledAsync(u, TestUser.TwoFactorEnabled, CancellationToken.None);
            Assert.Equal(TestUser.TwoFactorEnabled, u.TwoFactorEnabled); 
                       
            await userStore.SetUserNameAsync(u, TestUser.UserName, CancellationToken.None);
            Assert.Equal(TestUser.UserName, u.UserName);            
        }

        [Fact]
        public async void AccessFailedTest()
        {
            var userStore = NewUserStore();            
            await userStore.CreateAsync(TestUser, CancellationToken.None);
            var x = await userStore.GetAccessFailedCountAsync(TestUser, CancellationToken.None);

            // Increment
            Assert.Equal(x + 1, await userStore.IncrementAccessFailedCountAsync(TestUser, CancellationToken.None));
            Assert.Equal(x + 1, await userStore.GetAccessFailedCountAsync(TestUser, CancellationToken.None));

            // Increment again
            Assert.Equal(x + 2, await userStore.IncrementAccessFailedCountAsync(TestUser, CancellationToken.None));
            Assert.Equal(x + 2, await userStore.GetAccessFailedCountAsync(TestUser, CancellationToken.None));

            // Reset
            await userStore.ResetAccessFailedCountAsync(TestUser, CancellationToken.None);
            Assert.Equal(0, await userStore.GetAccessFailedCountAsync(TestUser, CancellationToken.None));
        }

        [Fact]
        public async void RolesTest()
        {
            var userStore = NewUserStore();
            var u = new IdentityUser();

            // Add soem roles
            await userStore.AddToRoleAsync(u, "Role1", CancellationToken.None);
            await userStore.AddToRoleAsync(u, "Role2", CancellationToken.None);

            // Check to make sure the roles are there
            var roles = await userStore.GetRolesAsync(u, CancellationToken.None);
            Assert.Equal(2, roles.Count);
            Assert.True(roles.Contains("Role1"));
            Assert.True(roles.Contains("Role2"));

            // Check if user is in various roles
            Assert.True(await userStore.IsInRoleAsync(u, "Role1", CancellationToken.None));
            Assert.True(await userStore.IsInRoleAsync(u, "Role2", CancellationToken.None));
            Assert.False(await userStore.IsInRoleAsync(u, "Role3", CancellationToken.None));

            // Remove user from a role that do not belong to
            await userStore.RemoveFromRoleAsync(u, "Role3", CancellationToken.None);
            Assert.Equal(2, roles.Count);
            Assert.True(await userStore.IsInRoleAsync(u, "Role1", CancellationToken.None));
            Assert.True(await userStore.IsInRoleAsync(u, "Role2", CancellationToken.None));
            Assert.False(await userStore.IsInRoleAsync(u, "Role3", CancellationToken.None));

            // Remove user from Role1
            await userStore.RemoveFromRoleAsync(u, "Role1", CancellationToken.None);
            Assert.Equal(1, roles.Count);
            Assert.False(await userStore.IsInRoleAsync(u, "Role1", CancellationToken.None));
            Assert.True(await userStore.IsInRoleAsync(u, "Role2", CancellationToken.None));

            // Remove user from Role2
            await userStore.RemoveFromRoleAsync(u, "Role2", CancellationToken.None);
            Assert.Equal(0, roles.Count);
            Assert.False(await userStore.IsInRoleAsync(u, "Role2", CancellationToken.None));
        }

        [Fact]
        public async void LoginsTest()
        {
            var userStore = NewUserStore();
            var u = new IdentityUser();

            // Add some logins
            await userStore.AddLoginAsync(u, new UserLoginInfo("Provider1", "ProviderKey1", "DisplayName1"), CancellationToken.None);
            await userStore.AddLoginAsync(u, new UserLoginInfo("Provider2", "ProviderKey2", "DisplayName2"), CancellationToken.None);

            // Check to make sure the logins are there
            var logins = await userStore.GetLoginsAsync(u, CancellationToken.None);
            Assert.Equal(2, logins.Count);
            Assert.True(logins.Any(x => x.LoginProvider == "Provider1" && x.ProviderKey == "ProviderKey1" && x.ProviderDisplayName == "DisplayName1"));
            Assert.True(logins.Any(x => x.LoginProvider == "Provider2" && x.ProviderKey == "ProviderKey2" && x.ProviderDisplayName == "DisplayName2"));

            // Remove login
            await userStore.RemoveLoginAsync(u, "Provider1", "ProviderKey1", CancellationToken.None);
            Assert.Equal(1, logins.Count);
            Assert.False(logins.Any(x => x.LoginProvider == "Provider1" && x.ProviderKey == "ProviderKey1" && x.ProviderDisplayName == "DisplayName1"));
            Assert.True(logins.Any(x => x.LoginProvider == "Provider2" && x.ProviderKey == "ProviderKey2" && x.ProviderDisplayName == "DisplayName2"));

            // Remove login
            await userStore.RemoveLoginAsync(u, "Provider2", "ProviderKey2", CancellationToken.None);
            Assert.Equal(0, logins.Count);
            Assert.False(logins.Any(x => x.LoginProvider == "Provider2" && x.ProviderKey == "ProviderKey2" && x.ProviderDisplayName == "DisplayName2"));
        }

        [Fact]
        public async void ClaimsTest()
        {
            var userStore = NewUserStore();
            var u = new IdentityUser();

            // Add some claims
            await userStore.AddClaimsAsync(
                u,
                new[] { new Claim(ClaimTypes.Name, "Test"), new Claim(ClaimTypes.Email, "test@example.com") }, 
                CancellationToken.None);

            // Check to make sure the claims are there
            var claims = await userStore.GetClaimsAsync(u, CancellationToken.None);
            Assert.Equal(2, claims.Count);
            Assert.Equal(1, claims.Count(x => x.Type == ClaimTypes.Name && x.Value == "Test"));
            Assert.Equal(1, claims.Count(x => x.Type == ClaimTypes.Email && x.Value == "test@example.com"));

            // Replace claim
            await userStore.ReplaceClaimAsync(u, new Claim(ClaimTypes.Name, "Test"), new Claim(ClaimTypes.Name, "Test2"), CancellationToken.None);
            Assert.Equal(2, claims.Count);
            Assert.Equal(0, claims.Count(x => x.Type == ClaimTypes.Name && x.Value == "Test"));
            Assert.Equal(1, claims.Count(x => x.Type == ClaimTypes.Name && x.Value == "Test2"));

            // Try to remove a claim that is not there
            await userStore.RemoveClaimsAsync(u, new[] { new Claim(ClaimTypes.Name, "NotThere") }, CancellationToken.None);
            claims = await userStore.GetClaimsAsync(u, CancellationToken.None);
            Assert.Equal(2, claims.Count);

            // Remove the claims
            await userStore.RemoveClaimsAsync(u, new[] { new Claim(ClaimTypes.Name, "Test2") }, CancellationToken.None);
            claims = await userStore.GetClaimsAsync(u, CancellationToken.None);
            Assert.Equal(1, claims.Count);
            await userStore.RemoveClaimsAsync(u, new[] { new Claim(ClaimTypes.Email, "test@example.com") }, CancellationToken.None);
            claims = await userStore.GetClaimsAsync(u, CancellationToken.None);
            Assert.Equal(0, claims.Count);
        }

    }
}
