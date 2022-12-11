using System;

namespace MsFakes.Library
{
    public static class IdentityGenerator
    {
        public static string New()
        {
            var newGuid = Guid.NewGuid();
            return newGuid.ToString().Replace("-", string.Empty);
        }
    }
}
