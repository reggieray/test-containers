using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Product.Api.Controllers;
using Product.Api.Data;

namespace Product.Api.Tests.Controllers
{
    public class ProductsControllerTests : IClassFixture<SqlServerContainerFixture>
    {
        private readonly DbContextOptions<AppDbContext> _options;
        public ProductsControllerTests(SqlServerContainerFixture fixture)
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(fixture.ConnectionString)
            .Options;
        }

        [Fact]
        public async Task PostAndGetProduct_UsingRealSqlServer_Works()
        {
            // Arrange: create a fresh DbContext for test
            await using var context = new AppDbContext(_options);
            var controller = new ProductsController(context);

            var newProduct = new Models.Product { Name = "Direct SQL Test", Price = 45.67m };

            // Act: POST
            var postResult = await controller.PostProduct(newProduct);

            // Assert created product
            var created = (postResult.Result as Microsoft.AspNetCore.Mvc.CreatedAtActionResult)?.Value as Models.Product;
            created.Should().NotBeNull();
            created!.Id.Should().BeGreaterThan(0);

            // Act: GET
            var getResult = await controller.GetProduct(created.Id);

            var fetched = getResult.Value;
            fetched.Should().NotBeNull();
            fetched!.Name.Should().Be("Direct SQL Test");
        }
    }
}
