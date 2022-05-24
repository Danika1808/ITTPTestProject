using Microsoft.AspNetCore.Identity;

namespace Domain
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        public ApplicationRole(string name) : base(name) { }
    }
}
