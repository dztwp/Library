using Epam.Library.Entities;
using FluentValidation;

namespace Epam.Library.BLL.Validation
{
    public class PatentValidator : AbstractValidator<Patent>
    {
        public PatentValidator()
        {
            CascadeMode = CascadeMode.Stop;

            const string patternForCountry = "^((([А-ЯЁ]+)|(([А-ЯЁ][а-яё]+)( [А-ЯЁ][а-яё]+)*))|(([A-Z]+)|(([A-Z][a-z]+)( [A-Z][a-z]+)*)))$";
            const string patternForRegistrationNumber = "^([0-9]{1,9})$";


            RuleFor(x => x.Name)
                .RequiredField()
                .LengthCantBeGreaterThan(300);

            RuleFor(x => x.Country)
                .RequiredField()
                .LengthCantBeGreaterThan(200)
                .Matches(patternForCountry)
                .WithMessage("Country name must start with capital symbol. It can be written as an abbreviation. Must contain only cyrillic or latin symbols");

            RuleFor(x => x.RegistrationNumber)
                .RequiredField()
                .Matches(patternForRegistrationNumber)
                .WithMessage("{PropertyName} must contains from 1 to 9 digits");

            RuleFor(x => x.YearOfPublishing)
                .DateMustBeCorrectForPatents();

            RuleFor(x => x.ApplicationDate)
                .DateMustBeCorrectForPatents();

            RuleFor(x => x.NumberOfPages)
                .GreaterThan(0)
                .WithMessage("Number of pages must be positive");

            RuleFor(x => x.Note)
                .RequiredField()
                .Length(1,2000)
                .WithMessage("Length of {PropertyName} mustn't be greater than 2000 symbols");
        }
    }
}
