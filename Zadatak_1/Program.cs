using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zadatak_1;

namespace ConsoleApplication1
{
    class Program
    {
        // Preferable to keep in the configuration files.
        private const string ConnectionString =
            "Server=(localdb)\\mssqllocaldb;Database=ToDoItemDb;Trusted_Connection=True;MultipleActiveResultSets=true";

        static void Main(string[] args)
        {
            using (var db = new ToDoDbContext(ConnectionString))
            {
                var userId1 = Guid.NewGuid();
                var userId2 = Guid.NewGuid();
                var Repository = new ToDoSqlRepository(db);
                var s = new ToDoItem("stavka 1", userId1);
                Repository.Add(s);
                s = new ToDoItem("stavka 2", userId1);
                Repository.Add(s);
                s = new ToDoItem("stavka 3", userId2);
                Repository.Add(s);
                var items = Repository.GetAll(userId1);

                foreach (var entry in items)
                {
                    Console.WriteLine(entry);
                }
                Console.WriteLine("Next user: ");
                var items2 = Repository.GetFiltered(x => x.Text.Equals("stavka 3"), userId2);
                foreach (var item in items2)
                {
                    Console.WriteLine(item);
                }
            }

        }
    }
}
