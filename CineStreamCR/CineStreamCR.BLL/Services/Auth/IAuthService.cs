using CineStreamCR.BLL.DTO.Auth;
using CineStreamCR.DAL.Entities;

namespace CineStreamCR.BLL.Services.Auth
{
    public interface IAuthService
    {
        Task<Users?> LoginAsync(LoginDTO loginDTO);

        Task<bool> RegisterAsync(Users user);
    }
}
