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

            // Assert
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

        [Fact]
        public async Task Create_Action_InvalidModelState_ReturnsView()
        {
            _sut.ModelState.AddModelError("Bio", "Biography is required");

            var actor = new Actor { Id=10, FullName= "Timothée Chalamet", ProfilePictureURL=""};

            var result = await _sut.Create(actor);

            var viewResult = Assert.IsType<ViewResult>(result);
            var testActor = Assert.IsType<Actor>(viewResult.Model);

            // Assert
            Assert.Equal(actor.FullName, testActor.FullName);
            Assert.Equal(actor.ProfilePictureURL, testActor.ProfilePictureURL);
        }

        [Fact]
        public async Task Create_Action_InvalidModelState_CreateNeverExecutes()
        {
            _sut.ModelState.AddModelError("Bio", "Biography is required");

            var actor = new Actor { Id = 10, FullName = "Timothée Chalamet", ProfilePictureURL = "" };

            await _sut.Create(actor);

            _mockService.Verify(x => x.AddAsync(It.IsAny<Actor>()), Times.Never);
        }

        [Fact]
        public async Task Create_Action_ModelStateValid_CreateActorCalledOnce()
        {
            Actor? act = null;
            _mockService.Setup(r => r.AddAsync(It.IsAny<Actor>()))
                .Callback<Actor>(x => act = x);

            var actor = new Actor
            {
                FullName = "Timothée Chalamet",
                ProfilePictureURL = "https://dotnethow.net/images/actors/actor-2.jpeg",
                Bio = "Vibrant Young Actor"
            };

            await _sut.Create(actor);

            _mockService.Verify(x => x.AddAsync(It.IsAny<Actor>()), Times.Once);

            Assert.Equal(act.FullName, actor.FullName);
            Assert.Equal(act.ProfilePictureURL, actor.ProfilePictureURL);
            Assert.Equal(act.Bio, actor.Bio);
        }


    }
}
