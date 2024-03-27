using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplicationmvcExam1.Models
{
    public class StudentContext:DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentMark> StudentMarks { get; set; }
        public StudentContext() : base("myStudentdb")
        {

        }

    }
}