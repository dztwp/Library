using Epam.Library.BLL;
using Epam.Library.BLL.Interfaces;
using Epam.Library.BLL.Validation;
using Epam.Library.DAL.FakeDAL;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.FakeDAL;
using FluentValidation;
using Ninject.Modules;

namespace Epam.Library.NinjectBinds
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IBookLogic>().To<BookLogic>();
            Bind<INewspaperLogic>().To<NewspaperLogic>();
            Bind<IPatentLogic>().To<PatentLogic>();
            Bind<ILibraryLogic>().To<LibraryLogic>();
            Bind<IPersonLogic>().To<PersonLogic>();
            Bind<IIssueLogic>().To<IssueLogic>();

            Bind<IBookDao>().To<MSSQLDAL.BookDao>();
            Bind<INewspaperDao>().To<MSSQLDAL.NewspaperDao>();
            Bind<IPatentDao>().To<MSSQLDAL.PatentDao>();
            Bind<ILibraryDao>().To<MSSQLDAL.LibraryDao>();
            Bind<IPersonDao>().To<MSSQLDAL.PersonDao>();
            Bind<IIssueDao>().To<MSSQLDAL.IssueDao>();

            Bind<IValidator<Book>>().To<BooksValidator>();
            Bind<IValidator<Newspaper>>().To<NewspapersValidator>();
            Bind<IValidator<Patent>>().To<PatentValidator>();
            Bind<IValidator<Issue>>().To<IssueValidator>();
            Bind<IValidator<Person>>().To<AuthorValidator>();
        }
    }
}
