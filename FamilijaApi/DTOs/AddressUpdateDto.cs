namespace FamilijaApi.DTOs
{
    public class AddressUpdateDto
    {
        public int UserId { get; set; }
        public string Place { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public int PostalCode { get; set; }
    }
}
