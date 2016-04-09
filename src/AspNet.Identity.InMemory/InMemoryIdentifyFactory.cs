using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AspNet.Identity.InMemory
{
    public class InMemoryIdentifyFactory<TUser, TRole> 
        where TUser : class, IIdentityUser, new() 
        where TRole : class, IIdentityRole, new()
    {
        private readonly UserManager<TUser> _userManager;
        private readonly RoleManager<TRole> _roleManager;

        public InMemoryIdentifyFactory(UserManager<TUser> userManager, RoleManager<TRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public Task AddUserAsync(TUser user, string password)
        {
            return _userManager.CreateAsync(user, password);
        }

        public Task AddUserAsync(string userId, string userName, string password, string[] roles = null, Claim[] claims = null)
        {
            var user = new TUser()
            {
                UserId = userId,
                UserName = userName
            };
            if (roles != null)
            {
                foreach (var role in roles)
                {
                    user.Roles.Add(role);
                }
            }
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    user.Claims.Add(claim);
                }
            }
            return AddUserAsync(user, password);
        }

        public Task AddRoleAsync(TRole role)
        {
            return _roleManager.CreateAsync(role);
        }

        public Task AddRoleAsync(string roleId, string roleName, Claim[] claims = null)
        {
            var role = new TRole()
            {
                RoleId = roleId,
                RoleName = roleName
            };
            if (claims != null)
            {
                foreach (var claim in claims)
                {
                    role.Claims.Add(claim);
                }
            }
            return AddRoleAsync(role);
        }
    }
}
