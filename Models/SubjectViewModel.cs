using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SyllabusAutomation.Models
{
    public class SubjectViewModel
    {
        public int SelectedSubjectId { get; set; }
        public List<Course> Subjects { get; set; }
        public List<CLOToPLO> ClotoploMappings { get; set; }
        public bool IsEditMode { get; set; }
    }
}