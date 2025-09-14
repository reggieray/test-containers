using Microsoft.EntityFrameworkCore;
using Product.Api.Data;
using System.ComponentModel;
using Testcontainers.MsSql;

namespace Product.Api.Tests
{
    public class SqlServerContainerFixture : IAsyncLifetime
    {
        private MsSqlContainer? _container { get; set; }
        public string ConnectionString { get; private set; } = string.Empty;

        public async Task InitializeAsync()
        {
            _container = new MsSqlBuilder().Build();

            await _container.StartAsync();

            ConnectionString = _container.GetConnectionString();

            var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

            using var context = new AppDbContext(options);
            await context.Database.EnsureCreatedAsync();
        }

        public async Task DisposeAsync()
        {
            if (_container is not null)
            {
                await _container.StopAsync();
                await _container.DisposeAsync();
            }
        }
    }
}
