using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyllabusAutomation.Models
{
    public class SyllabusViewModel
    {
        public int Id { get; set; }
        public int ProgramId { get; set; }
        public int SessionId { get; set; }
        public int YearLength { get; set; }
        public int SemesterPerYear { get; set; }
        public int CourseId { get; set; }
        public string CourseCode { get; set; }
    }
}