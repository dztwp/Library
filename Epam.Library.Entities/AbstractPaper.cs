using System;

namespace Epam.Library.Entities
{
    public abstract class AbstractPaper
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        public int YearOfPublishing { get; set; }

    }
}
