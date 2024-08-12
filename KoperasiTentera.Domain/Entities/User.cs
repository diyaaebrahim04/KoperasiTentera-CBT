using System;
namespace KoperasiTentera.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string ICNumber { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }
    public string? HashedPin { get; set; }
    public bool IsMobileVerified { get; set; } = false;
    public bool IsEmailVerified { get; set; } = false;
    public bool BiometricEnabled { get; set; } = false;

    public string MaskedEmail()
    {
        int dotIndex = EmailAddress.LastIndexOf('.');

        string firstPart = EmailAddress[..2];
        string domain = EmailAddress[dotIndex..];
        string maskedEmail = $"{firstPart}****@****{domain}";

        return maskedEmail;
    }

    public string MaskedPhoneNumber()
    {
        string lastFourDigits = MobileNumber[^4..];
        string maskedPhoneNumber = new string('*', MobileNumber.Length - 4) + lastFourDigits;

        return maskedPhoneNumber;
    }
}

