using System;
using KoperasiTentera.Application.Interfaces;
using System.Collections.Concurrent;
using KoperasiTentera.Application.DTOs;

namespace KoperasiTentera.Application.ApplicationServices;

public class OTPStore : IOTPStore
{
    private readonly ConcurrentDictionary<string, OTPDto> _otpStorage = new();

    public void StoreOTP(string key, string otp)
    {
        _otpStorage[key] = new OTPDto
        {
            OTPValue = otp,
            Expiry = DateTime.Now.AddMinutes(10)
        };
    }

    public OTPDto? RetrieveOTP(string key, string otp)
    {
        _otpStorage.TryGetValue(key, out OTPDto? otpDto);
        return otpDto;
    }

    public void InvalidateOTP(string key)
    {
        _otpStorage.TryRemove(key, out _);
    }
}