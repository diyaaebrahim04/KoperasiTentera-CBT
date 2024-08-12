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

        await SendOtp(request.MobileNumber, OtpType.Mobile);
    }

    public async Task<UserDto> LoginInitiationAsync(LoginInitiationRequestDto request)
    {
        User user = await _userRepository.GetUserByICNumberAsync(request.ICNumber) ?? throw new UserDoesNotExistException();

        if (!user.IsMobileVerified)
        {
            await SendOtp(user.MobileNumber, OtpType.Mobile);
        }
        else
        {
            if (!user.IsEmailVerified)
            {
                await SendOtp(user.EmailAddress, OtpType.Email);
            }
        }
        UserDto userDto = new()
        {
            FullName = user.FullName,
            ICNumber = user.ICNumber,
            IsEmailVerified = user.IsEmailVerified,
            IsMobileVerified = user.IsMobileVerified,
            EmailAddress = MaskEmail(user.EmailAddress),
            MobileNumber = MaskPhoneNumber(user.MobileNumber)
        };
        return userDto;
    }

    public async Task<UserDto> LoginCompletionAsync(LoginCompletionRequestDto request)
    {
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

        return userDto;
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
                    _ = SendOtp(user.EmailAddress, OtpType.Email);
                    break;
                case (int)OtpType.Email:
                    user.IsEmailVerified = true;
                    break;
            }
            await _userRepository.UpdateUserAsync(user);
            _otpService.ForgetOtp(request.ContactInfo);
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

    private async Task SendOtp(string contactInfo, OtpType otpType)
    {
        string otp = _otpService.GenerateOtp(contactInfo);
        await _otpService.SendOtpAsync(contactInfo, otp, otpType);
    }
    private string MaskEmail(string email)
    {
        int atIndex = email.IndexOf('@');
        int dotIndex = email.LastIndexOf('.');

        string firstPart = email[..2];
        string domain = email[dotIndex..];
        string maskedEmail = $"{firstPart}****@****{domain}";

        return maskedEmail;
    }

    private string MaskPhoneNumber(string phoneNumber)
    {
        string lastFourDigits = phoneNumber[^4..];
        string maskedPhoneNumber = new string('*', phoneNumber.Length - 4) + lastFourDigits;

        return maskedPhoneNumber;
    }
}