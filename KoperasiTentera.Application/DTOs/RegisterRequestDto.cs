using System;
using FluentValidation;

namespace KoperasiTentera.Application.DTOs;

public class RegisterRequestDto
{
    public required string FullName { get; set; }
    public required string ICNumber { get; set; }
    public required string MobileNumber { get; set; }
    public required string EmailAddress { get; set; }
}

public class RegisterRequestDtoValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestDtoValidator()
    {
        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Full Name is required.")
            .MaximumLength(100).WithMessage("Full Name cannot exceed 100 characters.");

        RuleFor(x => x.ICNumber)
            .NotEmpty().WithMessage("IC Number is required.")
            .Matches(@"^\d{10,12}$").WithMessage("IC Number must be between 10 and 12 digits.");

        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile Number is required.")
            .Matches(@"^\d{10}$").WithMessage("Mobile Number must be exactly 10 digits.");

        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email Address is required.")
            .EmailAddress().WithMessage("Email Address is not valid.");
    }
}
