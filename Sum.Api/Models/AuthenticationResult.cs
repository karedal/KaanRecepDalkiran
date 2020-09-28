using System.Collections.Generic;

namespace Sum.Api.Models
{
    public class AuthenticationResult
    {
        public string Token { get; set; }
        public bool Success { get; set; }
        public string ReadableMessage { get; set; } 
        public IEnumerable<string> Errors { get; set; }
    }
}