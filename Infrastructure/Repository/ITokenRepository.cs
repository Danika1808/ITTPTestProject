using Domain;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface ITokenRepository
    {
        Task<int> AddTokenAsync(RefreshToken refreshToken);
        Task<int> UpdateTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken?> GetRefreshTokenAsync(string id);
    }
}
