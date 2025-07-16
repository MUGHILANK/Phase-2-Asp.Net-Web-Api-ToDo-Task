using Microsoft.AspNetCore.Identity;

namespace todoTask.Repositories.Tokens
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user,List<string>roles);
    }
}
