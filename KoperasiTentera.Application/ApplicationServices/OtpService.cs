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
    public string GenerateOtp(string identifier)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        _otpStore.StoreOTP(identifier, otp);
        return otp;
    }

    public bool ValidateOtp(string identifier, string otp)
    {
        return _otpStore.RetrieveOTP(identifier) == otp;
    }

    public async Task SendOtpAsync(string identifier, string otp, OtpType otpType)
    {
        switch (otpType)
        {
            case OtpType.Mobile:
                await sendSmsAsync(identifier, otp);
                break;
            case OtpType.Email:
                await sendEmailAsync(identifier, otp);
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