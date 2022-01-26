using Microsoft.AspNetCore.Mvc;
using Moq;
using MovieConnect.Website.Controllers;
using MovieConnect.Website.Models;
using MovieConnect.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MovieConnect.Tests
{
    public class ActorsControllerTests
    {
        private readonly Mock<IActorsService> _mockService;
        private readonly ActorsController _sut;

        public ActorsControllerTests()
        {
            _mockService = new Mock<IActorsService>();
            _sut = new ActorsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_Action_Executes_ReturnsViewForIndex()
        {
            // Act
            var result = await _sut.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Index_Action_Executes_ReturnsExactNumberOfActors()
        {
            _mockService.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(new List<Actor>() { new Actor(), new Actor() });

            var result = await _sut.Index();

            var viewResult = Assert.IsType<ViewResult>(result);
            var actors = Assert.IsType<List<Actor>>(viewResult.Model);
            Assert.Equal(2, actors.Count);
        }

        [Fact]
        public void Create_Action_Executes_ReturnsViewForIndex()
        {
            // Act
            var result = _sut.Create();

            // Assert
            Assert.IsType<ViewResult>(result);
        }


    }
}
