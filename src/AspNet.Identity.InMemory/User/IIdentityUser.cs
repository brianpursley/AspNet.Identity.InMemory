using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.InMemory
{
    public interface IIdentityUser
    {
        string UserId { get; set; }
        string UserName { get; set; }
        string NormalizedUserName { get; set; }
        string Email { get; set; }
        string NormalizedEmail { get; set; }
        bool EmailConfirmed { get; set; }
        string PhoneNumber { get; set; }
        bool PhoneNumberConfirmed { get; set; }
        string PasswordHash { get; set; }
        string SecurityStamp { get; set; }
        bool TwoFactorEnabled { get; set; }
        bool LockoutEnabled { get; set; }
        DateTimeOffset? LockoutEndDate { get; set; }
        int AccessFailedCount { get; set; }
        IList<UserLoginInfo> Logins { get; }
        IList<string> Roles { get; }
        IList<Claim> Claims { get; }
    }
}
