using FamilijaApi.Configuration;
using FamilijaApi.DTOs.Requests;

namespace FamilijaApi.Models
{
    public class GetCommunicationModel<T>
    {
        public TokenRequest TokenRequest { get; set; }
        public T GenericModel { get; set; }
    }
}