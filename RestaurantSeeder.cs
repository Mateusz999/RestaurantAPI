using RestaurationAPI.Entities;

namespace RestaurationAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDBContext _context;
        public  RestaurantSeeder(RestaurantDBContext context)
        {
            _context = context;
        }
        public void Seed()
        {
            if (_context.Database.CanConnect())
            {

                if (!_context.Roles.Any())
                {
                    var roles = GetRoles();
                    _context.Roles.AddRange(roles);
                    _context.SaveChanges();
                }
                if (!_context.Restaurants.Any())
                {
                    var restaurants = GetRestaurants();
                    _context.Restaurants.AddRange(restaurants);
                    _context.SaveChanges();
                }
            }
        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>
            {
                new Role()  {  Name="User"},
                new Role()  {  Name="Manager"},
                new Role()  {  Name="Admin"}
            };
            return roles;
        }

        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restauranies = new List<Restaurant>
    {
        new Restaurant
        {
            Name = "Pizzeria Italia",
            Category = "Pizza",
            Description = "Najlepsza pizza w mieście",
            ContactEmail = "italiana@gmailc.com",
            ContactNumber = "123-456-789",
            HasDelivery = true,
             Address = new Address()
            {
                City = "Warszawa",
                Street = "Rynek Główny 20",
                PostalCode = "34-042"
            },
            Dishes = new List<Dish>()
            {
                new Dish()
                {
                    Name = "Margherita",
                    Description = "Klasyczna pizza z sosem pomidorowym i serem mozzarella",
                    Price = 29.99m
                },
                new Dish()
                {
                    Name = "Pepperoni",
                    Description = "Pizza z sosem pomidorowym, serem mozzarella i pepperoni",
                    Price = 34.99m
                }
            },
        },
        new Restaurant
        {
            Name = "Sushi Zen",
            Category = "Japońska",
            Description = "Nowoczesne sushi bar z autentyczną kuchnią japońską",
            ContactEmail = "kontakt@sushizen.pl",
            ContactNumber = "789-654-321",
            HasDelivery = false,

            Address = new Address()
            {
                City = "Kraków",
                Street = "Rynek Główny 20",
                PostalCode = "31-042"
            },

            Dishes = new List<Dish>()
            {
                new Dish()
                {
                    Name = "Sake Nigiri",
                    Description = "Kawałek łososia na ryżu z wasabi",
                    Price = 18.50m
                },
                new Dish()
                {
                    Name = "California Roll",
                    Description = "Ryż z krabem, ogórkiem i awokado w nori",
                    Price = 24.00m
                },
                new Dish()
                {
                    Name = "Miso Soup",
                    Description = "Tradycyjna japońska zupa z pastą miso i tofu",
                    Price = 12.00m
                }
            }
        }
    };

            return restauranies;
        }

    }
}
