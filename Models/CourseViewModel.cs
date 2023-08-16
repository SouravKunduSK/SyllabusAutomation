using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyllabusAutomation.Models
{
    public class CourseViewModel
    {
        public int YearId { get; set; }
        public int SemesterId { get; set; }
        public int CourseId { get; set; }

        public int MarksId { get; set; }

        public string CourseCode { get; set; }
    }
}