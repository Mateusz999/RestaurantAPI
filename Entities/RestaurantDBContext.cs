using Microsoft.EntityFrameworkCore;

namespace RestaurationAPI.Entities
{
    public class RestaurantDBContext : DbContext
    {
        public RestaurantDBContext(DbContextOptions<RestaurantDBContext> options)
               : base(options)
        {
        }

        private string  _connectionString = "Server=localhost\\SQLEXPRESS;Database=RestaurantDb;Trusted_Connection=True;TrustServerCertificate=True;";
        // --- PONIŻEJ DEKLARUJEMY DBSET
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Dish> Dishes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users{ get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<Role>()
               .Property(u => u.Name)
               .IsRequired();

            modelBuilder.Entity<Restaurant>()
                .Property(r => r.Name) // konfigurujemy właściwość Name
                .IsRequired() // ustawiamy, że jest wymagana
                .HasMaxLength(25); // ustawiamy maksymalną długość na 25 znaków


            modelBuilder.Entity<Dish>()
                .Property(d => d.Name)
                .IsRequired() // ustawiamy, że jest wymagana
                .HasMaxLength(25); // ustawiamy maksymalną długość na 25 znaków

             modelBuilder.Entity<Restaurant>() // OPERACJA NA ENCJI RESTAURANT
               .HasOne(r => r.Address)         // relacja jeden do jednego z encją Address - jeden adres
                .WithOne(a => a.Restaurant)    // adress ma jedną restauracje
                .HasForeignKey<Restaurant>(r => r.AdressId); // klucz obcy znajduje sie w tabeli restaurand - adressId

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
    }
}
