using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using FluentValidation;
using System.Collections.Generic;

namespace Epam.Library.BLL
{
    public class NewspaperLogic : INewspaperLogic
    {
        private readonly INewspaperDao _newspaperStorage;
        private readonly IValidator<Newspaper> _newspaperValidator;

        public NewspaperLogic(INewspaperDao newspaperStorage, IValidator<Newspaper> newspaperValidator)
        {
            _newspaperStorage = newspaperStorage;
            _newspaperValidator = newspaperValidator;
        }

        public Newspaper AddNewspaper(Newspaper newspaper, ref Response response)
        {
            if (IsNullNewspaperCheck(newspaper, ref response))
            {
                return newspaper;
            }
            var validationResult = _newspaperValidator.Validate(newspaper);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return newspaper;

            }
            return _newspaperStorage.AddNewspaper(newspaper, ref response);
        }

        private bool IsNullNewspaperCheck(Newspaper newspaper, ref Response response)
        {
            if (newspaper == null)
            {
                ErrorsManager.AddFalseResponse(ref response, "Newspaper can not be null");
                return true;
            }
            return false;
        }
        public Newspaper AddIssueToNewspaper(Issue newIssue, Newspaper newspaper, ref Response response)
        {
            if (newspaper.YearOfPublishing > newIssue.ReleaseDay)
            {
                ErrorsManager.AddFalseResponse(ref response, "Issues date of release must be greater than newspaper year of publishing");
                return newspaper;
            }
            return _newspaperStorage.AddIssueToNewspaper(newIssue, newspaper, ref response);
        }

        public void DeleteNewspaper(int id, ref Response response)
        {
            _newspaperStorage.DeleteNewspaper(id, ref response);
        }

        public IEnumerable<Newspaper> GetAllNewspapers()
        {
            return _newspaperStorage.GetAllNewspapers();
        }

        public Newspaper GetNewspaperById(int id)
        {
            return _newspaperStorage.GetNewspaperById(id);
        }

        public IEnumerable<Newspaper> GetNewspapersByCharacterSet(string characterSet)
        {
            return characterSet != null ?
                _newspaperStorage.GetNewspapersByCharacterSet(characterSet)
                : null;
        }

        public IEnumerable<Newspaper> GetOrderedByNewspapers()
        {
            return _newspaperStorage.GetOrderedByNewspapers();
        }

        public IEnumerable<Newspaper> GetOrderedByDescNewspapers()
        {
            return _newspaperStorage.GetOrderedByDescNewspapers();
        }

        public IEnumerable<Newspaper> GetNewspapersByPublishingHouse(string publishingHouse)
        {
            return publishingHouse != null ?
                _newspaperStorage.GetNewspapersByPublishingHouse(publishingHouse)
                : null;
        }

        public Newspaper UpdateNewspaper(Newspaper newspaper, ref Response response)
        {
            var validationResult = _newspaperValidator.Validate(newspaper);
            if (!validationResult.IsValid)
            {
                ErrorsManager.AddFalseResponse(ref response, validationResult);
                return newspaper;
            }
            return _newspaperStorage.UpdateNewspaper(newspaper, ref response);
        }
    }
}
