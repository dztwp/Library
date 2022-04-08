using Epam.Library.Entities;
using System.Collections.Generic;

namespace Epam.Library.DAL.FakeDAL
{
    public static class DataStorage
    {
        static DataStorage()
        {
            Storage = new Dictionary<int, AbstractPaper>();
            AuthorsStorage = new Dictionary<int, Person>();
            IssueStorage = new Dictionary<int, Issue>();
        }
        public static Dictionary<int, AbstractPaper> Storage { get; private set; }
        public static Dictionary<int, Person> AuthorsStorage { get; private set; }
        public static Dictionary<int, Issue> IssueStorage { get; private set; }

    }
}
