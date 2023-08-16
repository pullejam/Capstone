using CapstoneDebtSolver.Models;
using DebtDAL.Interfaces;
using DebtDAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using RestSharp;

namespace CapstoneDebtSolver.Controllers
{

    [Authorize(Roles = "User")]
    public class UserController : Controller
    {
        UserManager<IdentityUser> userManager;
        public IDebtDAL dal;
       

        public UserController(IDebtDAL injectedDal,
            UserManager<IdentityUser> injectedUserManager)
        {
            this.dal = injectedDal;
            this.userManager = injectedUserManager;

        }

        

        public IActionResult UserHome()
        {
           
            var user = dal.GetUserByUserId(userManager.GetUserId(User));

            if (user == null)
            {
                Redirect("/Home/Index");
            } 
            var userViewModel = getUser();
            return View(userViewModel);
        }

        public IActionResult UserInfo()
        {
            var userViewModel = getUser();

            if(userViewModel.Address.Trim() == "")
            {
                userViewModel.Address = "N/A";
            }
            
            return View(userViewModel);
        }

        public IActionResult Update()
        {
            var userViewModel = getUser();

            return View(userViewModel);
        }
        
        public IActionResult PracticeFile()
        {
            
            
            
            /*
            var client = new RestClient("https://cities-cost-of-living1.p.rapidapi.com/get_cities_list");
            var request = new RestRequest("Get");
            request.AddHeader("X-RapidAPI-Key", "8ee9e5e232mshc3fc6f6b72df324p19b9a3jsne53419576344");
            request.AddHeader("X-RapidAPI-Host", "cities-cost-of-living1.p.rapidapi.com");
            RestResponse response = client.Execute(request);

            var allCities = response.ToString();
*/
            return View();
        }

      

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateAsync(int CostOfLiving, int DebtAmount, string address, string State, string email, string education, List<string> talents, IFormFile file)
        {
           
            MyUser newUpdate = new MyUser();
            
            var existingUser = userManager.FindByEmailAsync(email).Result;
            if(existingUser == null)
            {
                return View();
            }
            if (DebtAmount > 0)
            {
               newUpdate.DebtAmount = DebtAmount;
            }
            if(CostOfLiving > 0)
            {
                newUpdate.CostOfLiving = CostOfLiving;
            }
            if(address != null)
            {
                newUpdate.Address = address;
            }
            if(State != null)
            {
                string stateName = dal.StateFinder(State);
                var state = dal.GetState(stateName);
                newUpdate.ResidenceState = state;
               
            }



            var UserId = userManager.GetUserId(User);

            dal.UpdateUser(UserId, newUpdate, education);

            dal.UpdateTalents(UserId, talents);

            if(file != null)
            {
                string fil = Path.GetFileName(file.FileName);
                
                /*string baseUrl = Request.PathBase.ToString() + "/";
                string path = Path.Combine(("./AllImages"), fil);*/

                using (Stream fileStream = new FileStream("./wwwroot/AllImages/"+ fil, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                dal.updatePhoto(UserId, file.FileName);
                
                return View("UserHome", getUser());
            }
            else
            {
                return View("UserHome", getUser());
            }

            
        }
       /* [HttpPost]
        public IActionResult GeneratePayment(int CostOfLiving, int DebtAmount, int RepaymentTime, int ExtraCosts)
        {
            List<int> xtraCosts = new List<int>();
            if(ExtraCosts != 0 && CostOfLiving != null)
            {
                xtraCosts.Add(ExtraCosts);
            }
            var result = dal.RequiredPayment(CostOfLiving, xtraCosts, RepaymentTime, ExtraCosts);

            
            if (User.IsInRole("User"))
            {
                return View("PaymentResult", result);
            }
            else
            {
                
                return View("PaymentResult", result);
            }
           
        }*/
        
        public UserViewModel getUser()
        {
            var user = dal.GetUserByUserId(userManager.GetUserId(User));
            UserViewModel userViewModel = new UserViewModel();
            var UserId = userManager.GetUserId(User);
            var ExistingUser = userManager.FindByIdAsync(UserId).Result;
            userViewModel.Name = userManager.GetUserName(User).Split('@')[0];
            userViewModel.Email = userManager.GetEmailAsync(ExistingUser).Result;
            userViewModel.Address = user.Address;
            userViewModel.UserPic = user.UserPic;
            userViewModel.CostOfLiving = user.CostOfLiving;
            userViewModel.DebtAmount = user.DebtAmount;
            userViewModel.ResidenceState = user.ResidenceState;
            userViewModel.SavedJobs = user.SavedJobs;
            userViewModel.Talents = user.Talents;
            return userViewModel;

        }
    }
}
