using System;
using System.Net.Mail;
using FluentValidation;
using KoperasiTentera.Application.Enums;

namespace KoperasiTentera.Application.DTOs;

public class VerifyOtpRequestDto
{
    public required string ContactInfo { get; set; }
    public required string Otp { get; set; }
    public required int OTPtypeCd { get; set; }
}

public class VerifyOtpRequestValidator : AbstractValidator<VerifyOtpRequestDto>
{
    public VerifyOtpRequestValidator()
    {
        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required.")
            .Matches(@"^\d{6}$").WithMessage("OTP must be exactly 6 digits long.");

        RuleFor(x => x.OTPtypeCd)
            .Must(BeAValidEnumValue).WithMessage("The number must be a valid value of OtpType.");

        RuleFor(x => x.ContactInfo)
            .NotEmpty().WithMessage("Contact Info is required.")
            .Must((dto, contactInfo) => ValidateContactInfo(dto.OTPtypeCd, contactInfo))
            .WithMessage("Contact Info is not valid for the specified OtpType.");
        

    }

    private static bool BeAValidEnumValue(int number)
    {
        return Enum.IsDefined(typeof(OtpType), number);
    }
    private static bool ValidateContactInfo(int otptypeCd, string contactInfo)
    {
        return otptypeCd == (int)OtpType.Email ? IsValidEmail(contactInfo) : otptypeCd == (int)OtpType.Mobile && IsValidMobileNumber(contactInfo);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var mailAddress = new MailAddress(email);
            return mailAddress.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidMobileNumber(string mobileNumber)
    {
        return !string.IsNullOrEmpty(mobileNumber) && mobileNumber.Length == 10 && mobileNumber.All(char.IsDigit);
    }
}