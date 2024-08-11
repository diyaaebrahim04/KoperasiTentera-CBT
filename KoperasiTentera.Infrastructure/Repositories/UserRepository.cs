using System;
using KoperasiTentera.Application.Interfaces;
using KoperasiTentera.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace KoperasiTentera.Infrastructure.Repositories;
public class UserRepository : IUserRepository
{
    private readonly KoperasiTenteraDbContext _context;

    public UserRepository(KoperasiTenteraDbContext context)
    {
        _context = context;
    }

    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.EmailAddress == email);
    }

    public async Task<User> GetUserByMobileAsync(string mobile)
    {
        return await _context.Users.SingleOrDefaultAsync(u => u.MobileNumber == mobile);
    }

    public async Task AddUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}