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
    
    public partial class EduYear
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EduYear()
        {
            this.Courses = new HashSet<Course>();
            this.Users = new HashSet<User>();
            this.Syllabus = new HashSet<Syllabu>();
        }
    
        public int YearId { get; set; }
        public string YearName { get; set; }
        public Nullable<int> SemesterId { get; set; }
        public Nullable<int> SessionId { get; set; }
        public int DepartmentId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Course> Courses { get; set; }
        public virtual Department Department { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Syllabu> Syllabus { get; set; }
    }
}