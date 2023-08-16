using DebtDAL.Data;
using DebtDAL.Interfaces;
using DebtDAL.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace DebtDAL.Implementation
{
    public class EFDebtDAL : IDebtDAL
    {

        public DebtContext context;
        public EFDebtDAL()
        {

        }
        public EFDebtDAL(DebtContext context)
        {
            this.context = context;
        }

       


        public int AddJob(Job newJob)
        {
            context.AllJobs.Add(newJob);
            context.SaveChanges();
            return 1;
        }

        public int AddUser(MyUser newUser)
        {
            context.AllUsers.Add(newUser);  
            context.SaveChanges();
            return 1;
        }

        public List<MyUser> GetAllUsers()
        {
            return context.AllUsers.ToList();
        }

        public MyUser GetUserById(int UserId)
        {
            return context.AllUsers.ToList()[UserId];
        }

        public MyUser GetUserByUserId(string UserId)
        {
            var user = context.AllUsers.FirstOrDefault(x => x.UniqueId == UserId);
            return user;    
        }

        public void RemoveUser(int UserId)
        {
            context.AllUsers.Remove(GetUserById(UserId));
            context.SaveChanges();
        }

        public List<Job> SearchForJob(string keyword)
        {
           
            return GetJobsFromAPI(keyword);
        }




        public List<Job> SearchForJobByKeyword(int CostRequirement, List<Certification> ReqOrprefEdu, List<Talent> ComplimentedTalents)
        {
            List<Job> QueriedJobs = new List<Job>();

            if(CostRequirement > 0)
            {
                foreach(Job job in context.AllJobs.ToList())
                {
                    if(int.Parse(job.Payment) >= CostRequirement)
                    {
                        QueriedJobs.Add(job);
                    }
                }
            }
            return QueriedJobs;
        }

        public int[] RequiredPayment(int CostOfLiving, List<int> extraCosts, int debtAmmount, int RepaymentTime)
        {
            int paymentAmmount = 0;
            paymentAmmount = paymentAmmount + CostOfLiving;
            int yearlyDebtPayments = debtAmmount / RepaymentTime;
            int monthlyDebtPayments = yearlyDebtPayments / 12;
            int finalPaymentPerMonth;
            if (RepaymentTime <= 10)
            {
                finalPaymentPerMonth = monthlyDebtPayments + (monthlyDebtPayments / 2);
            }
            else
            {
                finalPaymentPerMonth = monthlyDebtPayments + (monthlyDebtPayments / 4);
            }
            paymentAmmount = paymentAmmount + finalPaymentPerMonth;

            
            foreach(int payment in extraCosts)
            {
                paymentAmmount = paymentAmmount + payment;

            }
            
            int salaryReq = paymentAmmount * 12;
            int HourlyWage = (salaryReq / 52) / 40;


            int[] HourAndSalary = new int[2] {HourlyWage, salaryReq};
            
            return HourAndSalary;
        }
        
        public List<Job> SearchForIdealJobInArea(string State, string PaymentType, int PaymentAmmount, List<Certification> CertificationRequirements, List<Talent> talent)
        {
            List<Job> jobs = new List<Job>();

            

/*
            foreach (Job job in context.AllJobs.ToList())
            {
                if (job.state.Equals(State))
                {
                    jobs.Add(job);
                }
               
            }*/



            return jobs;
        }
        

        public void UpdateUser(string id, MyUser UserToUpdateTo, string Education)
        {
            
            if (UserToUpdateTo != null)
            {
                if(UserToUpdateTo.Address != null && UserToUpdateTo.Address != "")
                {
                    context.AllUsers.Where(User => User.UniqueId.Equals(id)).FirstOrDefault().Address = UserToUpdateTo.Address;
                    context.SaveChanges();
                }
                if(UserToUpdateTo.DebtAmount > 0)
                {
                    context.AllUsers.Where(User => User.UniqueId.Equals(id)).FirstOrDefault().DebtAmount = UserToUpdateTo.DebtAmount;
                    context.SaveChanges();
                }
                if (UserToUpdateTo.CostOfLiving > 0)
                {
                    context.AllUsers.Where(User => User.UniqueId.Equals(id)).FirstOrDefault().CostOfLiving = UserToUpdateTo.CostOfLiving;
                    context.SaveChanges();
                }
                if (UserToUpdateTo.ResidenceState != null)
                {

                    context.AllUsers.Where(User => User.UniqueId.Equals(id)).FirstOrDefault().ResidenceState = UserToUpdateTo.ResidenceState;
                    context.SaveChanges();
                }
                if(Education != null )
                {

                    if (!context.AllUsers.Where(User => User.UniqueId.Equals(id)).FirstOrDefault().Certifications.Contains(context.Certifications.Where(certification => certification.CertificationName.Equals(Education)).FirstOrDefault())){
                        context.AllUsers.Where(User => User.UniqueId.Equals(id)).FirstOrDefault().Certifications.Add(context.Certifications.Where(certification => certification.CertificationName.Equals(Education)).FirstOrDefault());
                        context.SaveChanges();
                    }
                }
                context.SaveChanges();
            }
        }

        public State? GetState(string requestedState)
        {
            var state = context.AllStates.Where(state=> state.Name.Equals(requestedState)).FirstOrDefault();  
            return state;
        }



        public string StateFinder(string stateCode)
        {
            switch (stateCode)
            {
                case "AL":
                    return "Alabama";

                case "AK":
                    return "Alaska";

                case "AS":
                    return "American Samoa";

                case "AZ":
                    return "Arizona";

                case "AR":
                    return "Arkansas";

                case "CA":
                    return "California";

                case "CO":
                    return "Colorado";

                case "CT":
                    return "Connecticut";

                case "DE":
                    return "Delaware";

                case "DC":
                    return "District Of Columbia";

                case "FM":
                    return "Federated States Of Micronesia";

                case "FL":
                    return "Florida";

                case "GA":
                    return "Georgia";

                case "GU":
                    return "Guam";

                case "HI":
                    return "Hawaii";

                case "ID":
                    return "Idaho";

                case "IL":
                    return "Illinois";

                case "IN":
                    return "Indiana";

                case "IA":
                    return "Iowa";

                case "KS":
                    return "Kansas";

                case "KY":
                    return "Kentucky";

                case "LA":
                    return "Louisiana";

                case "ME":
                    return "Maine";

                case "MH":
                    return "Marshall Islands";

                case "MD":
                    return "Maryland";

                case "MA":
                    return "Massachusetts";

                case "MI":
                    return "Michigan";

                case "MN":
                    return "Minnesota";

                case "MS":
                    return "Mississippi";

                case "MO":
                    return "Missouri";

                case "MT":
                    return "Montana";

                case "NE":
                    return "Nebraska";

                case "NV":
                    return "Nevada";

                case "NH":
                    return "New Hampshire";

                case "NJ":
                    return "New Jersey";

                case "NM":
                    return "New Mexico";

                case "NY":
                    return "New York";

                case "NC":
                    return "North Carolina";

                case "ND":
                    return "North Dakota";

                case "MP":
                    return "Northern Mariana Islands";

                case "OH":
                    return "Ohio";

                case "OK":
                    return "Oklahoma";

                case "OR":
                    return "Oregon";

                case "PW":
                    return "Palau";

                case "PA":
                    return "Pennsylvania";

                case "PR":
                    return "Puerto Rico";

                case "RI":
                    return "Rhode Island";

                case "SC":
                    return "South Carolina";

                case "SD":
                    return "South Dakota";

                case "TN":
                    return "Tennessee";

                case "TX":
                    return "Texas";

                case "UT":
                    return "Utah";

                case "VT":
                    return "Vermont";

                case "VI":
                    return "Virgin Islands";

                case "VA":
                    return "Virginia";

                case "WA":
                    return "Washington";

                case "WV":
                    return "West Virginia";

                case "WI":
                    return "Wisconsin";

                case "WY":
                    return "Wyoming";
                default:
                    return "State Couldnt be found"; 

            }
        }

        public void UpdateTalents(string userId, List<string> talents)
        {
            var user = context.AllUsers.Where(user => user.UniqueId.Equals(userId)).FirstOrDefault();
            List<Talent> NewTalents = new List<Talent>();
            foreach (var talent in talents)
            {
                NewTalents.Add(new Talent() { TalentName = talent });
            }
            if (!user.Talents.Equals(NewTalents))
            {
                user.Talents = NewTalents;
                context.SaveChanges();  
            }
            

            
            

            
        }

        public void updatePhoto(string userId, string fileName)
        {
            var user = context.AllUsers.Where(user => user.UniqueId.Equals(userId)).FirstOrDefault();
            user.UserPic = fileName;
            context.SaveChanges();
        }

        public static async Task<string> CallAPI(string searchTerm)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri("https://indeed11.p.rapidapi.com/"),
                Headers =
    {
        { "X-RapidAPI-Key", "8ee9e5e232mshc3fc6f6b72df324p19b9a3jsne53419576344" },
        { "X-RapidAPI-Host", "indeed11.p.rapidapi.com" },
    },
                Content = new StringContent("{\r\"search_terms\": \"" + searchTerm + "\",\r\"location\": \"United States\",\r\"page\": \"1\"\r}")

                {
                    Headers =

        {
                    ContentType = new MediaTypeHeaderValue("application/json")

        }
                }
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                
                    return body;
               
               

            }
           




        }

        public static List<List<string[]>> ConvertData(string apiJsonResponseString)
        {
            List<List<string[]>> Valores = new();
            //totalPagesQuantity = int.Parse(httpResponse.Headers.GetValues("pagesQuantity").FirstOrDefault());
            //Aqui tenho que colocar um try para o caso de ser retornado um objecto vazio


            JArray array = JArray.Parse(apiJsonResponseString);

            foreach (JObject objx in array.Children<JObject>())
            {
                List<string[]> ls = new();
                foreach (JProperty singleProp in objx.Properties())
                {
                    if (!singleProp.Name.Contains("_xyz"))
                    {
                        string[] val = new string[2];
                        val[0] = singleProp.Name;
                        val[1] = singleProp.Value.ToString();
                        ls.Add(val);
                    }
                }
                Valores.Add(ls);
            }

            return Valores;
        }
        public static List<Job> GetJobsFromAPI(string searchTerm)
        {

            var text = File.ReadAllText("C:/Users/James Pulley/source/repos/CapstoneDebtSolver/DebtDAL/Data/JobData.txt");


            List<Job> jobs = new List<Job>();
            int JobCounter = 0;

            var result = ConvertData(text);
            foreach (var Job in result)
            {
                JobCounter++;
                Job newJob = new Job();
                newJob.JobId = JobCounter;
                newJob.PostDate = HttpUtility.HtmlDecode(Job[0][1]);
                newJob.PositionName = HttpUtility.HtmlDecode(Job[1][1]);
                newJob.Name = HttpUtility.HtmlDecode(Job[2][1]);
                newJob.state = HttpUtility.HtmlDecode(Job[3][1]);
                newJob.Payment = HttpUtility.HtmlDecode(Job[4][1]);
                if (newJob.Payment.Contains("year"))
                {
                    newJob.PaymentType = "Salary";

                }
                else
                {
                    if (newJob.Payment.Trim() != "" && newJob.Payment.Trim() != "\"\"")
                    {
                        newJob.PaymentType = "Hourly";
                    }
                    else
                    {
                        newJob.Payment = "N/A";
                        newJob.PaymentType = "N/A";
                    }

                }

                newJob.Link = Job[5][1];

                newJob.PositionDescription = HttpUtility.HtmlDecode(Job[6][1]);

                jobs.Add(newJob);
                newJob = new Job();
            }
            return jobs;
        }



        public List<Job> SortJobByPaymentReq(List<Job> jobs, string SalaryReq, string HourReq)
        {
            List<Job> sortedJobs = new List<Job>();

            int jobCounter = 0;

            foreach (Job job in jobs)
            {
                string DefPayment = "";

                if (job.PaymentType != null)
                {
                    if (job.Payment != "N/A")
                    {


                        if (job.PaymentType.Trim().Equals("Salary"))
                        {
                            Regex regex = new Regex(@"\$([0-9,.]+k?)");
                            Match match = regex.Match(job.Payment);

                            Console.WriteLine("Job " + jobCounter + ": Group 1: " + match.Groups[1]);

                            if (match.Groups[1].Value.Contains('.') || match.Groups[1].Value.Length < 5)
                            {
                                string s = match.Groups[1].Value.Replace('k', ' ');
                                double result = double.Parse(s.Trim());
                                result = result * 1000;
                                if (double.Parse(SalaryReq) < result)
                                {
                                    if (!sortedJobs.Contains(job))
                                    {
                                        sortedJobs.Add(job);
                                    }
                                }
                                //job.Payment = result.ToString();
                            }
                            else
                            {
                                DefPayment = match.Groups[1].Value;
                                if (!job.Payment.Contains(','))
                                {
                                    if (double.Parse(SalaryReq) < double.Parse(match.Groups[1].Value))
                                    {
                                        if (!sortedJobs.Contains(job))
                                        {
                                            sortedJobs.Add(job);
                                        }

                                    }
                                }

                            }
                            if (job.Payment.Contains(','))
                            {
                                string pay = RemoveComma(DefPayment);
                                if (double.Parse(SalaryReq) < double.Parse(pay))
                                {
                                    if (!sortedJobs.Contains(job))
                                    {
                                        sortedJobs.Add(job);
                                    }
                                }

                            }

                        }
                        else if (job.PaymentType.Trim().Equals("Hourly"))
                        {
                            Regex regex = new Regex(@"\$([0-9,.]+k?)");
                            Match match = regex.Match(job.Payment);
                            Console.WriteLine("Job " + jobCounter + ": Group 1: " + match.Groups[1]);

                            if (double.Parse(match.Groups[1].Value) >= double.Parse(HourReq))
                            {
                                sortedJobs.Add(job);
                            }
                        }

                    }
                }
            }
            return sortedJobs;
        }


        public string RemoveComma(string Payment)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in Payment)
            {
                if (!c.Equals(','))
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString();
        }
    }

}
 