using Epam.Library.Entities;
using FluentValidation;

namespace Epam.Library.BLL.Validation
{
    public class NewspapersValidator : AbstractValidator<Newspaper>
    {
        public NewspapersValidator()
        {
            CascadeMode = CascadeMode.Stop;

            const string patternForCity = "^(([А-ЯЁ][а-яё]+(([ ][А-ЯЁ]?[а-яё]+)|((-[а-яё]+-)([А-ЯЁ][а-яё]+))|(-[А-ЯЁ][а-яё]+))?)|([A-Z][a-z]+(([ ][A-Z]?[a-z]+)|((-[a-z]+-)([A-Z][a-z]+))|(-[A-Z][a-z]+))?))$";

            const string patternForISSN = "^(ISSN ([0-9]{4})-([0-9]{4}))$";

            RuleFor(x => x.Name)
                .RequiredField()
                .LengthCantBeGreaterThan(300);

            RuleFor(x => x.PlaceOfPublication)
                .RequiredField()
                .LengthCantBeGreaterThan(200)
                .CityNameMustMatchPattern(patternForCity);

            RuleFor(x => x.PublishingHouse)
                .RequiredField()
                .LengthCantBeGreaterThan(300);

            RuleFor(x => x.YearOfPublishing)
                .DateMustBeCorrectForBooksNewspapers();

            RuleFor(x => x.Note)
                .RequiredField()
                .Length(1,2000)
                .WithMessage("Length of {PropertyName} mustn't be greater than 2000 symbols");

            RuleFor(x => x.ISSN)
                .Matches(patternForISSN)
                .WithMessage("{PropertyName} must be introduced according to the standart");
        }
    }
}
