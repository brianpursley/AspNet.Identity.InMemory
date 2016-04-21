using System.Linq;
using System.Security.Claims;
using System.Threading;
using Xunit;

namespace AspNet.Identity.InMemory.Test
{
    public class InMemoryRoleStoreTest
    {
        private readonly IdentityRole TestRole = new IdentityRole()
        {
            RoleId = "1",
            RoleName = "Test",
            NormalizedRoleName = "test"
        };

        public InMemoryRoleStoreTest()
        {
            TestRole.Claims.Add(new Claim("ClaimType1", "ClaimValue1"));
            TestRole.Claims.Add(new Claim("ClaimType2", "ClaimValue2"));
        }

        private InMemoryRoleStore<IdentityRole> NewRoleStore()
        {
            var roleStore = new InMemoryRoleStore<IdentityRole>();
            roleStore.Clear();
            return roleStore;
        }

        [Fact]
        public async void CreateRoleTest()
        {
            var roleStore = NewRoleStore();
            var result = await roleStore.CreateAsync(TestRole, CancellationToken.None);
            Assert.True(result.Succeeded);
            Assert.False(result.Errors.Any());
            Assert.Equal(1, roleStore.Roles.Count());
        }
       
        [Fact]
        public async void FindRoleTest()
        {
            var roleStore = NewRoleStore();
            await roleStore.CreateAsync(TestRole, CancellationToken.None);

            // Find by ID
            var r = await roleStore.FindByIdAsync(TestRole.RoleId, CancellationToken.None);
            Assert.Equal(TestRole.RoleId, r.RoleId);

            // Find by name
            r = await roleStore.FindByNameAsync(TestRole.NormalizedRoleName, CancellationToken.None);
            Assert.Equal(TestRole.RoleId, r.RoleId);

            // Finding roles that do not exist
            Assert.Null(await roleStore.FindByIdAsync("0", CancellationToken.None));
            Assert.Null(await roleStore.FindByNameAsync("NotThere", CancellationToken.None));
        }

        [Fact]
        public async void GetTest()
        {
            var roleStore = NewRoleStore();
            Assert.Equal(TestRole.Claims.Count, (await roleStore.GetClaimsAsync(TestRole, CancellationToken.None)).Count);
            Assert.Equal(TestRole.NormalizedRoleName, await roleStore.GetNormalizedRoleNameAsync(TestRole, CancellationToken.None));
            Assert.Equal(TestRole.RoleId, await roleStore.GetRoleIdAsync(TestRole, CancellationToken.None));
            Assert.Equal(TestRole.RoleName, await roleStore.GetRoleNameAsync(TestRole, CancellationToken.None));
        }

        [Fact]
        public async void SetTest()
        {
            var roleStore = NewRoleStore();
            var r = new IdentityRole();

            await roleStore.SetNormalizedRoleNameAsync(r, TestRole.NormalizedRoleName, CancellationToken.None);
            Assert.Equal(TestRole.NormalizedRoleName, r.NormalizedRoleName);

            await roleStore.SetRoleNameAsync(r, TestRole.RoleName, CancellationToken.None);
            Assert.Equal(TestRole.RoleName, r.RoleName);            
        }

        [Fact]
        public async void ClaimsTest()
        {
            var roleStore = NewRoleStore();
            var r = new IdentityRole();

            // Add some claims
            await roleStore.AddClaimAsync(r, new Claim("ClaimType1", "ClaimValue1"), CancellationToken.None);
            await roleStore.AddClaimAsync(r, new Claim("ClaimType2", "ClaimValue2"), CancellationToken.None);

            // Check to make sure the claims are there
            var claims = await roleStore.GetClaimsAsync(r, CancellationToken.None);
            Assert.Equal(2, claims.Count);
            Assert.Equal(1, claims.Count(x => x.Type == "ClaimType1" && x.Value == "ClaimValue1"));
            Assert.Equal(1, claims.Count(x => x.Type == "ClaimType2" && x.Value == "ClaimValue2"));

            // Try to remove a claim that is not there
            await roleStore.RemoveClaimAsync(r, new Claim("ClaimType1", "NotThere"), CancellationToken.None);
            claims = await roleStore.GetClaimsAsync(r, CancellationToken.None);
            Assert.Equal(2, claims.Count);

            // Remove the claims
            await roleStore.RemoveClaimAsync(r, new Claim("ClaimType1", "ClaimValue1"), CancellationToken.None);
            claims = await roleStore.GetClaimsAsync(r, CancellationToken.None);
            Assert.Equal(1, claims.Count);
            await roleStore.RemoveClaimAsync(r, new Claim("ClaimType2", "ClaimValue2"), CancellationToken.None);
            claims = await roleStore.GetClaimsAsync(r, CancellationToken.None);
            Assert.Equal(0, claims.Count);
        }

    }
}
