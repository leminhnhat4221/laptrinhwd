using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace BUOI6.Model
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<Faculty> Faculties { get; set; }
        public virtual DbSet<Student> Students { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Faculty>()
                .Property(e => e.FacultyID)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.StudentID)
                .IsUnicode(false);

            modelBuilder.Entity<Student>()
                .Property(e => e.FacultyID)
                .IsUnicode(false);
        }
    }
}
