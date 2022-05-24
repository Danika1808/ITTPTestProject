using Application.Extensions;
using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Services.UserServices.CreateUser
{
    public class CreateUserValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Login).NotEmpty().Matches(Constants.LatinAndNumberLettersRegex).WithMessage("Запрещены все символы кроме латинских букв и цифр");
            RuleFor(x => x.Password).Password();
            RuleFor(x => x.Name).NotEmpty().Matches(Constants.LatinAndRussianLettersRegex).WithMessage("Запрещены все символы кроме латинских и русских букв");
            RuleFor(x => x.Gender).Must(x => Enum.IsDefined(typeof(Gender), x)).WithMessage("Пол указан неверно");
            RuleFor(x => x.Day).GreaterThanOrEqualTo(0).LessThan(32).WithMessage("День указан неверно");
            RuleFor(x => x.Month).GreaterThanOrEqualTo(0).LessThan(13).WithMessage("Месяц указан неверно");
            RuleFor(x => x.Year).GreaterThanOrEqualTo(0).LessThan(10000).WithMessage("Год указан неверно");
        }
    }
}