using System.Collections.Generic;

namespace FamilijaApi.Configuration
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public List<string> Errors { get; set; }
    }
}