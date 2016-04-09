using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace AspNet.Identity.InMemory
{
    public class InMemoryRoleStore<TRole> :
        IQueryableRoleStore<TRole>,
        IRoleClaimStore<TRole>,
        IRoleStore<TRole>        
        where TRole : class, IIdentityRole
    {
        private readonly static IList<TRole> _roles = new List<TRole>();

        #region IQueryableRoleStore

        public IQueryable<TRole> Roles
        {
            get
            {
                return _roles.AsQueryable();
            }
        }

        #endregion

        #region IRoleClaimStore

        public async Task AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            await RemoveClaimAsync(role, claim, cancellationToken);
            role.Claims.Add(claim);
        }

        public Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(role.Claims);
        }

        public Task RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default(CancellationToken))
        {
            var existingClaim = role.Claims.SingleOrDefault(x => x == claim);
            if (existingClaim != null)
            {
                role.Claims.Remove(existingClaim);
            }
            return Task.FromResult(0);
        }

        #endregion

        #region IRoleStore

        public Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
        {
            lock (_roles)
            {
                var existingRole = FindRole(role);
                if (existingRole != null)
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "Role already exists" }));
                }
                _roles.Add(role);
            }
            return Task.FromResult(IdentityResult.Success);
        }

        public Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
        {
            lock (_roles)
            {
                var existingRole = FindRole(role);
                if (existingRole == null)
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "Role not found" }));
                }
                _roles.Remove(existingRole);
            }
            return Task.FromResult(IdentityResult.Success);
        }

        public void Dispose()
        {
        }

        public Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            return Task.FromResult(_roles.SingleOrDefault(x => x.RoleId == roleId));
        }

        public Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            return Task.FromResult(_roles.SingleOrDefault(x => x.NormalizedRoleName == normalizedRoleName));
        }

        public Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.NormalizedRoleName);
        }

        public Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleId);
        }

        public Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
        {
            return Task.FromResult(role.RoleName);
        }

        public Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
        {
            role.NormalizedRoleName = normalizedName;
            return Task.FromResult(0);
        }

        public Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
        {
            role.RoleName = roleName;
            return Task.FromResult(0);
        }

        public Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
        {
            lock (_roles)
            {
                var existingRole = FindRole(role);
                if (existingRole == null)
                {
                    return Task.FromResult(IdentityResult.Failed(new IdentityError() { Description = "Role not found" }));
                }
                _roles.Remove(existingRole);
                _roles.Add(role);
            }
            return Task.FromResult(IdentityResult.Success);
        }

        private TRole FindRole(TRole role)
        {
            return
                _roles.SingleOrDefault(x => x.RoleId == role.RoleId) ??
                _roles.SingleOrDefault(x => x.RoleName == role.RoleName);
        }

        #endregion
    }
}
