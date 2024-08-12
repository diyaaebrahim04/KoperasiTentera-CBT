using System;
namespace KoperasiTentera.Application.DTOs;

public class UserDto
{
    public string FullName { get; set; }
    public string ICNumber { get; set; }
    public string MobileNumber { get; set; }
    public string EmailAddress { get; set; }
    public bool IsMobileVerified { get; set; }
    public bool IsEmailVerified { get; set; }
    public bool BiometricEnabled { get; set; }
}

