using System;
using System.Collections.Generic;

namespace Epam.Library.Entities
{
    public class Newspaper : AbstractPaper
    {
        public IEnumerable<Issue> CollectionOfIssues { get; set; }

        public string ISSN { get; set; }

        public string PlaceOfPublication { get; set; }

        public string PublishingHouse { get; set; }
    }
}
