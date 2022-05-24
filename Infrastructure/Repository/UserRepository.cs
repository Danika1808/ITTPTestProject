using AutoMapper;
using AutoMapper.QueryableExtensions;
using Domain;
using Domain.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IPasswordValidator<ApplicationUser> _passwordValidator;


        public UserRepository() { }

        public UserRepository(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IPasswordValidator<ApplicationUser> passwordValidator)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _passwordValidator = passwordValidator;
        }

        public async Task<ApplicationUser> GetUserByLoginAsync(string login)
        {
            return await _userManager.FindByNameAsync(login);
        }

        public Task<SignInResult> PasswordSignInAsync(ApplicationUser user, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return _signInManager.PasswordSignInAsync(user, password, isPersistent, lockoutOnFailure);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<ApplicationUser?> GetUserAsync(Guid id, string login = null, bool isRevokedIgnore = false)
        {
            if ((!string.IsNullOrEmpty(login) || id != default) && isRevokedIgnore)
            {
                return await _context.Users.IgnoreQueryFilters().FirstOrDefaultAsync(x => x.Login == login || x.Id == id);
            }

            if (!string.IsNullOrEmpty(login) || id != default)
            {
                return await _context.Users.FirstOrDefaultAsync(x => x.Login == login || x.Id == id);
            }

            return null;
        }

        public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
        {
            return await _userManager.CheckPasswordAsync(user, password);
        }

        public async Task<IdentityResult> ValidateAsync(ApplicationUser user, string newPassword)
        {
            return await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<int> DeleteUserAsync(ApplicationUser user)
        {
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> AnyAsync(string login)
        {
            return await _context.Users.AnyAsync(x => x.Login.Equals(login));
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}
