using Application.Services.UserServices.ActivateUser;
using Domain;
using Domain.Results;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTPProjectTests
{
    internal class ActivateUserTests : BaseTest
    {
        [Test]
        public async Task Can_ActivateUser_Succeess()
        {
            //Arrange
            var request = new ActivateUserCommand { Id = Guid.NewGuid() }; 
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new ApplicationUser { IsRevoked = true });
            repositoryMock.Setup(x => x.UpdateUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            var handler = new ActivateUserHandler(repositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.True(result.IsSucceess);
            Assert.AreEqual("Пользователь успешно активирован", result.Message);
        }

        [Test]
        public async Task Can_ActivateUser_UserIsAlreadyActivate()
        {
            //Arrange
            var request = new ActivateUserCommand { Id = Guid.NewGuid() };
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new ApplicationUser { IsRevoked = false });
            var handler = new ActivateUserHandler(repositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual("Пользователь уже активирован", result.Message);
        }

        [Test]
        public async Task Can_ActivateUser_UserNotFound()
        {
            //Arrange
            var request = new ActivateUserCommand { Id = Guid.NewGuid() };
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync((ApplicationUser?)null);
            var handler = new ActivateUserHandler(repositoryMock.Object);
            
            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual("Пользователь не найден", result.Message);
        }
    }
}
