using AuthApi.Application.DTOs;
using SharedLib.Responses;

namespace AuthApi.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> Register(AppUserDTO appUserDTO);
        Task<Response> Login(LoginDTO loginDTO);

        Task<GetUserDTO> GetUser(int userID);
    }
}
