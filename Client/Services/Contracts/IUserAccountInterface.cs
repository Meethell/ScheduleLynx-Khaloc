using Client.Data;
using Client.DTOs;
using Client.Entities.UserEntity;
using Client.Responses;
using System;
using System.Threading.Tasks;

namespace ClientLibrary.Services.Constracts
{
    public interface IUserAccountInterface
    {
        Task<GeneralResponse> CreateAsync(Register user);
        Task<LoginResponse> SignInAsync(Login user);
        Task<GeneralResponse> CheckActivated(int userId);
        Task<GeneralResponse> ChangeActivateAccountStatus(int userId, bool IsActivated);
        Task<GeneralResponse> UpdateUserAccountAsync(int userId, string newName, string newPassword);
    }
}
