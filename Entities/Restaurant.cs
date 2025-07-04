﻿namespace RestaurationAPI.Entities
{
    public class Restaurant
    {
        public int Id { get; set; } // klucz głowny baza danych
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public bool HasDelivery { get; set; }
        public string ContactEmail { get; set; }
        public string ContactNumber { get; set; }

        public int? CreatedById { get; set; }
        public virtual User CreatedBy { get; set; }
        public int AdressId { get; set; } // na tej podstawie Entity Framework utworzy referencję/relacje do tabeli - klucz obcy do tabeli z adresem
        public virtual Address Address { get; set; }// virtual oznacza, że relacja będzie to tabeli Adress

        public virtual List<Dish> Dishes { get; set; } // virtual oznacza, że relacja będzie to tabeli Dish
    }
}
