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
            AddDemo();
            UpdateDemo();
            DeleteDemo();
            ReadDemo();
        }

        private static void ReadDemo()
        {
            using (var db = new ToDoDbContext(ConnectionString))
            {
                List<Student> overoccupiedStudents = db.Students.Where(s => s.ClassesAttending.Count > 10).ToList();
            }
        }


        private static void DeleteDemo()
        {
            using (var db = new UniversityDbContext(ConnectionString))
            {
                var student = db.Students.FirstOrDefault();
                if (student != null)
                {
                    db.Students.Remove(student);
                }
            }
        }

        private static void UpdateDemo()
        {
            using (var db = new UniversityDbContext(ConnectionString))
            {
                var student = db.Students.FirstOrDefault();
                if (student != null)
                {
                    student.Name = "Changed :)";
                    db.SaveChanges();
                }
            }
        }

        private static void AddDemo()
        {
            using (var db = new UniversityDbContext(ConnectionString))
            {
                var s = new Student()
                {
                    Jmbag = "1234",
                    Name = "Pero"
                };
                var p = new Professor()
                {
                    Name = "Marko"
                };

                s.Mentor = p;

                db.Students.Add(s);
                db.Professors.Add(p);

                db.SaveChanges();

            }
        }
    }
}
