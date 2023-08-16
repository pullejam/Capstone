using DebtDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebtDAL.Data
{
    public class DbInitializer
    {
        public static void Initialize(DebtContext context)
        {
            context.Database.EnsureCreated();
            string[] lines = File.ReadAllLines("../DebtDAL/Data/CostOfLivingData.txt");

            SetupUsers(context);

            SetupStates(context);
            SetupCertifications(context);
        }

        private static void SetupCertifications(DebtContext context)
        {
            if (context.Certifications.Any())
            {
                return;
            }
            List<string> CertificationNames = new List<string>();
            CertificationNames.Add("No formal education");
            CertificationNames.Add("Primary education");
            CertificationNames.Add("Secondary education");
            CertificationNames.Add("GED");
            CertificationNames.Add("Vocational Qaulifiction");
            CertificationNames.Add("Bachelor's Degree");
            CertificationNames.Add("Master's Degree");
            CertificationNames.Add("Doctorates or higher");
            int weight = 0;
            foreach(string certificationName in CertificationNames)
            {
                weight++;
                context.Certifications.Add(new Certification(weight,certificationName));
            }
            context.SaveChanges();
            



        }

        private static void SetupUsers(DebtContext context)
        {
            if (context.AllUsers.Any())
            {
                return;
            }
            MyUser myUser = new MyUser();
            myUser.UniqueId = "uniqueYes";
            myUser.Address = "143 S Main Street";


            context.AllUsers.Add(myUser);
            context.SaveChanges();
        }
        private static void SetupStates(DebtContext context)
        {
            if(context.AllStates.Any())
            {
                return;
            }
            string[] lines = File.ReadAllLines("C:/Users/James Pulley/source/repos/MazeSolver/MazeSolver/CostOfLivingData.txt");

            foreach (string line in lines)
            {
                string[] sections = line.Split('\t');

                Console.WriteLine("State Name: " + sections[0]);
                Console.WriteLine("State Cost Of Living: " + sections[1]);
                Console.WriteLine("State Rent average: " + sections[2]);
                Console.WriteLine("Hundred Dollar Cost: " + sections[3]);

                State NewState = new State();
                NewState.Name = sections[0];
                NewState.AverageCostOfLiving = sections[1];
                NewState.AverageRentCost = sections[2];
                NewState.ValueOfHundredDollars = sections[3];
                context.AllStates.Add(NewState);
               
            }
                 context.SaveChanges();



        }
    }
}
