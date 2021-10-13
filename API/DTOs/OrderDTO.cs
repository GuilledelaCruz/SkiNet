using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class OrderDTO
    {
        [Required]
        public string BasketId { get; set; }
        [Required]
        public int DeliveryMethod { get; set; }
        [Required]
        public AddressDTO ShipToAddress { get; set; }
    }
}