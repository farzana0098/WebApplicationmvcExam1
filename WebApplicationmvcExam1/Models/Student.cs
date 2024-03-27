using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationmvcExam1.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }


        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [StringLength(250)]
        public string Address { get; set; }

        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }




        public int Class { get; set; }


        public bool Regular { get; set; }

        [ScaffoldColumn(false)]
        [DataType(DataType.ImageUrl)]

        public string ImageUrl { get; set; }



        [NotMapped]
        [DataType(DataType.Upload)]
        [ScaffoldColumn(true)]

        public HttpPostedFileBase ImageUpload { get; set; }


        public virtual IList<StudentMark> StudentMarks { get; set; }

        public Student()
        {
        StudentMarks= new List<StudentMark>();
        
        }
    }
}