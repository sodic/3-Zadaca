using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    public class ToDoSqlRepository : IToDoRepository
    {
        private readonly ToDoDbContext _context;

        public ToDoSqlRepository(ToDoDbContext context)
        {
            _context = context;
        }

        public ToDoItem Get(Guid toDoId, Guid userId)
        {
            var item = _context.Items.FirstOrDefault(x => x.Id == toDoId);
            if (item != null)
                CheckUser(item, userId);
            return item;

        }

        public void Add(ToDoItem toDoItem)
        {
            if (toDoItem == null)
                throw new ArgumentNullException();
            if (_context.Items.Count(x => x.Id == toDoItem.Id) != 0)
                throw new DuplicateTodoItemException($"duplicate id: {toDoItem.Id}");
            _context.Items.Add(toDoItem);
            _context.SaveChanges();
        }

        public bool Remove(Guid toDoId, Guid userId)
        {
            var item = Get(toDoId, userId);
            if (item == null)
                return false;
            _context.Items.Remove(item);
            _context.SaveChanges();
            return true;
        }

        public void Update(ToDoItem toDoItem, Guid userId)
        {
            Remove(toDoItem.Id, userId);
            _context.Items.Add(toDoItem);
            _context.SaveChanges();
        }

        public bool MarkAsCompleted(Guid toDoId, Guid userId)
        {
            var item = Get(toDoId, userId);
            if (item == null)
                return false;
            if (!item.MarkAsDone())
                return false;
            Remove(toDoId, userId);
            Add(item);
            _context.SaveChanges();
            return true;
        }

        public List<ToDoItem> GetAll(Guid userId)
        {
            return GetFiltered(x => true, userId);
        }

        public List<ToDoItem> GetActive(Guid userId)
        {
            return GetFiltered(x => !x.IsCompleted, userId);
        }

        public List<ToDoItem> GetCompleted(Guid userId)
        {
            return GetFiltered(x => x.IsCompleted, userId);
        }

        public List<ToDoItem> GetFiltered(Func<ToDoItem, bool> filterFunction, Guid userId)
        {
            var returnList = _context.Items.Where(filterFunction).Where(x => x.UserId.Equals(userId)).OrderByDescending(x => x.DateCreated).ToList();
            return returnList.Count == 0 ? null : returnList;
        }

        private void CheckUser(ToDoItem item, Guid userId)
        {
            if (!item.UserId.Equals(userId))
                throw new ToDoAccesDeniedException($"Item {item.Id} does not belong to the user {userId}");
        }

    }
}
