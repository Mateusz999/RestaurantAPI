﻿namespace RestaurationAPI.Models
{
    public class RestaurantDto
    {

        public int Id { get; set; } // klucz głowny baza danych
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }  
        public List<DishDto> Dishes { get; set; }


    }
}
