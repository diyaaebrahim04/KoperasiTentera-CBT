using System;
using KoperasiTentera.Domain.Entities;

namespace KoperasiTentera.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task<User?> GetUserByMobileAsync(string mobile);
    Task<User?> GetUserByICNumberAsync(string icNumber);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
}

