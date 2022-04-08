using Epam.Library.Entities;
using FluentValidation;

namespace Epam.Library.BLL.Validation
{
    public class BooksValidator : AbstractValidator<Book>
    {
        public BooksValidator()
        {
            CascadeMode = CascadeMode.Stop;

            const string patternForCity = "^(([А-ЯЁ][а-яё]+(([ ][А-ЯЁ]?[а-яё]+)|((-[а-яё]+-)([А-ЯЁ][а-яё]+))|(-[А-ЯЁ][а-яё]+))?)|([A-Z][a-z]+(([ ][A-Z]?[a-z]+)|((-[a-z]+-)([A-Z][a-z]+))|(-[A-Z][a-z]+))?))$";
            const string patternForISBN = "^(ISBN ([0-7]|([8-9][0-4])|(9[5-9][0-3])|(99[4-8][0-9])|(999[0-9][0-9]))-([0-9]{1,7})-([0-9]{1,7})-[0-9X])$";

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

            RuleFor(x => x.NumberOfPages)
                .GreaterThan(0)
                .WithMessage("Number of pages must be positive");

            RuleFor(x => x.Note)
                .RequiredField()
                .Length(1, 2000)
                .WithMessage("Length of {PropertyName} mustn't be greater than 2000 symbols");

            RuleFor(x => x.ISBN)
                .Matches(patternForISBN)
                .WithMessage("{PropertyName} must be introduced according to the standart");
        }
    }
}
