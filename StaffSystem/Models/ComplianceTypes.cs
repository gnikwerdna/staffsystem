using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffSystem.Models
{
    public class ComplianceTypes
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<ComplianceItems> ComplianceItems { get; set; }
    }
}