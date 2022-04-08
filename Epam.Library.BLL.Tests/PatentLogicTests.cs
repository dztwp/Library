using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Epam.Library.BLL.Tests
{
    public class PatentLogicTests
    {
        private Response _response;
        private Patent _correctPatent;
        private Patent _inCorrectPatent;
        private Person _author;

        [SetUp]
        public void Setup()
        {
            _response = new Response();
            _author = new Person() { Id = 2, FirstName = "Roman", LastName = "Lezin" };
            _correctPatent = new Patent()
            {
                Id = 1,
                Name = "Biba",
                Note = "asd",
                YearOfPublishing = 1600,
                ApplicationDate = 1599,
                Country = "Russia",
                RegistrationNumber = "685842923",
                NumberOfPages = 5,
                Authors = new List<Person> { _author }
            };
            _inCorrectPatent = new Patent()
            {
                Id = 1,
                Name = "Biba",
                Note = "asd",
                YearOfPublishing = 1600,
                ApplicationDate = 1599,
                Country = "asdRussia",
                RegistrationNumber = "685842923",
                NumberOfPages = 5,
                Authors = new List<Person> { _author }
            };
        }

        delegate void AddingCallback(Patent book, ref Response response);

        [Test]
        public void AddingPatent_NewCorrectPatent_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            personLogic.Setup(personLog => personLog.AddPerson(_author, ref It.Ref<Response>.IsAny)).Returns(_author);
            patentDAO.Setup(obj => obj.AddPatent(_correctPatent, ref It.Ref<Response>.IsAny)).Returns(_correctPatent)
                .Callback(new AddingCallback((Patent x, ref Response y) => y.IsSuccess = true));
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            patentLogic.AddPatent(_correctPatent, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void AddingPatent_NewInCorrectPatent_Incorrect()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            personLogic.Setup(personLog => personLog.AddPerson(_author, ref It.Ref<Response>.IsAny)).Returns(_author);
            patentDAO.Setup(obj => obj.AddPatent(_inCorrectPatent, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Patent x, ref Response y) => y.IsSuccess = true));
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            patentLogic.AddPatent(_inCorrectPatent, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        delegate void DeleteCallback(int id, ref Response response);

        [Test]
        public void DeletingPatent_DataWithIdIsExist_Correct()
        {
            int id = 1;
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.DeletePatent(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) => y.IsSuccess = true));
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            patentLogic.DeletePatent(id, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void DeletingPatent_DataWithIdIsNotExist_Incorrect()
        {
            int id = 1;
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.DeletePatent(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("There is no data with the specified id in the data storage"));
                    y.IsSuccess = false;
                }
                ));
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            patentLogic.DeletePatent(id, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void GettingAllPatents_PatentsExistsInStorage_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetAllPatents())
                .Returns(new List<Patent>() { _inCorrectPatent });
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetAllPatents();
            Assert.Greater(patentCollection.Count, 0);
        }

        [Test]
        public void GettingAllPatents_PatentsIsNotExistsInStorage_Incorrect()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetAllPatents())
                .Returns(new List<Patent>());
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetAllPatents();
            Assert.AreEqual(patentCollection.Count, 0);
        }

        [Test]
        public void GettingPatentsByAuthor_AuthorsPatentExistInStorage_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetPatentsByAuthor(_author))
                .Returns(new List<Patent>() { _inCorrectPatent });
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetPatentsByAuthor(_author);
            Assert.Greater(patentCollection.Count, 0);
        }

        [Test]
        public void GettingPatentsByAuthor_AuthorsPatentsIsNotExistInStorage_Incorrect()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetPatentsByAuthor(_author))
                .Returns(new List<Patent>());
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetPatentsByAuthor(_author);
            Assert.AreEqual(patentCollection.Count, 0);
        }

        [Test]
        public void GettingPatentById_PatentWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            int id = 1;
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetPatentById(id))
                .Returns((Patent)null);
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patent = patentLogic.GetPatentById(id);
            Assert.AreEqual(patent, null);
        }

        [Test]
        public void GettingPatentById_PatentWithEnteredExistInStorage_Correct()
        {
            int id = 1;
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetPatentById(id))
                .Returns(_correctPatent);
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patent = patentLogic.GetPatentById(id);
            Assert.IsNotNull(patent);
        }

        [Test]
        public void GettingPatentByCharacterSet_PatentsWithNameStartedOnCharacterSetIsExist_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetPatentByCharacterSet("Bi"))
                .Returns(new List<Patent>() { _correctPatent });
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetPatentsByCharacterSet("Bi");
            Assert.Greater(patentCollection.Count, 0);
        }

        [Test]
        public void GettingPatentByCharacterSet_PatentsWithNameStartedOnCharacterSetIsNull_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetPatentByCharacterSet(null))
                .Returns((IEnumerable<Patent>)null);
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetPatentsByCharacterSet(null);
            Assert.AreEqual(patentCollection, null);
        }

        [Test]
        public void GettingOrderedPatents_PatentsExistInStorage_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetOrderedByPatents())
                .Returns(new List<Patent>() { _correctPatent });
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetOrderedByPatents();
            Assert.Greater(patentCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedPatents_PatentsIsNotExistInStorage_Incorrect()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetOrderedByPatents())
                .Returns(new List<Patent>());
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetOrderedByPatents();
            Assert.AreEqual(patentCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedByDescPatents_PatentExistInStorage_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetOrderedByDescPatents())
                .Returns(new List<Patent>() { _correctPatent });
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetOrderedByDescPatents();
            Assert.Greater(patentCollection.Count, 0);
        }

        [Test]
        public void GettingOrderedByDescPatents_PatentsIsNotExistInStorage_Incorrect()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO
                .Setup(obj => obj.GetOrderedByDescPatents())
                .Returns(new List<Patent>());
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            var patentCollection = (List<Patent>)patentLogic.GetOrderedByDescPatents();
            Assert.AreEqual(patentCollection.Count, 0);
        }

        delegate void UpdatingCallback(Patent patent, ref Response response);

        [Test]
        public void UpdatingPatent_NewCorrectPatent_Correct()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO.Setup(obj => obj.UpdatePatent(_correctPatent, ref It.Ref<Response>.IsAny))
                .Callback(new UpdatingCallback((Patent x, ref Response y) => y.IsSuccess = true));
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            patentLogic.UpdatePatent(_correctPatent, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void UpdatingBook_NewInCorrectBook_Incorrect()
        {
            var patentValidator = new Validation.PatentValidator();
            var patentDAO = new Mock<IPatentDao>();
            var personLogic = new Mock<IPersonLogic>();

            patentDAO.Setup(obj => obj.UpdatePatent(_inCorrectPatent, ref It.Ref<Response>.IsAny))
                .Callback(new UpdatingCallback((Patent x, ref Response y) => y.IsSuccess = true));
            IPatentLogic patentLogic = new PatentLogic(patentDAO.Object, personLogic.Object, patentValidator);
            patentLogic.UpdatePatent(_inCorrectPatent, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }
    }
}
