using MedicTrack.Infrastructure.IdentityModels;

namespace MedicTrack.Infrastructure.Services;

public interface IJwtService
{
    string GenerateToken(AppUser user);
}