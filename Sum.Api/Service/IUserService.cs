using System.Threading.Tasks;
using Sum.Api.Models;

namespace Sum.Api.Service
{
    public interface IUserService
    {
        Task<AuthenticationResult> RegisterAsync(RegisterDto model);
        Task<AuthenticationResult> LoginAsync(LoginDto model);
    }
}   