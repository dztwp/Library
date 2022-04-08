using Epam.Library.BLL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using Ninject;
using NUnit.Framework;
using Epam.Library.Dependencies;
using System.Linq;
using Epam.Library.DAL.FakeDAL;

namespace Epam.Library.BLL.IntegrationTests
{
    public class PersonIntegrationTests
    {
        private Response _response;
        private IPersonLogic _personLogic;
        private Person _correctPerson;

        [SetUp]
        public void Setup()
        {
            DataStorage.AuthorsStorage.Clear();
            _correctPerson = new Person
            {
                FirstName = "Lev",
                LastName = "Gnomich"
            };
            _response = new Response();
            _personLogic = DependencyResolver.NinjectKernel.Get<IPersonLogic>();
        }

        [Test]
        public void AddingPerson_PersonAlreadyExistInDataStorage_Incorrect()
        {
            _personLogic.AddPerson(_correctPerson, ref _response);
            _personLogic.AddPerson(_correctPerson, ref _response);

            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "This author already exist in data storage");
        }

        [Test]
        public void AddingPerson_PersonIsNotAlreadyExistInDataStorage_Incorrect()
        {
            _personLogic.AddPerson(_correctPerson, ref _response);

            Assert.Greater(_correctPerson.Id, 0);
        }

        [Test]
        public void DeletingPerson_PersonExistInStorageWithEnteredId_Correct()
        {
            _personLogic.AddPerson(_correctPerson, ref _response);
            _personLogic.DeletePerson(1, ref _response);

            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void DeletingPerson_PersonWithEnteredIdIsNotExist_Incorrect()
        {
            _personLogic.DeletePerson(2, ref _response);

            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "There is no data with the specified id in the data storage");
        }

        [Test]
        public void GettingPerson_PersonWithEnteredIdExistInStorage_Correct()
        {
            _personLogic.AddPerson(_correctPerson, ref _response);
            var person = _personLogic.GetPersonById(1);

            Assert.IsNotNull(person);
        }

        [Test]
        public void GettingPerson_PersonWithEnteredIdIsNotExistInStorage_Correct()
        {
            _personLogic.AddPerson(_correctPerson, ref _response);
            var person = _personLogic.GetPersonById(2);

            Assert.IsNull(person);
        }
    }
}
