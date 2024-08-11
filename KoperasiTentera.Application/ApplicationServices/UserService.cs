using System;
using KoperasiTentera.Application.Common;
using KoperasiTentera.Application.DTOs;
using KoperasiTentera.Application.Enums;
using KoperasiTentera.Application.Interfaces;
using KoperasiTentera.Domain.Entities;

namespace KoperasiTentera.Application.ApplicationServices;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IOTPService _otpService;

    public UserService(IUserRepository userRepository, IOTPService otpService)
    {
        _userRepository = userRepository;
        _otpService = otpService;
    }

    public async Task RegisterUserAsync(RegisterRequestDto request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.EmailAddress) ??
                   await _userRepository.GetUserByMobileAsync(request.MobileNumber);

        if (user != null && user.IsMobileVerified && user.IsEmailVerified)
        {
            throw new UserAlreadyExistsException();
        }
        else if (user == null)
        {
            user = new User
            {
                FullName = request.FullName,
                ICNumber = request.ICNumber,
                MobileNumber = request.MobileNumber,
                EmailAddress = request.EmailAddress
            };

            await _userRepository.AddUserAsync(user);
        }

        // Generate and send OTPs
        string mobileOtp = _otpService.GenerateOtp(request.MobileNumber);
        string emailOtp = _otpService.GenerateOtp(request.EmailAddress);

        await _otpService.SendOtpAsync(request.MobileNumber, mobileOtp, OtpType.Mobile);
        await _otpService.SendOtpAsync(request.EmailAddress, emailOtp, OtpType.Email);
    }

    public async Task<bool> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        User user = request.OTPtypeCd switch
        {
            (int)OtpType.Email => await _userRepository.GetUserByEmailAsync(request.ContactInfo),
            (int)OtpType.Mobile => await _userRepository.GetUserByMobileAsync(request.ContactInfo),
            _ => throw new InvalidOtpException()
        } ?? throw new UserDoesNotExistException();

        bool isValid = _otpService.ValidateOtp(request.ContactInfo, request.Otp);

        if (isValid)
        {
            switch (request.OTPtypeCd)
            {
                case (int)OtpType.Mobile:
                    user.IsMobileVerified = true;
                    break;
                case (int)OtpType.Email:
                    user.IsEmailVerified = true;
                    break;
            }
            await _userRepository.UpdateUserAsync(user);
            return true;
        }
        throw new InvalidOtpException();
    }

    public async Task CreatePinAsync(CreatePinRequestDto request)
    {
        User user = await _userRepository.GetUserByEmailAsync(request.EmailAddress) ?? throw new UserDoesNotExistException();
        if (!user.IsEmailVerified || !user.IsMobileVerified)
        {
            throw new EmailOrMobileIsNotVerifiedException();
        }
        user.HashedPin = new Domain.ValueObjects.PIN(request.PIN).PinHash;
        await _userRepository.UpdateUserAsync(user);
    }
}