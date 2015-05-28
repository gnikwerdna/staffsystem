using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffSystem.Models
{
    public class ComplianceItems
    {
        [Key]
        public int ComplianceID { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Title { get; set; }
        [Display(Name = "Status")]
        public int TypeID { get; set; }
        [Display(Name = "Sub Item")]
        public int SubID { get; set; }
        public int level { get; set; }
        public int grp { get; set; }
        public int order { get; set; }
        public string description { get; set; }
        public virtual ICollection<Staff> Staff { get; set; }
        //public virtual ICollection<ComplianceTypes> ComplianceTypes { get; set; }
    }
}