using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtDAL.Models
{
    public class Certification
    {
        [Key]
        public int CertificationId { get; set; }
        public int Weight { get; set; }
        public string CertificationName { get; set;}
        public Certification()
        {
            CertificationName = string.Empty;
        }
        public Certification(int weight, string certificationName)
        {
            Weight = weight;
            CertificationName = certificationName;
           
        }
    }
}
