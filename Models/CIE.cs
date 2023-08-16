//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SyllabusAutomation.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class CIE
    {
        public int CIEId { get; set; }
        public string Category { get; set; }
        public string Mark { get; set; }
        public string Test { get; set; }
        public string Assignment { get; set; }
        public string Quizzes { get; set; }
        public string CCA { get; set; }
        public Nullable<int> CourseId { get; set; }
        public Nullable<int> ProgramId { get; set; }
        public Nullable<int> DepartmentId { get; set; }
        public Nullable<int> FacultyId { get; set; }
        public Nullable<int> UniversityId { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual Department Department { get; set; }
        public virtual Faculty Faculty { get; set; }
        public virtual Program Program { get; set; }
        public virtual University University { get; set; }
    }
}
