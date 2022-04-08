using Epam.Library.Entities;
using System.Collections.Generic;

namespace Epam.Library.DAL.FakeDAL
{
    public static class PersonHelper
    {
        public static bool IsAuthorsListContainsAuthor(IEnumerable<Person> personsList, Person author)
        {
            foreach (var item in personsList)
            {
                if (IsAuthorsEquals(item, author))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsAuthorsEquals(Person firstPerson, Person secondPerson)
        {
            if (firstPerson == null && secondPerson == null)
                return true;
            else if (firstPerson == null || secondPerson == null)
                return false;

            return firstPerson.FirstName == secondPerson.FirstName
                && firstPerson.LastName == secondPerson.LastName;
        }
    }
}
