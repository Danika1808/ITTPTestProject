using Domain.Revoke;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Extensions
{
    public static class RevokeQueryExtension
    {
        private static readonly MethodInfo _methodInfo = typeof(RevokeQueryExtension)
        .GetMethod(nameof(GetSoftDeleteFilter),
            BindingFlags.NonPublic | BindingFlags.Static);
        public static void AddRevokeQueryFilter(
        this IMutableEntityType entityData)
        {
            var methodToCall = _methodInfo.MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall.Invoke(null, Array.Empty<object>());
            entityData.SetQueryFilter((LambdaExpression)filter);
            entityData.AddIndex(entityData.
                 FindProperty(nameof(IRevoke.IsRevoked)));
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>()
            where TEntity : class, IRevoke
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsRevoked;
            return filter;
        }
    }
}
