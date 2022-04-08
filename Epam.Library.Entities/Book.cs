using System;
using System.Collections.Generic;

namespace Epam.Library.Entities
{
    public class Book : AbstractPaper
    {
        public IEnumerable<Person> Authors { get; set; }

        public string PlaceOfPublication { get; set; }

        public string PublishingHouse { get; set; }

        public string ISBN { get; set; }

        public int NumberOfPages { get; set; }
    }
}
