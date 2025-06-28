namespace RestaurationAPI.Entities
{
    public class Dish
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }


        public int RestaurantId { get; set; } // na tej podstawie Entity Framework utworzy referencję/relacje do tabeli
        public virtual Restaurant Restaurant { get; set; } // virtual oznacza, że relacja będzie to tabeli Restaurant
    }
}
