using System;
using KoperasiTentera.Application.Enums;

namespace KoperasiTentera.Application.Interfaces;

public interface IOTPService
{
    string GenerateOtp(string contactInfo);
    bool ValidateOtp(string contactInfo, string code);
    Task SendOtpAsync(string identifier, string otp, OtpType otpType);

}

