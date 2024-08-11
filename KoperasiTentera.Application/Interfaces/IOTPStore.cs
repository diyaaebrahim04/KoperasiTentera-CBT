using KoperasiTentera.Application.DTOs;

namespace KoperasiTentera.Application.Interfaces;

public interface IOTPStore
{
    void StoreOTP(string key, string otp);
    OTPDto? RetrieveOTP(string key, string otp);
    void InvalidateOTP(string key);
}