using System;
using System.Net;

namespace Sho.Pocket.Application.Exceptions
{
    public abstract class BasePocketException : Exception
    {
        public BasePocketException(string code, string message, HttpStatusCode statusCode) : base(message)
        {
            Code = code;
            StatusCode = statusCode;
        }

        public string Code { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
