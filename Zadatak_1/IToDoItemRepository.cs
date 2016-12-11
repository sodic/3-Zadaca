using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    public interface IToDoRepository
    {
        /// <summary>
        /// Gets ToDoItem for a given IDd. Throw TodoAccessDeniedException
        ///with appropriate message if user is not the owner of the Todo item
        /// </summary>
        /// <param name="toDoId">The requested item's ID</param>
        /// <param name ="userId" >Id of the user that is trying to fetch the data</param>
        /// <returns>ToDoItem if found, null otherwise</returns>
        ToDoItem Get(Guid toDoId, Guid userId);

        /// <summary>
        /// Adds new ToDoItem object to the database.
        /// If the object with the same ID already exists,
        /// the method should throw DuplicateToDoItemException 
        /// with the message "duplicate ID: {ID}"
        /// </summary>
        /// <param name="toDoItem"></param>
        void Add(ToDoItem toDoItem);

        /// <summary >
        /// Tries to remove a ToDoItem with given ID from the database. Throw TodoAccessDeniedException
        ///with appropriate message if user is not the owner of the Todo item
        /// </summary>
        /// <param name="toDoId">The item's ID</param>
        /// <param name ="userId" >Id of the user that is trying to remove the data</param>
        /// <returns > True if success , false otherwise </returns >
        bool Remove(Guid toDoId, Guid userId);

        /// <summary >
        /// Updates given ToDoItem in database. Throw TodoAccessDeniedException
        ///with appropriate message if user is not the owner of the Todo item
        /// If ToDoItem does not exist , method will add one.
        /// </summary>
        /// <param name="toDoItem">The item to update</param>
        /// <param name ="userId" >Id of the user that is trying to update the data</param>
        void Update(ToDoItem toDoItem, Guid userId);

        /// <summary >
        /// Tries to mark a ToDoItem as completed in database. Throw TodoAccessDeniedException
        ///with appropriate message if user is not the owner of the Todo item
        /// </summary>
        /// <param name="toDoId">The item's ID</param>
        /// <param name ="userId" >Id of the user that is trying to edit the data</param>
        /// <returns > True if success , false otherwise </returns >
        bool MarkAsCompleted(Guid toDoId, Guid userId);

        /// <summary >
        /// Gets all ToDoItem objects in database for the given user, sorted by date created
        /// (descending)
        /// </summary>
        /// <param name ="userId" >Id of the user that is trying to fetch the data</param>
        List<ToDoItem> GetAll(Guid userId);

        /// <summary >
        /// Gets all incomplete ToDoItem objects in database for the given user
        /// </summary>
        List<ToDoItem> GetActive(Guid userId);

        /// <summary >
        /// Gets all completed ToDoItem objects in database for the given user
        /// </summary>
        List<ToDoItem> GetCompleted(Guid userId);

        /// <summary >
        /// Gets all ToDoItem objects in database for the given user that apply to the filter 
        /// </summary>
        List<ToDoItem> GetFiltered(Func<ToDoItem, bool> filterFunction, Guid userId);
    }
}
