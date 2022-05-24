using Application.Services.UserServices.CreateUser;
using Domain;
using Domain.Dto;
using Infrastructure;
using Microsoft.AspNetCore.Identity;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITTPProjectTests
{
    internal class CreateUserTests : BaseTest
    {
        [Test]
        public async Task Can_CreateUser_Succeess()
        {
            //Arrange
            var login = "login";
            var password = "Password";
            var request = new CreateUserCommand { Login = login, Password = password, IsAdmin = true };
            var createUserHandler = new CreateUserHandler(repositoryMock.Object, mapperMock.Object);

            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync((ApplicationUser?)null);
            repositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            repositoryMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            mapperMock.Setup(x => x.Map<ApplicationUser>(It.IsAny<CreateUserCommand>())).Returns(new ApplicationUser());
            mapperMock.Setup(x => x.Map<UserDto>(It.IsAny<ApplicationUser>())).Returns(new UserDto() { Login = login });

            //Act
            var result = await createUserHandler.Handle(request, default);

            //Assert
            Assert.True(result.IsSucceess);
            Assert.AreEqual(login, result.Value.Login);
        }
        [Test]
        public async Task Can_CreateUser_UserIsAlreadyExist()
        {
            //Arrange
            var login = "login";
            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync(new ApplicationUser { Login = login });
            var request = new CreateUserCommand { Login = login };
            var createUserHandler = new CreateUserHandler(repositoryMock.Object, mapperMock.Object);

            //Act
            var result = await createUserHandler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(result.Message, "Пользователь с таким логином уже существует");
        }

        [Test]
        public async Task Can_CreateUser_IncorrectBirthday()
        {
            //Arrange
            var login = "login";
            var request = new CreateUserCommand { Login = login, Day = 1 };
            var createUserHandler = new CreateUserHandler(repositoryMock.Object, mapperMock.Object);

            //Act
            var result = await createUserHandler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(result.Message, "Заполните дату рождения полностью");
        }

        [Test]
        public async Task Can_CreateUser_CannotCreateUser()
        {
            //Arrange
            var login = "login";
            var password = "Password";
            var request = new CreateUserCommand { Login = login, Password = password };
            var createUserHandler = new CreateUserHandler(repositoryMock.Object, mapperMock.Object);

            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync((ApplicationUser?)null);
            repositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError()));
            mapperMock.Setup(x => x.Map<ApplicationUser>(It.IsAny<CreateUserCommand>())).Returns(new ApplicationUser());
            //Act
            var result = await createUserHandler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(result.Message, "Ошибка создания пользователя");
        }

        [Test]
        public async Task Can_CreateUser_CannotAddRole()
        {
            //Arrange
            var login = "login";
            var password = "Password";
            var request = new CreateUserCommand { Login = login, Password = password, IsAdmin = true };
            var createUserHandler = new CreateUserHandler(repositoryMock.Object, mapperMock.Object);

            repositoryMock.Setup(x => x.GetUserAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<bool>())).ReturnsAsync((ApplicationUser?)null);
            repositoryMock.Setup(x => x.CreateUserAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            repositoryMock.Setup(x => x.AddToRoleAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed(new IdentityError()));
            mapperMock.Setup(x => x.Map<ApplicationUser>(It.IsAny<CreateUserCommand>())).Returns(new ApplicationUser());

            //Act
            var result = await createUserHandler.Handle(request, default);

            //Assert
            Assert.False(result.IsSucceess);
            Assert.AreEqual(result.Message, "Ошибка добавление роли");
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_EmptyModel()
        {
            //Arrange
            var request = new CreateUserCommand();

            var validator = new CreateUserValidator();
            
            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_IncorrectLogin()
        {
            //Arrange
            var message = "Запрещены все символы кроме латинских букв и цифр";

            var requests = new List<CreateUserCommand>() { new CreateUserCommand { Login = "ЕльПуджеро" }, new CreateUserCommand { Login = "_" }, new CreateUserCommand { Login = "555_" }, new CreateUserCommand { Login = "CorrectНоНет" } };

            var validator = new CreateUserValidator();

   
            foreach (var request in requests)
            {
                //Act
                var result = validator.Validate(request);

                //Asseert
                Assert.False(result.IsValid);
                Assert.True(result.Errors.Any(x => x.ErrorMessage == message));
            }
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_IncorrectPassword_LengthError()
        {
            //Arrange
            var message = "Пароль должен состоять из 7 символов";

            var request = new CreateUserCommand { Password = "123456" };

            var validator = new CreateUserValidator();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any(x => x.ErrorMessage == message));
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_IncorrectPassword_requireDigitError()
        {
            //Arrange
            var message = "Пароль должен содержать хотя бы 1 цифру";

            var request = new CreateUserCommand { Password = "Password" };

            var validator = new CreateUserValidator();

            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any(x => x.ErrorMessage == message));
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_IncorrectPassword()
        {
            //Arrange
            var message = "Запрещены все символы кроме латинских букв и цифр";

            var requests = new List<CreateUserCommand>() { new CreateUserCommand { Login = "ЕльПуджеро" }, new CreateUserCommand { Login = "_______" }, new CreateUserCommand { Login = "Pa3456_" }, new CreateUserCommand { Login = "Correc1НоНет" } };

            var validator = new CreateUserValidator();

            foreach (var request in requests)
            {
                //Act
                var result = validator.Validate(request);

                //Assert
                Assert.False(result.IsValid);
                Assert.True(result.Errors.Any(x => x.ErrorMessage == message));
            }
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_IncorrectPassword_RequireUppercaseError()
        {
            //Arrange
            var message = "Пароль должен содержать хотя бы 1 символ в верхнем регистре";

            var request = new CreateUserCommand { Password = "1assword" };

            var validator = new CreateUserValidator();
            
            //Act
            var result = validator.Validate(request);

            //Assert
            Assert.False(result.IsValid);
            Assert.True(result.Errors.Any(x => x.ErrorMessage == message));
        }

        [Test]
        public void Can_CreateUser_ValidateErrors_IncorrectBirthday()
        {
            //Arrange
            var requests = new List<CreateUserCommand>() { new CreateUserCommand { Day = -1, Month = -1, Year = -1 }, new CreateUserCommand { Day = 32, Month = 13, Year = 10000 } };

            var validator = new CreateUserValidator();

            foreach (var request in requests)
            {
                //Act
                var result = validator.Validate(request);

                //Assert
                Assert.False(result.IsValid);
            }
        }
    }
}
