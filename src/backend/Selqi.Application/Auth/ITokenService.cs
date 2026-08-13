using Selqi.Domain.Identity;

namespace Selqi.Application.Auth;

public interface ITokenService
{
    (string Token, DateTime ExpiresAt) Generate(ApplicationUser user);
}
