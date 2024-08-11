using System;
using KoperasiTentera.Application.Interfaces;
using System.Collections.Concurrent;

namespace KoperasiTentera.Application.ApplicationServices;

public class OTPStore : IOTPStore
{
    private readonly ConcurrentDictionary<string, string> _otpStorage = new();

    public void StoreOTP(string key, string otp)
    {
        _otpStorage[key] = otp;
    }

    public string RetrieveOTP(string key)
    {
        _otpStorage.TryGetValue(key, out var otp);
        return otp;
    }

    public void InvalidateOTP(string key)
    {
        _otpStorage.TryRemove(key, out _);
    }
}