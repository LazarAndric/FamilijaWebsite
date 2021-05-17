using System.Collections.Generic;
using FamilijaApi.Configuration;
using FamilijaApi.DTOs;

namespace FamilijaApi.Models
{
    public class CommunicationModel<T>
    {
        public AuthResult Result { get; set; }
        public AuthTokenCreate CreateToken { get; set; }
        public T GenericModel { get; set; }
    }
    public class CommunicationModel
    {
        public AuthTokenCreate CreateToken { get; set; }
        public AuthResult Result { get; set; }
    }
}