using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtDAL.Models
{
    public class Job
    {
        public int JobId { get; set; }

        public string Name { get; set; }

        public string PostDate { get; set; }

        public string Link { get; set; }

        public string PositionName { get; set; }

        public string? PositionDescription { get; set; }

        public string? PaymentType { get; set; }

        public string? Payment { get; set; }

        public string Schedule { get; set; }

        public string? state { get; set; }
        public virtual List<Certification> Certifications { get; set; }
        public virtual List<Talent> Talents { get; set; }

        public Job()
        {
            PositionDescription = "";
            Certifications = new List<Certification>();
            Talents = new List<Talent>();
            PositionName = String.Empty;
        }


    }
}
