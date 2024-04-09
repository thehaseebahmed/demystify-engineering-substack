using Bogus;
using FilteringPagination.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Pagination.Benchmarks;

internal static class DataHelpers
{
    internal const int TotalRecords = 1000000;
    internal static readonly DateTime StartingDate = DateTime.Parse("2024-01-01");

    internal static AppDbContext GetDbContext()
    {
        const string connectionString = "Server=(localdb)\\mssqllocaldb;Database=PaginationMethods;Trusted_Connection=True;MultipleActiveResultSets=true";
        var contextOptionsBuilder = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString);

        return new AppDbContext(contextOptionsBuilder.Options);
    }

    internal static void SeedDataIfEmpty()
    {
        var context = GetDbContext();

        Console.WriteLine("Creating [dbo].[Posts] table if it does not exist...");
        context.Database.ExecuteSqlRaw(@"
            IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Posts]') AND type in (N'U'))
            BEGIN
                CREATE TABLE [dbo].[Posts] (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    Content NVARCHAR(256) NOT NULL,
                    CreatedOn DATETIME NOT NULL
                );

                CREATE NONCLUSTERED INDEX IDX_Posts_CreatedOn ON [dbo].[Posts] (CreatedOn);
            END
            ");

        var hasRecords = context.Posts.Any();
        if (hasRecords)
        {
            Console.WriteLine("Skipping data creation because table is not empty");
            return;
        }

        Console.WriteLine("Creating test data...");
        var faker = new Faker("en");
        var lastGeneratedDate = StartingDate;
        for (int i = 0; i < TotalRecords; i++)
        {
            lastGeneratedDate = lastGeneratedDate.AddMinutes(1);

            context.Posts.Add(
                new()
                {
                    Content = faker.Lorem.Sentence(
                        faker.Random.Number(5, 20)
                        ),
                    CreatedOn = lastGeneratedDate
                });

            Console.WriteLine($"Created record #{i}.");
        }

        Console.Clear();
        Console.WriteLine($"Writing {TotalRecords} posts to database...");
        context.SaveChanges();

        Console.WriteLine("Data successfully created!");
    }
}
