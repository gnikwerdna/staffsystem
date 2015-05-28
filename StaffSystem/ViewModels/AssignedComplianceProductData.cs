using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StaffSystem.ViewModels
{
    public class AssignedComplianceProductData
    {
        public int ComplianceID { get; set; }
        public string Title { get; set; }
        public bool Assigned { get; set; }
        public int level { get; set; }
        public int grp { get; set; }
        public int order { get; set; }
    }
}