using System.ComponentModel.DataAnnotations;

namespace RestaurationAPI.Models
{
    public class CreateRestaurantDto
    {

        [Required]
        [MaxLength(25)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        [EmailAddress]
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maksymalna dozwolna długość miasta wynosi 50 znaków.")]

        public string City { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maksymalna dozwolna długość ulicy wynosi 50 znaków.")]
        public string Street { get; set; }
        [RegularExpression(@"^\d{2}-\d{3}$", ErrorMessage = "Kod pocztowy musi mieć format 12-345.")]
        public string PostalCode { get; set; }

    }
}
