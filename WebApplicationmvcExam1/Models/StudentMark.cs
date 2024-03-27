using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationmvcExam1.Models
{
    public class StudentMark
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int ID { get; set; }
        public string SubjectName { get; set; }
        public int TotalNumber { get; set; }

        public int ObtainedNumber { get; set; }
        [Column(TypeName = "datetime2")]

        [DataType(DataType.Date)]
        [DisplayName("Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime StartDate { get; set; }



        [Column(TypeName = "datetime2")]
        [DataType(DataType.Date)]
        [DisplayName("End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public int StudentID { get; set; }


        public virtual Student Student { get; set; }

    }
}