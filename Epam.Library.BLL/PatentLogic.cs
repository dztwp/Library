using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using FluentValidation;
using System.Collections.Generic;

namespace Epam.Library.BLL
{
    public class PatentLogic : IPatentLogic
    {
        private readonly IPatentDao _patentStorage;
        private readonly IValidator<Patent> _patentValidator;
        private readonly IPersonLogic _personLogic;

        public PatentLogic(IPatentDao patentStorage, IPersonLogic personLogic, IValidator<Patent> patentValidator)
        {
            _patentStorage = patentStorage;
            _patentValidator = patentValidator;
            _personLogic = personLogic;
        }

        public Patent AddPatent(Patent patent, ref Response response)
        {
            if (IsNullPatentCheck(patent, ref response))
            {
                return patent;
            }
            var validationResult = _patentValidator.Validate(patent);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return patent;

            }
            var returnedBook = _patentStorage.AddPatent(patent, ref response);
            response = AddingAuthorsToPatent(response, returnedBook);
            return returnedBook;
        }

        private Response AddingAuthorsToPatent(Response response, Patent returnedBook)
        {
            if (response.IsSuccess)
            {
                foreach (var person in returnedBook.Authors)
                {
                    var retPerson = _personLogic.AddPerson(person, ref response);
                    if (retPerson.Id == 0)
                    {
                        retPerson.Id = _personLogic.GetPersonIdByName(retPerson.FirstName, retPerson.LastName);
                    }
                    AddPersonToPatent(returnedBook.Id, retPerson);
                }
            }

            return response;
        }

        private bool IsNullPatentCheck(Patent patent, ref Response response)
        {
            if (patent == null)
            {
                ErrorsManager.AddFalseResponse(ref response, "Patent can not be null");
                return true;
            }
            return false;
        }
        public void DeletePatent(int Id, ref Response response)
        {
            _patentStorage.DeletePatent(Id, ref response);
        }

        public IEnumerable<Patent> GetAllPatents()
        {
            return _patentStorage.GetAllPatents();
        }

        public IEnumerable<Patent> GetPatentsByAuthor(Person author)
        {
            return author != null ?
                _patentStorage.GetPatentsByAuthor(author) :
                null;
        }

        public IEnumerable<Patent> GetPatentsByCharacterSet(string characterSet)
        {
            return characterSet != null ?
                _patentStorage.GetPatentByCharacterSet(characterSet) :
                null;
        }

        public IEnumerable<Patent> GetOrderedByPatents()
        {
            return _patentStorage.GetOrderedByPatents();
        }

        public IEnumerable<Patent> GetOrderedByDescPatents()
        {
            return _patentStorage.GetOrderedByDescPatents();
        }

        public Patent GetPatentById(int id)
        {
            return _patentStorage.GetPatentById(id);
        }

        public Patent UpdatePatent(Patent patent, ref Response response)
        {
            var validationResult = _patentValidator.Validate(patent);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return patent;

            }
            return _patentStorage.UpdatePatent(patent, ref response);
        }

        public void AddPersonToPatent(int patentId, Person author)
        {
            _patentStorage.AddPersonToPatent(patentId, author);
        }
    }
}
