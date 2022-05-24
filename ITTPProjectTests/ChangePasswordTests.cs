using Application.Services.UserServices.ChangePassword;
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
    internal class ChangePasswordTests : BaseTest
    {
        [Test]
        public async Task Can_ChangePassword_Success()
        {
            //Arrange
            var message = "Пароль успешно изменен";
            repositoryMock.Setup(x => x.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            repositoryMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            repositoryMock.Setup(x => x.ValidateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            repositoryMock.Setup(x => x.UpdateUserAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);
            passwordHasherMock.Setup(x => x.HashPassword(It.IsAny<ApplicationUser>(), It.IsAny<string>())).Returns("MegaPasswordHash");

            var request = new ChangePasswordCommand();

            var handler = new ChangePasswordHandler(repositoryMock.Object, passwordHasherMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.True(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public async Task Can_ChangePassword_UserIsNotFound()
        {
            //Arrange
            var message = "Пользователь не найден";
            repositoryMock.Setup(x => x.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync((ApplicationUser?)null);

            var request = new ChangePasswordCommand();

            var handler = new ChangePasswordHandler(repositoryMock.Object, passwordHasherMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public async Task Can_ChangePassword_IncorrectPassword()
        {
            //Arrange
            var message = "Неправильный пароль";
            repositoryMock.Setup(x => x.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            repositoryMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(false);
            
            var request = new ChangePasswordCommand();

            var handler = new ChangePasswordHandler(repositoryMock.Object, passwordHasherMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public async Task Can_ChangePassword_ValidateError()
        {
            //Arrange
            var message = "Ошибка валидации пароля";
            repositoryMock.Setup(x => x.GetUserByLoginAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser());
            repositoryMock.Setup(x => x.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(true);
            repositoryMock.Setup(x => x.ValidateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            var request = new ChangePasswordCommand();

            var handler = new ChangePasswordHandler(repositoryMock.Object, passwordHasherMock.Object);

            //Act
            var result = await handler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(message, result.Message);
        }

        [Test]
        public void Can_ChangePassword_ValidateErrors_PasswordsIsNotEqual()
        {
            //Arrange
            var message = "Пароли не совпадают";
            var request = new ChangePasswordCommand { NewPassword = "Password", ConfirmNewPassword = "password" };

            var validator = new ChangePasswordValidator();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any(x => x.ErrorMessage == message));
        }
    }
}
