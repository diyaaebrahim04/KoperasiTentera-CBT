﻿using System;
using KoperasiTentera.Application.Common;
using KoperasiTentera.Application.DTOs;
using KoperasiTentera.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace KoperasiTentera.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
    {
        try
        {
            GenericResponse<UserDto> response = await _userService.RegisterUserAsync(request);
            return Ok(response);
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(new { ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
    [HttpPost("login-initiation")]
    public async Task<IActionResult> LoginInitiation([FromBody] LoginInitiationRequestDto request)
    {
        try
        {
            GenericResponse<UserDto> response = await _userService.LoginInitiationAsync(request);
            return Ok(response);
        }
        catch (UserDoesNotExistException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost("login-completion")]
    public async Task<IActionResult> LoginCompletion([FromBody] LoginCompletionRequestDto request)
    {
        try
        {
            GenericResponse<UserDto> response = await _userService.LoginCompletionAsync(request);
            //TOKEN GENERATION CAN BE DONE HERE
            return Ok(response);
        }
        catch (UserDoesNotExistException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (EmailOrMobileIsNotVerifiedException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (InvalidPINException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequestDto request)
    {
        try
        {
            GenericResponse<bool> response = await _userService.VerifyOtpAsync(request);
            return Ok(new { Message = response.ResponseMessage });
        }
        catch (InvalidOtpException ex)
        {
            return BadRequest(new { ex.Message });
        }
        catch (UserDoesNotExistException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }

    [HttpPost("create-pin")]
    public async Task<IActionResult> CreatePin([FromBody] CreatePinRequestDto request)
    {
        try
        {
            await _userService.CreatePinAsync(request);
            return Ok(new { Message = "PIN created successfully." });
        }
        catch (EmailOrMobileIsNotVerifiedException ex)
        {
            return BadRequest(new { ex.Message });
        }
    }
}