using System;
using FluentValidation;

namespace KoperasiTentera.Application.DTOs;

public class LoginInitiationRequestDto
{
    public required string ICNumber { get; set; }
}

public class LoginInitiationRequestDtoValidator : AbstractValidator<LoginInitiationRequestDto>
{
    public LoginInitiationRequestDtoValidator()
    {
        RuleFor(x => x.ICNumber)
            .NotEmpty().WithMessage("IC Number is required.")
            .Matches(@"^\d{10,12}$").WithMessage("IC Number must be between 10 and 12 digits.");
    }
}

public class LoginCompletionRequestDto
{
    public required string ICNumber { get; set; }
    public required string PIN { get; set; }
}

public class LoginCompletionRequestValidator : AbstractValidator<LoginCompletionRequestDto>
{
    public LoginCompletionRequestValidator()
    {
        RuleFor(x => x.ICNumber)
           .NotEmpty().WithMessage("IC Number is required.")
           .Matches(@"^\d{10,12}$").WithMessage("IC Number must be between 10 and 12 digits.");

        RuleFor(x => x.PIN)
            .Matches(@"^\d*$").WithMessage("Pin must contain only digits.")
            .Length(6).WithMessage("Pin must be exactly 6 digits long.");
    }
}