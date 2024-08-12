using System;
using System.Security.Cryptography;
using System.Text;

namespace KoperasiTentera.Domain.ValueObjects;

public class PIN
{
    public string PinHash { get; private set; }

    public PIN(string pin)
    {
        PinHash = HashPin(pin);
    }

    private static string HashPin(string pin)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(pin));
        return Convert.ToBase64String(bytes);
    }

    public bool ValidatePin(string pin)
    {
        return HashPin(pin) == PinHash;
    }
}
