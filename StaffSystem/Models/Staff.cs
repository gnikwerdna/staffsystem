using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffSystem.Models
{
    public class Staff
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Status")]
        public int TypeId { get; set; }
        public virtual ICollection<ComplianceItems> ComplianceItems { get; set; }

    }
}