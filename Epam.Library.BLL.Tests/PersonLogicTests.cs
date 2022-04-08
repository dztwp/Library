using Epam.Library.BLL.Interfaces;
using Epam.Library.DAL.Interfaces;
using Epam.Library.Entities;
using Epam.Library.ErrorArchiver;
using Moq;
using NUnit.Framework;

namespace Epam.Library.BLL.Tests
{
    public class PersonLogicTests
    {
        private Response _response;
        private Person _correctPerson;
        private Person _inCorrectPerson;

        [SetUp]
        public void Setup()
        {
            _response = new Response();
            _correctPerson = new Person()
            {
                Id = 1,
                FirstName = "Roman",
                LastName = "Lezin"
            };

            _inCorrectPerson = new Person()
            {
                Id = 1,
                FirstName = "asffRoman",
                LastName = "asdLezin"
            };

        }
        delegate void AddingCallback(Person person, ref Response response);

        [Test]
        public void AddingPerson_CorrectPerson_Correct()
        {
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO.Setup(obj => obj.AddPerson(_correctPerson, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Person x, ref Response y) => y.IsSuccess = true));
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            personLogic.AddPerson(_correctPerson, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void AddingPerson_InCorrectPerson_Incorrect()
        {
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO.Setup(obj => obj.AddPerson(_correctPerson, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Person x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("The specified newspaper does not exist in the data storage"));
                    y.IsSuccess = false;
                }));
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            personLogic.AddPerson(_inCorrectPerson, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        delegate void DeleteCallback(int id, ref Response response);

        [Test]
        public void DeletingPerson_DataWithIdIsExist_Correct()
        {
            int id = 1;
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO
                .Setup(obj => obj.DeletePerson(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) => y.IsSuccess = true));
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            personLogic.DeletePerson(id, ref _response);
            Assert.AreEqual(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void DeletingPerson_DataWithIdIsNotExist_Incorrect()
        {
            int id = 1;
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO
                .Setup(obj => obj.DeletePerson(id, ref It.Ref<Response>.IsAny))
                .Callback(new DeleteCallback((int x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("There is no data with the specified id in the data storage"));
                    y.IsSuccess = false;
                }
                ));
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            personLogic.DeletePerson(id, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }

        [Test]
        public void GettingPersonById_PersonWithEnteredIdIsNotExistInStorage_Incorrect()
        {
            int id = 1;
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO
                .Setup(obj => obj.GetPersonById(id))
                .Returns((Person)null);
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            var person = personLogic.GetPersonById(id);
            Assert.AreEqual(person, null);
        }

        [Test]
        public void GettingPersonById_PersonWithEnteredExistInStorage_Correct()
        {
            int id = 1;
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO
                .Setup(obj => obj.GetPersonById(id))
                .Returns(_correctPerson);
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            var person = personLogic.GetPersonById(id);
            Assert.IsNotNull(person);
        }

        delegate void UpdateCallback(Person issue, ref Response response);

        [Test]
        public void UpdatingPerson_CorrectIssue_Correct()
        {
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO.Setup(obj => obj.UpdatePerson(_correctPerson, ref It.Ref<Response>.IsAny))
                .Callback(new UpdateCallback((Person x, ref Response y) => y.IsSuccess = true));
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            personLogic.UpdatePerson(_correctPerson, ref _response);
            Assert.AreEqual(0, _response.ErrorCollection.Count);
        }

        [Test]
        public void UpdatingPerson_InCorrectPerson_Incorrect()
        {
            var personValidator = new Validation.AuthorValidator();
            var personDAO = new Mock<IPersonDao>();
            personDAO.Setup(obj => obj.UpdatePerson(_correctPerson, ref It.Ref<Response>.IsAny))
                .Callback(new AddingCallback((Person x, ref Response y) =>
                {
                    y.ErrorCollection.Add(new Error("The specified newspaper does not exist in the data storage"));
                    y.IsSuccess = false;
                }));
            IPersonLogic personLogic = new PersonLogic(personDAO.Object, personValidator);
            personLogic.UpdatePerson(_inCorrectPerson, ref _response);
            Assert.Greater(_response.ErrorCollection.Count, 0);
        }
    }
}



