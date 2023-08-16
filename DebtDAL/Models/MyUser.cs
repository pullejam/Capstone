using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace DebtDAL.Models
{
    public class MyUser
    {
        [Key]
        public int UserId { get; set; }
        public string? UniqueId { get; set; }
        public string UserPic { get; set; }
        public string? Address { get; set; } 
        public int? DebtAmount { get; set; } 
        public int? CostOfLiving { get; set; }
        public virtual State? ResidenceState { get; set; }
        public virtual ICollection<Job> SavedJobs { get; set; }
        public virtual ICollection<Talent> Talents { get; set; }
        
        public virtual ICollection<Certification> Certifications { get; set; }

        public MyUser()
        {
            UserPic = "";
            SavedJobs = new List<Job>();
            Talents = new List<Talent>();   
            Certifications = new List<Certification>();
            Address = string.Empty;
            DebtAmount = 0;
            CostOfLiving = 0;
            UserId = 0; 
    
        }

    }
}
