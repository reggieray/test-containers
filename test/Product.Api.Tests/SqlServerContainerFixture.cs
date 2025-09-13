using Microsoft.EntityFrameworkCore;
using Product.Api.Data;
using System.ComponentModel;
using Testcontainers.MsSql;

namespace Product.Api.Tests
{
    public class SqlServerContainerFixture : IAsyncLifetime
    {
        public MsSqlContainer Container { get; private set; } = default!;
        public string ConnectionString { get; private set; } = string.Empty;

        public async Task InitializeAsync()
        {
            Container = new MsSqlBuilder()
            .WithPassword("YourStrong!Passw0rd")
            .Build();

            await Container.StartAsync();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

            using var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            await Container.StopAsync();
            await Container.DisposeAsync();
        }
    }
}
