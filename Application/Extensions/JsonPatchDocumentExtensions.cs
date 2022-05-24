using Domain.Attributes;
using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class JsonPatchDocumentExtensions
    {
        public static void Sanitize<T>(this JsonPatchDocument<T> document) where T : class
        {
            for (int i = document.Operations.Count - 1; i >= 0; i--)
            {
                string pathPropertyName = document.Operations[i].path
                    .Split("/", StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault();

                if (typeof(T)
                    .GetProperties()
                    .Where(p => !p.IsDefined(typeof(JsonPatchAllowAttribute), true) && string.Equals(p.Name, pathPropertyName, StringComparison.CurrentCultureIgnoreCase))
                    .Any())
                {
                    document.Operations.RemoveAt(i);
                }
            }
        }
    }
}
