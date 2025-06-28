using System.ComponentModel.DataAnnotations;

namespace RestaurationAPI.Entities
{
    public class Address
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maksymalna dozwolna długość miasta wynosi 50 znaków.")]
        public string City { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "Maksymalna dozwolna długość ulicy wynosi 50 znaków.")]

        public string Street { get; set; }
        public string PostalCode { get; set; }

        public virtual Restaurant Restaurant { get; set; } // virtual oznacza, że relacja będzie to tabeli Restaurant
    }
}
