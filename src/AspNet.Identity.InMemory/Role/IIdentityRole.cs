using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.InMemory
{
    public interface IIdentityRole
    {
        string RoleId { get; set; }
        string RoleName { get; set; }
        string NormalizedRoleName { get; set; }
        IList<Claim> Claims { get; }
    }
}
