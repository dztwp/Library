using Epam.Library.BLL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Epam.Library.Dependencies;
using Ninject;
using System.Linq;
using Epam.Library.DAL.FakeDAL;

namespace Epam.Library.BLL.IntegrationTests
{
    public class PatentIntegrationTests
    {
        private Response _response;
        private Patent _correctPatent;
        private Person _author;
        private IPatentLogic _patentLogic;

        [SetUp]
        public void Setup()
        {
            DataStorage.Storage.Clear();
            _response = new Response();
            _author = new Person() { Id = 2, FirstName = "Roman", LastName = "Lezin" };
            _correctPatent = new Patent()
            {
                Name = "Biba",
                Note = "asd",
                YearOfPublishing = 1600,
                ApplicationDate = 1599,
                Country = "Russia",
                RegistrationNumber = "685842923",
                NumberOfPages = 5,
                Authors = new List<Person> { _author }
            };
            _patentLogic = DependencyResolver.NinjectKernel.Get<IPatentLogic>();
        }

        [Test]
        public void PatentAdding_CorrectPatent_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            Assert.True(_response.IsSuccess);
        }

        [Test]
        public void PatentAdding_TryToAddEqualPatent_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            _patentLogic.AddPatent(_correctPatent, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "Patent with this registration number and country is already exist");
        }

        [Test]
        public void PatentDeleting_PatentWithEnteredIdExistInStorage_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            _patentLogic.DeletePatent(1, ref _response);
            Assert.True(_response.IsSuccess);
        }

        [Test]
        public void PatentDeleting_PatentWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            _patentLogic.DeletePatent(2, ref _response);
            Assert.AreEqual(_response.ErrorCollection.FirstOrDefault().ErrorDescription, "There is no data with the specified id in the data storage");
        }

        [Test]
        public void PatentGettingById_PatentWithEnteredIdExistInStorage_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patent = _patentLogic.GetPatentById(1);
            Assert.IsNotNull(patent);
        }

        [Test]
        public void PatentGettingById_PatentWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patent = _patentLogic.GetPatentById(3);
            Assert.IsNull(patent);
        }

        [Test]
        public void GettingAllPatentes_PatentExistInStorage_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patents = _patentLogic.GetAllPatents();
            Assert.Greater(patents.Count(), 0);
        }

        [Test]
        public void GettingAllPatentes_PatentDoNotExistInStorage_Incorrect()
        {
            var patents = _patentLogic.GetAllPatents();
            Assert.AreEqual(patents.Count(), 0);
        }

        [Test]
        public void GettingPatentsByAuthor_NoteWithAuthorExistInStorage_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patents = _patentLogic.GetPatentsByAuthor(_author);
            Assert.Greater(patents.Count(), 0);
        }

        [Test]
        public void GettingPatentsByAuthor_NoteWithAuthorIsNotExistInStorage_Correct()
        {
            Person newPerson = new Person()
            {
                Id = 1,
                FirstName = "Jonh",
                LastName = "Paul"
            };
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patents = _patentLogic.GetPatentsByAuthor(newPerson);
            Assert.AreEqual(patents.Count(), 0);
        }

        [Test]
        public void GettingPatentsByAuthor_AuthorIsNull_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patents = _patentLogic.GetPatentsByAuthor(null);
            Assert.IsNull(patents);
        }

        [Test]
        public void GettingPatentsByCharSet_CharSetIsCorrect_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patentCollection = _patentLogic.GetPatentsByCharacterSet("Bi");
            Assert.Greater(patentCollection.Count(), 0);
        }

        [Test]
        public void GettingPatentsByCharSet_CharSetIsNull_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patentCollection = _patentLogic.GetPatentsByCharacterSet(null);
            Assert.IsNull(patentCollection);
        }

        [Test]
        public void GettingPatentsByCharSet_CharSetIsIncorrect_Incorrect()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patentCollection = _patentLogic.GetPatentsByCharacterSet("asd");
            Assert.AreEqual(patentCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedPatents_StorageIsNotEmpty_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patentCollection = _patentLogic.GetOrderedByPatents();
            Assert.Greater(patentCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedPatents_StorageIsEmpty_Correct()
        {
            var patentCollection = _patentLogic.GetOrderedByPatents();
            Assert.AreEqual(patentCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedByDescPatents_StorageIsNotEmpty_Correct()
        {
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var patentCollection = _patentLogic.GetOrderedByDescPatents();
            Assert.Greater(patentCollection.Count(), 0);
        }

        [Test]
        public void GettingOrderedByDescPatents_StorageIsEmpty_Correct()
        {
            var patentCollection = _patentLogic.GetOrderedByDescPatents();
            Assert.AreEqual(patentCollection.Count(), 0);
        }
    }
}
