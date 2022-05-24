using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class TokenRepository : ITokenRepository
    {
        private readonly AppDbContext _context;

        public TokenRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> AddTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            return await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string id)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(x => x.JwtId.Equals(id));
        }

        public async Task<int> UpdateTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            return await _context.SaveChangesAsync();
        }
    }
}
