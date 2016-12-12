using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    public class ToDoDbContext : System.Data.Entity.DbContext
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

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
