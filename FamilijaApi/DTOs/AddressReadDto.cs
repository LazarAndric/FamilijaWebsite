using System.ComponentModel.DataAnnotations;

namespace FamilijaApi.DTOs
{ 

    public class AddressReadDto
    {
        public string Place { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
    }
}
