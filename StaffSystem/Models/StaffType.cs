using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StaffSystem.Models
{
    public class StaffType
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Decscription")]
        public string TypeDesc { get; set; }
    }
}