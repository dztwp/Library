using Epam.Library.Entities;
using FluentValidation;

namespace Epam.Library.BLL.Validation
{
    public class IssueValidator : AbstractValidator<Issue>
    {
        public IssueValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(x => x.NumberOfPages)
                .GreaterThan(0)
                .WithMessage("Number of pages must be greater than 0");
            RuleFor(x => x.NumberOfIssue)
                .GreaterThan(0)
                .WithMessage("Number of issue must be greater than 0");

            RuleFor(x => x.ReleaseDay)
                .DateMustBeCorrectForBooksNewspapers();
          
        }
    }
}
