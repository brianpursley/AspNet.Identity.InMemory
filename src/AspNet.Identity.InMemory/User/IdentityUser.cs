using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.InMemory
{
    public class IdentityUser : IIdentityUser
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEndDate { get; set; }
        public int AccessFailedCount { get; set; }
        public IList<UserLoginInfo> Logins { get; private set; } = new List<UserLoginInfo>();
        public IList<string> Roles { get; private set; } = new List<string>();
        public IList<Claim> Claims { get; private set; } = new List<Claim>();
    }
}
