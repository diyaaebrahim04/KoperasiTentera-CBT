namespace KoperasiTentera.Application.Interfaces;

public interface IOTPStore
{
    void StoreOTP(string key, string otp);
    string RetrieveOTP(string key);
    void InvalidateOTP(string key);
}