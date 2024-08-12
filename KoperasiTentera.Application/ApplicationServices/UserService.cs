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

    public async Task<GenericResponse<UserDto>> RegisterUserAsync(RegisterRequestDto request)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.EmailAddress) ??
                   await _userRepository.GetUserByMobileAsync(request.MobileNumber);

        if (user != null)
        {
            throw new UserAlreadyExistsException();
        }

        user = new User
        {
            FullName = request.FullName,
            ICNumber = request.ICNumber,
            MobileNumber = request.MobileNumber,
            EmailAddress = request.EmailAddress
        };

        await _userRepository.AddUserAsync(user);

        UserDto userDto = new()
        {
            FullName = user.FullName,
            ICNumber = user.ICNumber,
            MobileNumber = user.MobileNumber,
            EmailAddress = user.EmailAddress
        };

        await SendOtp(request.MobileNumber, OtpType.Mobile);
        string message = "OTP has been sent to " + user.MobileNumber;
        return new GenericResponse<UserDto>(message, userDto);
    }

    public async Task<GenericResponse<UserDto>> LoginInitiationAsync(LoginInitiationRequestDto request)
    {
        string message = "";
        User user = await _userRepository.GetUserByICNumberAsync(request.ICNumber) ?? throw new UserDoesNotExistException();

        if (!user.IsMobileVerified)
        {
            await SendOtp(user.MobileNumber, OtpType.Mobile);
            message = "OTP has been sent to " + user.MaskedPhoneNumber();
        }
        else
        {
            if (!user.IsEmailVerified)
            {
                await SendOtp(user.EmailAddress, OtpType.Email);
                message = "OTP has been sent to " + user.MaskedEmail();
            }
        }
        UserDto userDto = new()
        {
            FullName = user.FullName,
            ICNumber = user.ICNumber,
            IsEmailVerified = user.IsEmailVerified,
            IsMobileVerified = user.IsMobileVerified,
            EmailAddress = user.MaskedEmail(),
            MobileNumber = user.MaskedPhoneNumber()
        };
        return new GenericResponse<UserDto>(message, userDto);
    }

    public async Task<GenericResponse<UserDto>> LoginCompletionAsync(LoginCompletionRequestDto request)
    {
        string message = "";
        User user = await _userRepository.GetUserByICNumberAsync(request.ICNumber) ?? throw new UserDoesNotExistException();

        if (user.HashedPin != new Domain.ValueObjects.PIN(request.PIN).PinHash)
        {
            throw new InvalidPINException();
        }

        if (!user.IsEmailVerified || !user.IsMobileVerified)
        {
            throw new EmailOrMobileIsNotVerifiedException();
        }

        if (request.BiometricEnabled != user.BiometricEnabled)
        {
            user.BiometricEnabled = request.BiometricEnabled;
            await _userRepository.UpdateUserAsync(user);
            message = "Biometric Enabled";
        }

        UserDto userDto = new()
        {
            FullName = user.FullName,
            ICNumber = user.ICNumber,
            IsEmailVerified = user.IsEmailVerified,
            IsMobileVerified = user.IsMobileVerified,
            EmailAddress = user.EmailAddress,
            MobileNumber = user.MobileNumber,
            BiometricEnabled = user.BiometricEnabled
        };

        return new GenericResponse<UserDto>(message, userDto);
    }
    public async Task<GenericResponse<bool>> VerifyOtpAsync(VerifyOtpRequestDto request)
    {
        string message = "";
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
                    _ = SendOtp(user.EmailAddress, OtpType.Email);
                    message = "Mobile Number verified. OTP has been sent to " + user.MaskedEmail();
                    break;
                case (int)OtpType.Email:
                    user.IsEmailVerified = true;
                    message = "Email address verified. Proceed to create PIN.";
                    break;
            }
            await _userRepository.UpdateUserAsync(user);
            _otpService.ForgetOtp(request.ContactInfo);
            return new GenericResponse<bool>(message, true);
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

    private async Task SendOtp(string contactInfo, OtpType otpType)
    {
        string otp = _otpService.GenerateOtp(contactInfo);
        await _otpService.SendOtpAsync(contactInfo, otp, otpType);
    }
}