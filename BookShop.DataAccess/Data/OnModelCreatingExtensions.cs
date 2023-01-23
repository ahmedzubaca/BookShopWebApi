using BookShop.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace BookShop.DataAccess.Data
{
    public static class OnModelCreatingExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {            
            modelBuilder.Entity<Category>()
                .HasData(
                new Category
                {
                    Id = 1,
                    Name = "Drama",
                    DisplayOrder = 1,
                    CreatedDateTime = new DateTime(2022, 12, 23, 14, 43, 0)
                },
                new Category
                {
                    Id = 2,
                    Name = "Triller",
                    DisplayOrder = 3,
                    CreatedDateTime = new DateTime(2022, 12, 23, 14, 44, 10)
                },
                new Category
                {
                    Id = 3,
                    Name = "Comedy",
                    DisplayOrder = 2,
                    CreatedDateTime = new DateTime(2022, 12, 23, 14, 45, 20)
                });

            modelBuilder.Entity<CoverType>()
                .HasData(
                new CoverType
                {
                    Id = 1,
                    Name = "Softcover",
                },
                new CoverType
                {
                    Id = 2,
                    Name = "Hardcover" 
                }
                );
        }
    }
}
