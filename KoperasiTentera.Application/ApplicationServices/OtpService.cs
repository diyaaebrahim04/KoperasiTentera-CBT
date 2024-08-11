using KoperasiTentera.Application.Enums;
using KoperasiTentera.Application.Interfaces;

namespace KoperasiTentera.Application.ApplicationServices;

public class OtpService : IOTPService
{
    private readonly IOTPStore _otpStore;

    public OtpService(IOTPStore otpStore)
    {
        _otpStore = otpStore;
    }
    public string GenerateOtp(string key)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        _otpStore.StoreOTP(key, otp);
        return otp;
    }

    public bool ValidateOtp(string key, string otp)
    {
        return _otpStore.RetrieveOTP(key, otp)?.OTPValue == otp;
    }

    public void ForgetOtp(string key)
    {
        _otpStore.InvalidateOTP(key);
    }
    public async Task SendOtpAsync(string key, string otp, OtpType otpType)
    {
        switch (otpType)
        {
            case OtpType.Mobile:
                await sendSmsAsync(key, otp);
                break;
            case OtpType.Email:
                await sendEmailAsync(key, otp);
                break;
        }
    }

    #region Helpers

    private async Task sendSmsAsync(string phoneNumber, string otp)
    {
        // Code for SMS provider - Twilio?
    }

    private async Task sendEmailAsync(string emailAddress, string otp)
    {
        // Code for Mail service - MailKit?
    }
    #endregion
}