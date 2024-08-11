using System;
using KoperasiTentera.Application.DTOs;

namespace KoperasiTentera.Application.Interfaces;

public interface IUserService
{
    Task RegisterUserAsync(RegisterRequestDto request);
    Task<bool> VerifyOtpAsync(VerifyOtpRequestDto request);
    Task CreatePinAsync(CreatePinRequestDto request);
    Task<UserDto> LoginInitiationAsync(LoginInitiationRequestDto request);
    Task<UserDto> LoginCompletionAsync(LoginCompletionRequestDto request);
}

