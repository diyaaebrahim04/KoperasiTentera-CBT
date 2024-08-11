using System;
namespace KoperasiTentera.Application.Common;

// Base exception for application-specific errors
public class ApplicationException : Exception
{
    public ApplicationException(string message) : base(message) { }
    public ApplicationException(string message, Exception innerException) : base(message, innerException) { }
}

// Exception for user already exists
public class UserAlreadyExistsException : ApplicationException
{
    public UserAlreadyExistsException() : base("User already exists.") { }
}

// Exception for user not found
public class UserDoesNotExistException : ApplicationException
{
    public UserDoesNotExistException() : base("User does not exist.") { }
}

// Exception for Email address or/and phone number is/are not verified
public class EmailOrMobileIsNotVerifiedException : ApplicationException
{
    public EmailOrMobileIsNotVerifiedException() : base("Email address or/and phone number is/are not verified.") { }
}

// Exception for invalid OTP
public class InvalidOtpException : ApplicationException
{
    public InvalidOtpException() : base("Invalid OTP.") { }
}

// Exception for failed validation
public class ValidationException : ApplicationException
{
    public ValidationException(string message) : base(message) { }
}

// Exception for unauthorized access
public class UnauthorizedAccessException : ApplicationException
{
    public UnauthorizedAccessException() : base("Unauthorized access.") { }
}

// Exception for not found resource
public class NotFoundException : ApplicationException
{
    public NotFoundException(string message) : base(message) { }
}