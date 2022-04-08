using Epam.Library.BLL.Interfaces;
using NUnit.Framework;
using Epam.Library.Dependencies;
using Ninject;
using Epam.Library.DAL.FakeDAL;
using Epam.Library.Entities;
using System;
using System.Collections.Generic;
using Epam.Library.ErrorArchiver;
using System.Linq;

namespace Epam.Library.BLL.IntegrationTests
{
    public class LibraryIntegrationTests
    {
        private ILibraryLogic _libraryLogic;
        private IBookLogic _bookLogic;
        private IPatentLogic _patentLogic;
        private Book _correctBook;
        private Patent _correctPatent;
        private Response _response;

        [SetUp]
        public void Setup()
        {
            DataStorage.Storage.Clear();
            _libraryLogic = DependencyResolver.NinjectKernel.Get<ILibraryLogic>();
            _bookLogic = DependencyResolver.NinjectKernel.Get<IBookLogic>();
            _patentLogic = DependencyResolver.NinjectKernel.Get<IPatentLogic>();

            _correctBook = new Book
            {
                Name = "Jija",
                Note = "This book about Jija",
                YearOfPublishing = 1974,
                PlaceOfPublication = "Engels",
                PublishingHouse = "Some",
                ISBN = "ISBN 7-4345-3543-4",
                NumberOfPages = 5,
                Authors = new List<Person>() { new Person() { Id = 1, FirstName = "Ivan", LastName = "Ivanov" } }
            };
            _correctPatent = new Patent()
            {
                Name = "Biba",
                Note = "asd",
                YearOfPublishing = 1600,
                ApplicationDate = 1599,
                Country = "Russia",
                RegistrationNumber = "685842923",
                NumberOfPages = 5,
                Authors = new List<Person> { new Person() { Id = 2, FirstName = "Roman", LastName = "Lezin" } }
            };
        }

        [Test]
        public void GettingAllNotes_NotesExistInStorage_Correct()
        {
             var collectionOfPapers = _libraryLogic.GetAll().ToList();
            Assert.Greater(collectionOfPapers.Count, 0);
        }

        [Test]
        public void GettingAllNotes_NotesIsntExistInStorage_Correct()
        {
            var collectionOfPapers = _libraryLogic.GetAll().ToList();
            Assert.AreEqual(collectionOfPapers.Count, 0);
        }

        [Test]
        public void GettingNoteByName_NoteWithNameExistInStorage_Correct()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            var noteCollection = _libraryLogic.GetByName("Jija");
            Assert.Greater(noteCollection.Count(),0);
        }

        [Test]
        public void GettingNoteByName_NoteWithNameIsNotExistInStorage_Correct()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            var noteCollection = _libraryLogic.GetByName("Jijaфыв");
            Assert.AreEqual(noteCollection.Count(), 0);
        }

        [Test]
        public void GettingNotesByDates_NotesExistInStorage_Corret()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var noteCollection = _libraryLogic.GroupByDate(1974);
            Assert.Greater(noteCollection.Count(), 0);
        }

        [Test]
        public void GettingNotesByDates_NotesIsNotExistInStorage_Corret()
        {
            _bookLogic.AddBook(_correctBook, ref _response);
            _patentLogic.AddPatent(_correctPatent, ref _response);
            var noteCollection = _libraryLogic.GroupByDate(1944);
            Assert.AreEqual(noteCollection.Count(), 0);
        }

    }
}
