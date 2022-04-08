using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using FluentValidation;

namespace Epam.Library.BLL
{
    public class PersonLogic : IPersonLogic
    {
        private readonly IPersonDao _personStorage;
        private readonly IValidator<Person> _personValidator;

        public PersonLogic(IPersonDao issueStorage, IValidator<Person> personValidator)
        {
            _personStorage = issueStorage;
            _personValidator = personValidator;
        }
        public Person AddPerson(Person author, ref Response response)
        {
            if (IsNullPersonCheck(author, ref response))
            {
                return author;
            }
            var validationResult = _personValidator.Validate(author);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return author;

            }
            return _personStorage.AddPerson(author, ref response);
        }

        private bool IsNullPersonCheck(Person person, ref Response response)
        {
            if (person == null)
            {
                ErrorsManager.AddFalseResponse(ref response, "Issue can not be null");
                return true;
            }
            return false;
        }

        public void DeletePerson(int id, ref Response response)
        {
            _personStorage.DeletePerson(id, ref response);
        }

        public Person GetPersonById(int id)
        {
            return _personStorage.GetPersonById(id);
        }

        public Person UpdatePerson(Person author, ref Response response)
        {
            var validationResult = _personValidator.Validate(author);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return author;
            }
            return _personStorage.UpdatePerson(author, ref response);
        }

        public int GetPersonIdByName(string firstName, string lastName)
        {
            return _personStorage.GetPersonIdByName(firstName, lastName);
        }
    }
}
