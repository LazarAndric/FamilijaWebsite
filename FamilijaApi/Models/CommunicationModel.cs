using FamilijaApi.Configuration;

namespace FamilijaApi.Models
{
    public class CommunicationModel<T>
    {
        public AuthResult AuthResult { get; set; }
        public T GenericModel { get; set; }
    }
}