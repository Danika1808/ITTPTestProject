using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Revoke
{
    public interface IRevoke
    {
        bool IsRevoked { get; set; }
    }
}
