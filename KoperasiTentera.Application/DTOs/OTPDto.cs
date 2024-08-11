using System;
namespace KoperasiTentera.Application.DTOs;

public class OTPDto
{
    public string OTPValue { get; set; }
    public DateTime Expiry { get; set; }
}

