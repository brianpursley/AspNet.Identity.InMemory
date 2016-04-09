using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.InMemory
{
    public class IdentityRole : IIdentityRole
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string NormalizedRoleName { get; set; }
        public IList<Claim> Claims { get; private set; } = new List<Claim>();
    }
}
