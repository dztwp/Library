using Epam.Library.Entities;
using FluentValidation;
using System;

namespace Epam.Library.BLL.Validation
{
    public static class MyCustomValidators
    {
        public static IRuleBuilderOptions<T, string> LengthCantBeGreaterThan<T>(this IRuleBuilder<T, string> ruleBuilder, int num) where T : class
        {
            return ruleBuilder
                .Length(1, num)
                .WithMessage("Length of {PropertyName} can't be greater than " + $"{num}");
        }

        public static IRuleBuilderOptions<T, string> CityNameMustMatchPattern<T>(this IRuleBuilder<T, string> ruleBuilder, string cityPattern) where T : AbstractPaper
        {
            return ruleBuilder
                .Matches(cityPattern)
                .WithMessage("{PropertyName} must contains Russian or Latin characters starting with a capital letter. May contain a hyphen, in this case the first character after the hyphen is capitalized. It can contain a double hyphen, in which case the character becomes capitalized only after the second hyphen. The hyphen cannot be the first or last character. May contain spaces. The space can be followed by either a lowercase or an uppercase letter.");
        }

        public static IRuleBuilderOptions<T, string> FirstNameMatchesPattern<T>(this IRuleBuilder<T, string> ruleBuilder, string firstNamePattern) where T : Person
        {
            return ruleBuilder
                .Matches(firstNamePattern)
                .WithMessage("Incorrect input of {PropertyName}.Name must contain either Cyrillic or Latin. If the last name is written with a hyphen, the character after the hyphen is capitalized.");
        }

        public static IRuleBuilderOptions<T, string> LastNameMatchesPattern<T>(this IRuleBuilder<T, string> ruleBuilder, string lastNamePattern) where T : Person
        {
            return ruleBuilder
                .Matches(lastNamePattern)
                .WithMessage("Incorrect input of {PropertyName}.Name must contain either Cyrillic or Latin. After the prefix, the surname is capitalized.If the surname is with a hyphen, a capital letter is placed after the hyphen.It can contain an apostrophe and a capital letter is written after the apostrophe.");
        }

        public static IRuleBuilderOptions<T, T1> RequiredField<T, T1>(this IRuleBuilder<T, T1> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .WithMessage("{PropertyName} must be filled");
        }

        public static IRuleBuilderOptions<T, int> DateMustBeCorrectForBooksNewspapers<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x >= 1400 && x <= DateTime.Now.Year)
                .WithMessage("{PropertyName} must be between 1400 and this year");
        }

        public static IRuleBuilderOptions<T, int> DateMustBeCorrectForPatents<T>(this IRuleBuilder<T, int> ruleBuilder)
        {
            return ruleBuilder
                .Must(x => x >= 1474 && x <= DateTime.Now.Year)
                .WithMessage("{PropertyName} must be between 1474 and this year");
        }

    }
}
