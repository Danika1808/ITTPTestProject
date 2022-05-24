using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Results
{
    public enum Error : int
    {
        /// <summary>
        /// Http status code equal 200
        /// </summary>
        Success = 200,
        /// <summary>
        /// if Http status code equal 404
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// Http status code equal 409
        /// </summary>
        Conflict = 409,
        /// <summary>
        /// Http status code equal 500
        /// </summary>
        InternalServerError = 500,
        /// <summary>
        /// Http status code equal 401
        /// </summary>
        Unauthorized = 401,
        /// <summary>
        /// Http status code equal 400
        /// </summary>
        BadRequest = 400,
        /// <summary>
        /// Http status code equal 403
        /// </summary>
        Forbidden = 403
    }
}
