using System;

namespace Prisms.Core
{
    public class Response {
        public bool IsSuccess { get; set; }
        public string Content { get; set; }
        public Exception Exception {  get; set; }

        public static Response Success(string content)
        {
            return new Response
            {
                IsSuccess = true,
                Content = content
            };
        }

        public static Response Error(Exception exception)
        {
            return new Response { 
                IsSuccess = false,
                Exception = exception
            };
        }

        public override string ToString()
        {
            return $"{IsSuccess}: {(IsSuccess ? Content : Exception)}";
        }
    }
}