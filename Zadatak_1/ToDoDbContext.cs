using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class ToDoDbContext : System.Data.Entity.DbContext
    {
        public IDbSet<ToDoItem> Items { get; set; }

        public ToDoDbContext(string connectionString) : base(connectionString)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ToDoItem>().HasKey(x => x.Id);
            modelBuilder.Entity<ToDoItem>().Property(s => s.Text).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(s => s.IsCompleted).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(s => s.DateCompleted);
            modelBuilder.Entity<ToDoItem>().Property(x => x.DateCreated).IsRequired();
            modelBuilder.Entity<ToDoItem>().Property(x => x.UserId).IsRequired();
        }

    }
}
