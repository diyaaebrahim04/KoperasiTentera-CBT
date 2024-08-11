using System;
using FluentValidation;

namespace KoperasiTentera.Application.DTOs;

public class CreatePinRequestDto
{
    public required string EmailAddress { get; set; }
    public required string PIN { get; set; }
}

public class CreatePinRequestValidator : AbstractValidator<CreatePinRequestDto>
{
    public CreatePinRequestValidator()
    {
        RuleFor(x => x.EmailAddress)
            .NotEmpty().WithMessage("Email Address is required.")
            .EmailAddress().WithMessage("Email Address is not valid.");

        RuleFor(x => x.PIN)
            .Matches(@"^\d*$").WithMessage("Pin must contain only digits.")
            .Length(6).WithMessage("Pin must be exactly 6 digits long.");
    }
}

