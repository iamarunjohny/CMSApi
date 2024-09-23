using System.Threading.Tasks;
using CMSApi.Core.DTO;

namespace CMSApi.Core.Interfaces
{
    public interface IAuthService
    {
        Task<TokenModelDTO> Authenticate(UserLoginDTO userLoginDTO);
        Task<TokenModelDTO> RefreshTokenAsync(string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
