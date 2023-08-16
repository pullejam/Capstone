using CapstoneDebtSolver.Models;
using DebtDAL.Data;
using DebtDAL.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DebtDAL.Models;
using DebtDAL.Implementation;

using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using RestSharp;
using System.Web;
using System.Text.RegularExpressions;
using System.Text;

namespace CapstoneDebtSolver.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        IDebtDAL dal;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;

        public HomeController(ILogger<HomeController> logger, IDebtDAL injectedDal,
            DebtContext injectedContext,
            UserManager<IdentityUser> injectedUserManager, SignInManager<IdentityUser> signInManager)
        {
            this.signInManager = signInManager;
            _logger = logger;
            dal = injectedDal;
            userManager = injectedUserManager;
            if (injectedDal.GetType() == typeof(EFDebtDAL))
            {
                ((EFDebtDAL)dal).context = injectedContext;
            }
        }

        /* [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register()
        {


            if (userManager.GetUserId(User) != null)
            {
                var UserId = userManager.GetUserId(User);
                var ExistingUser = userManager.FindByIdAsync(UserId).Result;
                MyUser user = new MyUser();
                if (ExistingUser != null)
                {
                    var AllRoles = userManager.GetRolesAsync(ExistingUser).Result.ToList();
                    if (!AllRoles.Contains("User"))
                    {

                        userManager.AddToRoleAsync(userManager.FindByIdAsync(UserId).Result, "User").Wait();

                        return View();
                    }
                }
            }
            return View();

        }*/
       public IActionResult FindRequiredPayment()
        {
            if (User.IsInRole("User"))
            {
                return View(getUser());
            }
            return View(new UserViewModel());
        }
        public IActionResult Index()
        {
            if (userManager.GetUserId(User) != null)
            {
                var UserId = userManager.GetUserId(User);
                var ExistingUser = userManager.FindByIdAsync(UserId).Result;
                MyUser user = new MyUser();
                if (ExistingUser != null)
                {
                    var AllRoles = userManager.GetRolesAsync(ExistingUser).Result.ToList();
                    if (!AllRoles.Contains("User"))
                    {

                        userManager.AddToRoleAsync(userManager.FindByIdAsync(UserId).Result, "User").Wait();
                        signInManager.SignInAsync(ExistingUser, false).Wait();
                        if (dal.GetUserByUserId(UserId) == null)
                        {
                           
                            user.UniqueId = UserId;
                            dal.AddUser(user);
                        }
                        return Redirect("../User/UserHome");
                    }


                }
            }
            return View();
        }

        public IActionResult JobSearch()
        {

            if (User.IsInRole("User"))
            {
                return View(getUser());
            }
            
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        /*public IActionResult Results()
        {
            *//*var content = CallJobApiAsync();*//*
            while (!content.IsCompleted)
            {

            }

            return View(content);*//*
            return View();
        }*/

        [HttpPost]
        public IActionResult Results(string keyword)
        {
            List<Job> jobs = new List<Job>();
            if(keyword == null)
            {
                return View("JobSearch");
            }
            else if(keyword.Trim() == "")
            {
                return View("JobSearch");
            }
            else
            {
               jobs = dal.SearchForJob(keyword);

                foreach (Job job in jobs)
                {
                    job.PositionDescription = job.PositionDescription.Replace("\\n", " ").Trim();
                    job.PositionDescription = job.PositionDescription.Replace('\"', ' ').Trim();
                    job.Link = HttpUtility.UrlDecode(job.Link).Replace('\"', ' ').Trim();
                }
            }
           


            
            return View(jobs);
        }

        [HttpPost]
        public IActionResult IdealJobByPay(int SalaryId, int HourlyId, string KeywordId)
        {
            List<Job> jobs = new List<Job>();
            if (SalaryId == 0 || HourlyId == 0 )
            {
                return View("JobSearch");
            }
            else
            {
                jobs = dal.SearchForJob(KeywordId);

                foreach (Job job in jobs)
                {
                    job.PositionDescription = job.PositionDescription.Replace("\\n", " ").Trim();
                    job.PositionDescription = job.PositionDescription.Replace('\"', ' ').Trim();
                    job.Link = HttpUtility.UrlDecode(job.Link).Replace('\"', ' ').Trim();
                }

                var SortedJobs = dal.SortJobByPaymentReq(jobs, SalaryId.ToString(), HourlyId.ToString());
                return View("Results",SortedJobs);

                
            }



           
        }

        [HttpPost]
        public IActionResult GeneratePayment(int CostOfLiving, int DebtAmount, int RepaymentTime, int ExtraCost)
        {
            List<int> xtraCosts = new List<int>();
            if (ExtraCost != 0 && CostOfLiving != null)
            {
                xtraCosts.Add(ExtraCost);
            }
            var result = dal.RequiredPayment(CostOfLiving, xtraCosts, DebtAmount, RepaymentTime);


            if (User.IsInRole("User"))
            {
                return View("PaymentResult", result);
            }
            else
            {

                return View("PaymentResult", result);
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public UserViewModel getUser()
        {
            if (User == null) return new UserViewModel();
            var user = dal.GetUserByUserId(userManager.GetUserId(User));
            UserViewModel userViewModel = new UserViewModel();
            var UserId = userManager.GetUserId(User);
            var ExistingUser = userManager.FindByIdAsync(UserId).Result;
            userViewModel.Name = userManager.GetUserName(User).Split('@')[0];
            userViewModel.Email = userManager.GetEmailAsync(ExistingUser).Result;
            userViewModel.Address = user.Address;
            userViewModel.CostOfLiving = user.CostOfLiving;
            userViewModel.DebtAmount = user.DebtAmount;
            userViewModel.ResidenceState = user.ResidenceState;
            userViewModel.SavedJobs = user.SavedJobs;
            userViewModel.Talents = user.Talents;
            return userViewModel;

        }

        /*public async Task<string> CallJobApiAsync()
        {

            *//* var client = new HttpClient();
             var request = new HttpRequestMessage
             {
                 Method = HttpMethod.Post,
                 RequestUri = new Uri("https://indeed11.p.rapidapi.com/"),
                 Headers =
             {
         { "X-RapidAPI-Key", "8ee9e5e232mshc3fc6f6b72df324p19b9a3jsne53419" },
         { "X-RapidAPI-Host", "linkedin-jobs-search.p.rapidapi.com" },
             },
                 Content = new StringContent("{\r\"search_terms\": \"Marketing\",\r\"location\": \"United States\",\r\"page\": \"5\"\r}")

                 {
                     Headers =

             {
                     ContentType = new MediaTypeHeaderValue("application/json")

             }
                 }
             };

             using var response = await client.SendAsync(request);
             response.EnsureSuccessStatusCode();

             var body = await response.Content.ReadAsStringAsync();


             Console.WriteLine(body);*//*

            var client = new RestClient("http://url.../token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "grant_type=password&username=username&password=password", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var result = response.Content;

           
        }*/


        
    }

}