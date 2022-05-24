using Application.Services.UserServices.DeleteUser;
using Domain;
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
    internal class DeleteUserTests : BaseTest
    {
        [Test]
        public async Task Can_DeleteUser_Success_IsSoftDelete()
        {
            //Arrange
            var message = "Пользователь успешно деактивирован";
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new ApplicationUser());
            repositoryMock.Setup(x => x.UpdateUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            var request = new DeleteUserCommand() { IsSoftDelete = true };

            var handler = new DeleteUserHandler(repositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.True(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public async Task Can_DeleteUser_Success_IsHardDelete()
        {
            //Arrange
            var message = "Пользователь успешно удален";
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new ApplicationUser());
            repositoryMock.Setup(x => x.DeleteUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(1);

            var request = new DeleteUserCommand() { IsSoftDelete = false };

            var handler = new DeleteUserHandler(repositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.True(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public async Task Can_DeleteUser_Success_UserIsNotFound()
        {
            //Arrange
            var message = "Пользователь не найден";
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync((ApplicationUser?)null);

            var request = new DeleteUserCommand();

            var handler = new DeleteUserHandler(repositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public async Task Can_DeleteUser_Success_UserIsAlreadyRevoked()
        {
            //Arrange
            var message = "Пользователь уже деактивирован";
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new ApplicationUser() { IsRevoked = true });

            var request = new DeleteUserCommand() { IsSoftDelete = true };

            var handler = new DeleteUserHandler(repositoryMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }
    }
}
