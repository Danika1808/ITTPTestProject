using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Results
{
    public class Result
    {
        public Error Error { get; private set; }
        public string? Message { get; internal set; }
        public bool IsSucceess { get; internal set; }

        public Result(string message)
        {
            Message = message;
            IsSucceess = true;
        }

        public Result(Error error, string message)
        {
            Error = error;
            Message = message;
            IsSucceess = false;
        }

        public static Result CreateSuccess(string message = null)
        {
            return new Result(message);
        }

        public static Result CreateFailure(Error error, string message)
        {
            return new Result(error, message);
        }
    }
    public class Result<TResult> : Result
    {
        public TResult Value { get; private set; }

        private Result(TResult value, string message) : base(message)
        {
            Value = value;
            IsSucceess = true;
        }

        private Result(Error error, string message) : base(error, message) { }

        /// <summary>
        /// Create success result
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Result<TResult> CreateSuccess(TResult value, string message = null)
        {
            return new Result<TResult>(value, message);
        }


        /// <summary>
        /// Create failure result
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public static new Result<TResult> CreateFailure(Error error, string message)
        {
            return new Result<TResult>(error, message);
        }
    }
}
