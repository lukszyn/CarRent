using AutoMapper;
using CarRent.Data;
using CarRent.DTOs;
using CarRent.ViewModels;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentTests
{
    public class CarsControllerTests
    {
        [Test]
        public void Index_ReturnsAViewResult_WithAListOfCars()
        {
            // Arrange
            var mockRepo = new Mock<ICarRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockWebHost = new Mock<IWebHostEnvironment>();
            var controller = new CarsController(mockRepo.Object, mockMapper.Object, mockWebHost.Object);

            mockRepo.Setup(repo => repo.GetCars()).Returns(new List<CarDto>
            {
                new CarDto(),
                new CarDto()
            });

            // Act
            var result = (ViewResult)controller.Index(true);
            var viewResult = (ViewResult)controller.Index(true);
            var model = (CarsViewModel)result.ViewData.Model;

            //Assert
            result.Should().BeOfType<ViewResult>();
            model.Cars.Should().NotBeNull().And.HaveCount(2);
        }

        [Test]
        public async Task New_ReturnsBadRequest()
        {
            // Arrange
            var mockRepo = new Mock<ICarRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockWebHost = new Mock<IWebHostEnvironment>();
            var controller = new CarsController(mockRepo.Object, mockMapper.Object, mockWebHost.Object);
            var carDto = new CarDto();
            IFormCollection formCollection = new FormCollection(new Dictionary<string, StringValues>());
            controller.ModelState.AddModelError("Brand", "Required");

            // Act
            var result = await controller.Create(carDto, formCollection);
            
            //Assert
            result.Should().BeOfType<BadRequestResult>();
        }

        [TestCase(1)]
        [TestCase(10)]
        [TestCase(100)]
        public void EditUser_ReturnsProperViewModel(int id)
        {
            // Arrange
            var mockRepo = new Mock<ICarRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockWebHost = new Mock<IWebHostEnvironment>();
            var controller = new CarsController(mockRepo.Object, mockMapper.Object, mockWebHost.Object);

            mockRepo.Setup(repo => repo.GetCarByID(It.IsAny<int>()))
                .Returns(new CarDto() { 
                    Id = id
                });

            // Act
            var result = (ViewResult)controller.Edit(id);
            var model = (CarDto)result.ViewData.Model;

            //Assert
            result.Should().BeOfType<ViewResult>();
            model.Should().NotBeNull();
        }


        [TestCase(1)]
        [TestCase(-1)]
        [TestCase(0)]
        public void DeleteUser_ReturnsNotFound(int id)
        {
            // Arrange
            var mockRepo = new Mock<ICarRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockWebHost = new Mock<IWebHostEnvironment>();
            var controller = new CarsController(mockRepo.Object, mockMapper.Object, mockWebHost.Object);

            mockRepo.Setup(repo => repo.GetCarByID(It.IsAny<int>()))
                .Returns((CarDto)null);

            // Act
            var result = controller.Delete(id);

            //Assert
            result.Should().BeOfType<NotFoundResult>();
        }
    }
}