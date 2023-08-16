using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtDAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DebtDAL.Data
{
    public class DebtContext : DbContext
    {
        public DebtContext(DbContextOptions<DebtContext> options) : base(options)
        {

        }
        //each debset represents a table in the database.
        public DbSet<MyUser> AllUsers { get; set; }
        public DbSet<Job> AllJobs { get; set; }
        public DbSet<Certification> Certifications { get; set; }    
       
        public DbSet<State> AllStates { get; set; }

        


    }
}
