using Application.Services.UserServices.CreateUser;
using AutoMapper;
using Domain;
using Infrastructure;
using Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ITTPProjectTests
{
    public class BaseTest
    {
        internal Mock<IUserRepository> repositoryMock;
        internal Mock<IMapper> mapperMock;
        internal Mock<IPasswordHasher<ApplicationUser>> passwordHasherMock;

        [SetUp]
        public void Setup()
        {
            mapperMock = new Mock<IMapper>();
            repositoryMock = new Mock<IUserRepository>();
            passwordHasherMock = new Mock<IPasswordHasher<ApplicationUser>>();
        }
    }
}