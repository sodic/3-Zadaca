using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1.Tests
{
    [TestClass]
    public class ToDoSqlRepositoryTests
    {

        private const string ConnectionString= "Server=(localdb)\\mssqllocaldb;Database=ToDoDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        ToDoDbContext _db= new ToDoDbContext(ConnectionString);
        //Begin tests for Add
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddingNullToDatabaseThrowsException()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            repository.Add(null);
        }

        [TestMethod]
        public void AddReallyAdds()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var id = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test", id);
            repository.Add(item);
            Assert.AreEqual(1, repository.GetAll(id).Count);
            Assert.IsTrue(repository.Get(item.Id, id) != null);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateTodoItemException))]
        public void AddingDupicateItemsThrowsDTDIE()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var id = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test", id);
            repository.Add(item);
            repository.Add(item);
        }

        //end test for Add

        //begin test for Get
        [TestMethod]
        public void GetValidItemGetsItem()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test",id1);
            repository.Add(item);
            Assert.AreEqual(item, repository.Get(item.Id,id1));
        }

        [TestMethod]
        [ExpectedException(typeof(ToDoAccesDeniedException))]
        public void GetWithUnauthorizedIdThrowsToDoAccesDeniedException()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test", id1);
            repository.Add(item);
            repository.Get(item.Id, id2);
        }

        [TestMethod]
        public void GettingInvalidItemReturnsNull()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId);
            repository.Add(item1);
            Assert.AreEqual(null, repository.Get(item2.Id,userId));
        }

        [TestMethod]
        public void GettingItemFromEmptyRepositoryReturnsNull()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test",userId);
            Assert.AreEqual(null, repository.Get(item.Id,userId));
        }

        //End tests for Get

        //begin tests for Remove
        [TestMethod]
        public void RemoveReallyRemoves()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId);
            repository.Add(item1);
            repository.Add(item2);
            Assert.IsTrue(repository.Remove(item1.Id,userId));
            Assert.AreEqual(1, repository.GetAll(userId).Count);
            repository.Add(item1);
        }

        [TestMethod]
        public void RemoveReturnsFalseForInvalidRequest()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId);
            repository.Add(item1);
            Assert.IsFalse(repository.Remove(item2.Id,userId));
            Assert.AreEqual(1, repository.GetAll(userId).Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ToDoAccesDeniedException))]
        public void RemovingByUnauthorisedUserThrowsToDoAccessDeniedException()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1", userId);
            repository.Add(item1);
            repository.Remove(item1.Id, Guid.NewGuid());
        }

        //end tests for Remove


        //begin tests for Update
        [TestMethod]
        public void UpdateAddsNonexistingItem()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test",userId);
            repository.Update(item,userId);
            Assert.AreEqual(repository.GetAll(userId).Count, 1);
            Assert.IsTrue(repository.Get(item.Id,userId) == item);
        }

        [TestMethod]
        public void UpdateUpdatesExistingItems()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test",userId);
            repository.Add(item);
            item.Text = "changed";
            repository.Update(item,userId);
            Assert.AreEqual(repository.GetAll(userId).Count, 1);
            Assert.IsTrue(String.Compare(repository.Get(item.Id,userId).Text, "changed") == 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ToDoAccesDeniedException))]
        public void UpdateByUnauthorisedUserThrowsTDADE()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test", userId);
            repository.Add(item);
            item.Text = "changed";
            repository.Update(item, userId2);
        }
        //end tests for Update

        //begin tests for MarkAsCompleted
        [TestMethod]
        public void MarkAsCompletedMarksExistingItem()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item = new ToDoItem("test",userId);
            repository.Add(item);
            Assert.IsTrue(repository.MarkAsCompleted(item.Id,userId));
            Assert.AreEqual(repository.GetAll(userId).Count, 1);
            Assert.IsTrue(repository.Get(item.Id,userId).IsCompleted);
        }

        [TestMethod]
        public void MarkAsCompletedIgnoresNonExistingItems()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId);
            repository.Add(item1);
            Assert.IsFalse(repository.MarkAsCompleted(item2.Id,userId));
            Assert.AreEqual(repository.GetAll(userId).Count, 1);
            Assert.IsFalse(repository.Get(item1.Id,userId).IsCompleted);
        }

        [TestMethod]
        [ExpectedException(typeof(ToDoAccesDeniedException))]
        public void MarkAsCompleteByUnauthorisedUserThrowsToDoAccessDeniedException()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1", userId);
            repository.Add(item1);
            repository.MarkAsCompleted(item1.Id, Guid.NewGuid());
        }

        //end tests for MarkAsCompleted

        //begin tests for GetActive
        [TestMethod]
        public void GetActiveRetursAllActiveItemsForUser()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId);
            item1.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetActive(userId).IndexOf(item1) == -1);
            Assert.IsTrue(repository.GetActive(userId).IndexOf(item2) == 0);
            Assert.AreEqual(repository.GetActive(userId).Count, 1);
        }
        //end tests for GetActive

        [TestMethod]
        public void GetActiveReturnsNullWhenAllUserItemsAreInactive()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId2);
            item1.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetActive(userId) == null);
        }

        [TestMethod]
        public void GetActiveReturnsNullWhenRepositoryIsEmptyForUser()
        {
            var userId = Guid.NewGuid();
            IToDoRepository repository = new ToDoSqlRepository(_db);
            Assert.IsTrue(repository.GetActive(userId) == null);
        }

        //end tests for GetActive

        //begin tests for GetAll
        [TestMethod]
        public void GetAllGetsAllItemsForUser()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId);
            ToDoItem item2 = new ToDoItem("test2",userId);
            ToDoItem item3=new ToDoItem("item3",userId2);
            item2.DateCreated = DateTime.UtcNow;
            repository.Add(item2);
            repository.Add(item1);
            repository.Add(item3);
            List<ToDoItem> list = repository.GetAll(userId);
            Assert.AreEqual(list.Count, 2);
            Assert.IsTrue(list.IndexOf(item1) == 0 && list.IndexOf(item2) == 1);
        }

        [TestMethod]
        public void GetAllReturnsNullWhenRepositoryIsEmptyForUser()
        {
            var userId = Guid.NewGuid();
            IToDoRepository repository = new ToDoSqlRepository(_db);
            Assert.IsTrue(repository.GetAll(userId) == null);
        }

        //end tests for GetAll

        //begin tests for GetCompleted
        [TestMethod]
        public void GetCompletedRetursAllCompletedItemsForUser()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId1);
            ToDoItem item2 = new ToDoItem("test2",userId1);
            ToDoItem item3 = new ToDoItem("item3", userId2);
            item1.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            repository.Add(item3);
            Assert.IsTrue(repository.GetCompleted(userId1).IndexOf(item1) == 0);
            Assert.AreEqual(repository.GetCompleted(userId1).Count, 1);
        }

        [TestMethod]
        public void GetCompletedReturnsNullWhenAllUserItemsAreActive()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId1);
            ToDoItem item2 = new ToDoItem("test2",userId2);
            item2.MarkAsDone();
            repository.Add(item2);
            repository.Add(item1);
            Assert.IsTrue(repository.GetCompleted(userId1) == null);
        }

        [TestMethod]
        public void GetCompletedReturnsNullWhenRepositoryIsEmpty()
        {
            var userId = Guid.NewGuid();
            IToDoRepository repository = new ToDoSqlRepository(_db);
            Assert.IsTrue(repository.GetCompleted(userId) == null);
        }
        //end tests for GetCompleted

        //begin tests for GetFiltered
        [TestMethod]
        public void GetFilteredReturnsUserItemsMeetingTheCondition()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId1);
            ToDoItem item2 = new ToDoItem("test2",userId1);
            ToDoItem item3 = new ToDoItem("test2",userId1);
            ToDoItem item4=new ToDoItem("test4",userId2);
            item3.DateCreated=DateTime.UtcNow;
            repository.Add(item2);
            repository.Add(item1);
            repository.Add(item3);
            repository.Add(item4);
            var testList = repository.GetFiltered(x => String.Compare(x.Text, "test2") == 0,userId1);
            Assert.IsTrue(testList.Count == 2 && testList.IndexOf(item2) == 0 && testList.IndexOf(item3) == 1);
        }

        [TestMethod]
        public void GetFilteredReturnsNullWhenNoItemMeetsTheCondition()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            ToDoItem item1 = new ToDoItem("test1",userId1);
            ToDoItem item2 = new ToDoItem("test2",userId1);
            ToDoItem item3 = new ToDoItem("test3",userId1);
            ToDoItem item4 = new ToDoItem("test4", userId2);
            repository.Add(item2);
            repository.Add(item1);
            repository.Add(item3);
            repository.Add(item4);
            Assert.AreEqual(repository.GetFiltered(x => String.Compare(x.Text, "test4") == 0,userId1), null);
        }

        [TestMethod]
        public void GetFilteredReturnsNullIfRepositoryIsEmpty()
        {
            IToDoRepository repository = new ToDoSqlRepository(_db);
            var userId = Guid.NewGuid();
            ToDoItem item3 = new ToDoItem("test3",userId);
            Assert.AreEqual(repository.GetFiltered(x => String.Compare(x.Text, "test4") == 0,userId), null);
        }
        //end tests for GetFiltered
    }
}