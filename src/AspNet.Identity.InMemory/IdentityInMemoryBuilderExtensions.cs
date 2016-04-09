using Microsoft.AspNet.Identity;
using System.Reflection;

namespace AspNet.Identity.InMemory
{
    public static class IdentityInMemoryBuilderExtensions
    {
        public static IdentityBuilder AddInMemoryStores(this IdentityBuilder builder)
        {
            TypeInfo identityBuilderTypeInfo = typeof(IdentityBuilder).GetTypeInfo();
            var result = builder;
            result = AddInMemoryUserStore(result, identityBuilderTypeInfo);
            result = AddInMemoryRoleStore(result, identityBuilderTypeInfo);
            return result;            
        }

        private static IdentityBuilder AddInMemoryUserStore(IdentityBuilder builder, TypeInfo identityBuilderTypeInfo)
        {
            var userStoreType = typeof(InMemoryUserStore<>).MakeGenericType(builder.UserType);
            var addUserStoreMethod = identityBuilderTypeInfo.GetDeclaredMethod("AddUserStore").MakeGenericMethod(userStoreType);
            return addUserStoreMethod.Invoke(builder, null) as IdentityBuilder;
        }

        private static IdentityBuilder AddInMemoryRoleStore(IdentityBuilder builder, TypeInfo identityBuilderTypeInfo)
        {
            var roleStoreType = typeof(InMemoryRoleStore<>).MakeGenericType(builder.RoleType);
            var addRoleStoreMethod = identityBuilderTypeInfo.GetDeclaredMethod("AddRoleStore").MakeGenericMethod(roleStoreType);
            return addRoleStoreMethod.Invoke(builder, null) as IdentityBuilder;
        }
    }
}
