using System;
using KoperasiTentera.Application.DTOs;

namespace KoperasiTentera.Application.Interfaces;

public interface IUserService
{
    Task<GenericResponse<UserDto>> RegisterUserAsync(RegisterRequestDto request);
    Task<GenericResponse<bool>> VerifyOtpAsync(VerifyOtpRequestDto request);
    Task CreatePinAsync(CreatePinRequestDto request);
    Task<GenericResponse<UserDto>> LoginInitiationAsync(LoginInitiationRequestDto request);
    Task<GenericResponse<UserDto>> LoginCompletionAsync(LoginCompletionRequestDto request);
}

