//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PatientHelpCare.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class Qualification
    {
        public int Id { get; set; }
        public string Country { get; set; }
        public string Degree { get; set; }
        public string University { get; set; }
        public int StartYear { get; set; }
        public int EndYear { get; set; }
        public string DoctorId { get; set; }
    
        public virtual Doctor Doctor { get; set; }
    }
}
