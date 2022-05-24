using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Results
{
    public struct ResponseResult<TResult>
    {
        public TResult Value { get; set; }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Uri Type { get; set; }
        public string TraceId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
    }

    public struct ResponseResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Uri Type { get; set; }
        public string TraceId { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
    }
}
