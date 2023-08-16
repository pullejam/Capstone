using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtDAL.Models
{
    public class State
    {  
        [Key]
        public int Id { get; set; } 
        public string? Name { get; set; }
        public string? AverageCostOfLiving { get; set; }
        public string AverageRentCost { get; set; }
        public string ValueOfHundredDollars { get; set; }
        public virtual List<MyUser> Residents { get; set; }
        public State()
        {
                this.Residents = new List<MyUser>();
            ValueOfHundredDollars = String.Empty;
            AverageRentCost = String.Empty;

        }
    }
}
