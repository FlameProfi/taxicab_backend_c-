using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using course.Controllers;
using course.Models;
using Xunit;
namespace course.course.Tests.Controllers
{
    public class CarsControllerTests
    {
        private readonly Mock<TaxiDbContext> _mockContext;
        private readonly CarsController _controller;

        public CarsControllerTests()
        {
            _mockContext = new Mock<TaxiDbContext>();
            _controller = new CarsController(_mockContext.Object);
        }

        [Fact]
        public async Task GetCars_Returns_OkResult_With_Cars_List()
        {
            var cars = new List<Car>
            {
                new Car { Id = 1, Make = "Toyota", Model = "Camry", Year = 2022, Price = 25000m, Color = "White" },
                new Car { Id = 2, Make = "Honda", Model = "Civic", Year = 2021, Price = 22000m, Color = "Blue" }
            };

            var mockSet = new Mock<DbSet<Car>>();
            mockSet.As<IQueryable<Car>>().Setup(m => m.Provider).Returns(cars.AsQueryable().Provider);
            mockSet.As<IQueryable<Car>>().Setup(m => m.Expression).Returns(cars.AsQueryable().Expression);
            mockSet.As<IQueryable<Car>>().Setup(m => m.ElementType).Returns(cars.AsQueryable().ElementType);
            mockSet.As<IQueryable<Car>>().Setup(m => m.GetEnumerator()).Returns(cars.AsQueryable().GetEnumerator());

            _mockContext.Setup(c => c.Cars).Returns(mockSet.Object);

            var result = await _controller.GetCars();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<Car>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task GetCar_Returns_NotFound_When_Car_Does_Not_Exist()
        {
            var mockSet = new Mock<DbSet<Car>>();
            mockSet.Setup(m => m.FindAsync(999)).ReturnsAsync((Car)null);

            _mockContext.Setup(c => c.Cars).Returns(mockSet.Object);

            var result = await _controller.GetCar(999);

            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            var returnValue = notFoundResult.Value as dynamic;
            Assert.Equal("Автомобиль не найден", returnValue.message.ToString());
        }

        [Fact]
        public async Task CreateCar_Returns_CreatedAtActionResult_When_Valid_Car()
        {
            var car = new Car
            {
                Make = "Ford",
                Model = "Mustang",
                Year = 2023,
                Price = 35000m,
                Color = "Red"
            };

            var mockSet = new Mock<DbSet<Car>>();
            _mockContext.Setup(m => m.Cars).Returns(mockSet.Object);

            var result = await _controller.CreateCar(car);

            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var returnValue = Assert.IsType<Car>(createdAtActionResult.Value);
            Assert.Equal("Ford", returnValue.Make);
            Assert.Equal("Mustang", returnValue.Model);
            mockSet.Verify(m => m.Add(It.IsAny<Car>()), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task UpdateCar_Returns_BadRequest_When_Id_Mismatch()
        {
            var car = new Car { Id = 1, Make = "Toyota", Model = "Camry" };

            var result = await _controller.UpdateCar(2, car);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var returnValue = badRequestResult.Value as dynamic;
            Assert.Equal("ID в URL не совпадает с ID в теле запроса", returnValue.message.ToString());
        }

        [Fact]
        public async Task DeleteCar_Returns_Ok_When_Car_Exists()
        {
            var car = new Car { Id = 1, Make = "Toyota", Model = "Camry" };
            var mockSet = new Mock<DbSet<Car>>();
            mockSet.Setup(m => m.FindAsync(1)).ReturnsAsync(car);

            _mockContext.Setup(c => c.Cars).Returns(mockSet.Object);

            var result = await _controller.DeleteCar(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = okResult.Value as dynamic;
            Assert.Equal("Автомобиль успешно удален", returnValue.message.ToString());
            mockSet.Verify(m => m.Remove(car), Times.Once());
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }
    }
}
