using System.ComponentModel.DataAnnotations;

namespace RestaurationAPI.Models
{
    public class CreateDishDto
    {
        [Required]
        [MaxLength(25,ErrorMessage = "Maksymalna dozwolna długość nazwy dania wynosi 25 znaków.")]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


        public int RestaurantId { get; set; } 
    }
}
