using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreTutorial.EntityClasses
{
    public class Grade
    {
        public int Id { get; set; }

        public float CourseGrade { get; set; }
        
        public Student Student { get; set; }

        public Course Course { get; set; }

    }

}
