using Domain;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilder<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            var options = ruleBuilder
                .NotEmpty()
                .Matches(Constants.LatinAndNumberLettersRegex).WithMessage("Запрещены все символы кроме латинских букв и цифр")
                .MinimumLength(7).WithMessage("Пароль должен состоять из 7 символов")
                .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы 1 символ в верхнем регистре")
                .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы 1 цифру");

            return options;
        }
    }
}
