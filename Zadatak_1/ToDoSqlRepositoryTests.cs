using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak_1;

namespace Zadatak_1.Tests
{
    [TestClass]
    public class ToDoSqlRepositoryTests
    {
        //Begin tests for Add
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingNullToDatabaseThrowsException()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            repository.Add(null);

        }

        [TestMethod]
        public void AddReallyAdds()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            repository.Add(item);
            Assert.AreEqual(1, repository.GetAll().Count);
            Assert.IsTrue(repository.Get(item.ID) != null);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateTodoItemException))]
        public void AddingDupicateItemsThrowsDTDIE()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            repository.Add(item);
            repository.Add(item);
        }

        //end test for Add

        //begin test for Get
        [TestMethod]
        public void GetValidItemGetsItem()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            repository.Add(item);
            Assert.AreEqual(item, repository.Get(item.ID));
        }

        [TestMethod]
        public void GettingInvalidItemReturnsNull()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            repository.Add(item1);
            Assert.AreEqual(null, repository.Get(item2.ID));
        }

        [TestMethod]
        public void GettingItemFromEmptyRepositoryReturnsNull()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            Assert.AreEqual(null, repository.Get(item.ID));
        }

        //End tests for Get

        //begin tests for Remove
        [TestMethod]
        public void RemoveReallyRemoves()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            repository.Add(item1);
            repository.Add(item2);
            Assert.IsTrue(repository.Remove(item1.ID));
            Assert.AreEqual(1, repository.GetAll().Count);
            repository.Add(item1);
        }

        [TestMethod]
        public void RemoveReturnsFalseForInvalidRequest()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            repository.Add(item1);
            Assert.IsFalse(repository.Remove(item2.ID));
            Assert.AreEqual(1, repository.GetAll().Count);
        }

        //end tests for Remove


        //begin tests for Update
        [TestMethod]
        public void UpdateAddsNonexistingItem()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            repository.Update(item);
            Assert.AreEqual(repository.GetAll().Count, 1);
            Assert.IsTrue(repository.Get(item.ID) == item);
        }

        [TestMethod]
        public void UpdateUpdatesExistingItems()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            repository.Add(item);
            item.Text = "changed";
            repository.Update(item);
            Assert.AreEqual(repository.GetAll().Count, 1);
            Assert.IsTrue(String.Compare(repository.Get(item.ID).Text, "changed") == 0);
        }

        //end tests for Update

        //begin tests for MarkAsCompleted
        [TestMethod]
        public void MarkAsCompletedMarksExistingItem()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item = new ToDoItem("test");
            repository.Add(item);
            Assert.IsTrue(repository.MarkAsCompleted(item.ID));
            Assert.AreEqual(repository.GetAll().Count, 1);
            Assert.IsTrue(repository.Get(item.ID).IsCompleted);
        }

        [TestMethod]
        public void MarkAsCompletedIgnoresNonExistingItems()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            repository.Add(item1);
            Assert.IsFalse(repository.MarkAsCompleted(item2.ID));
            Assert.AreEqual(repository.GetAll().Count, 1);
            Assert.IsFalse(repository.Get(item1.ID).IsCompleted);
        }

        //end tests for MarkAsCompleted

        //begin tests for GetActive
        [TestMethod]
        public void GetActiveRetursAllActiveItems()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            item1.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetActive().IndexOf(item1) == -1);
            Assert.IsTrue(repository.GetActive().IndexOf(item2) == 0);
            Assert.AreEqual(repository.GetActive().Count, 1);


        }
        //end tests for GetActive

        [TestMethod]
        public void GetActiveReturnsNullWhenAllItemsAreActive()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            item1.MarkAsDone();
            item2.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetActive() == null);
        }

        [TestMethod]
        public void GetActiveReturnsNullWhenRepositoryIsEmpty()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            Assert.IsTrue(repository.GetActive() == null);
        }

        //end tests for GetActive

        //begin tests for GetAll
        [TestMethod]
        public void GetAllGetsAllItems()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            item2.DateCreated = DateTime.UtcNow;
            repository.Add(item2);
            repository.Add(item1);
            List<ToDoItem> list = repository.GetAll();
            Assert.AreEqual(list.Count, 2);
            Assert.IsTrue(list.IndexOf(item1) == 0 && list.IndexOf(item2) == 1);
        }

        [TestMethod]
        public void GetAllReturnsNullWhenRepositoryIsEmpty()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            Assert.IsTrue(repository.GetAll() == null);
        }

        //end tests for GetAll

        //begin tests for GetCompleted
        [TestMethod]
        public void GetCompletedRetursAllCompletedItems()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            item1.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetCompleted().IndexOf(item1) == 0);
            Assert.AreEqual(repository.GetCompleted().Count, 1);
        }

        [TestMethod]
        public void GetCompletedReturnsNullWhenAllItemsAreActive()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetCompleted() == null);
        }

        [TestMethod]
        public void GetCompletedReturnsNullWhenRepositoryIsEmpty()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            Assert.IsTrue(repository.GetCompleted() == null);
        }
        //end tests for GetCompleted

        //begin tests for GetFiltered
        [TestMethod]
        public void GetFilteredReturnsItemsMeetingTheCondition()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            ToDoItem item3 = new ToDoItem("test2");
            repository.Add(item2);
            repository.Add(item1);
            repository.Add(item3);
            var testList = repository.GetFiltered(x => String.Compare(x.Text, "test2") == 0);
            Assert.IsTrue(testList.Count == 2 && testList.IndexOf(item2) != -1 && testList.IndexOf(item3) != 0);
        }

        [TestMethod]
        public void GetFilteredReturnsNullWhenNoItemMeetsTheCondition()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            ToDoItem item1 = new ToDoItem("test1");
            ToDoItem item2 = new ToDoItem("test2");
            ToDoItem item3 = new ToDoItem("test3");
            repository.Add(item2);
            repository.Add(item1);
            repository.Add(item3);
            Assert.AreEqual(repository.GetFiltered(x => String.Compare(x.Text, "test4") == 0), null);
        }

        [TestMethod]
        public void GetFilteredReturnsNullIfRepositoryIsEmpty()
        {
            IToDoRepository repository = new ToDoSqlRepository();
            Assert.AreEqual(repository.GetFiltered(x => String.Compare(x.Text, "test4") == 0), null);
        }
        //end tests for GetFiltered
    }

}