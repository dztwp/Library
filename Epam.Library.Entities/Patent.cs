using System;
using System.Collections.Generic;

namespace Epam.Library.Entities
{
    public class Patent : AbstractPaper
    {
        public IEnumerable<Person> Authors { get; set; }

        public string Country { get; set; }

        public string RegistrationNumber { get; set; }

        public int ApplicationDate { get; set; }

        public int NumberOfPages { get; set; }
    }
}
