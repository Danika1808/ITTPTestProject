using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class JsonPatchAllowAttribute : Attribute
    {
    }
}
