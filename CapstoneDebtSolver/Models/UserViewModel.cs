
using DebtDAL.Models;

namespace CapstoneDebtSolver.Models
{
    public class UserViewModel
    {
        public string Name { get; set; }
        public string? Address { get; set; }
        public string Email { get; set; }
        public int? DebtAmount { get; set; }
        public int? CostOfLiving { get; set; }
        public string UserPic { get; set; }
        public virtual State? ResidenceState { get; set; }
        public virtual ICollection<Job> SavedJobs { get; set; }
        public virtual ICollection<Talent> Talents { get; set; }

        public virtual ICollection<Certification> Certifications { get; set; }

        public UserViewModel()
        {
            UserPic = "";
            SavedJobs = new List<Job>();
            Talents = new List<Talent>();
            Certifications = new List<Certification>();
            Name = string.Empty;
            Email = string.Empty;
            DebtAmount = 0;
            CostOfLiving = 0;
        }
    }
}
