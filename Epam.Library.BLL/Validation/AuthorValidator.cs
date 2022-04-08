using Epam.Library.Entities;
using FluentValidation;

namespace Epam.Library.BLL.Validation
{
    public class AuthorValidator : AbstractValidator<Person>
    {
        public AuthorValidator()
        {
            CascadeMode = CascadeMode.Stop;

            const string patternFirstName = "^((([А-ЯЁ][а-яё]+)(-[А-ЯЁ][а-яё]+)?)|(([A-Z][a-z]+)(-[A-Z][a-z]+)?))$";
            const string patternLastName = "^((((([а-яё]+ ))((([а-яё]+'))([А-ЯЁ][а-яё]+)(-([А-ЯЁ][а-яё]+))?))|(((([а-яё]+'))([А-ЯЁ][а-яё]+)(-([А-ЯЁ][а-яё]+))?))|((([а-яё]+ ))(([А-ЯЁ][а-яё]+)(-([А-ЯЁ][а-яё]+))?))|(([А-ЯЁ][а-яё]+)(-([А-ЯЁ][а-яё]+))?))|(((([a-z]+ ))((([a-z]+'))([A-Z][a-z]+)(-([A-Z][a-z]+))?))|(((([a-z]+'))([A-Z][a-z]+)(-([A-Z][a-z]+))?))|((([a-z]+ ))(([A-Z][a-z]+)(-([A-Z][a-z]+))?))|(([A-Z][a-z]+)(-([A-Z][a-z]+))?)))$";

            RuleFor(x => x.FirstName)
                .RequiredField()
                .LengthCantBeGreaterThan(200)                
                .FirstNameMatchesPattern(patternFirstName);

            RuleFor(x => x.LastName)
                .RequiredField()
                .LengthCantBeGreaterThan(200)
                .LastNameMatchesPattern(patternLastName);
        }
    }
}
