using System.Collections.Generic;
using System.Security.Claims;

namespace AspNet.Identity.InMemory
{
    internal class ClaimEqualityComparer : IEqualityComparer<Claim>
    {
        public bool Equals(Claim x, Claim y)
        {
            if (x.Equals(y))
            {
                return true;
            }
            return x.Issuer == y.Issuer
                && x.OriginalIssuer == y.OriginalIssuer
                && x.Subject == y.Subject
                && x.Type == y.Type
                && x.Value == y.Value
                && x.ValueType == y.ValueType;
        }

        public int GetHashCode(Claim obj)
        {
            return obj.Type.GetHashCode();
        }
    }
}
