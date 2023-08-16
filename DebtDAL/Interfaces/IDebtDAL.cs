using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DebtDAL.Models;

namespace DebtDAL.Interfaces
{
    public interface IDebtDAL
    {
        List<MyUser> GetAllUsers();
        MyUser GetUserById(int UserId);
        MyUser GetUserByUserId(string UserId);
        List<Job> SearchForJob(string keyword);
        int AddUser(MyUser newUser);
        int AddJob(Job newJob);
        void UpdateUser(string id, MyUser UserToUpdate, string education);
        void RemoveUser(int UserId);
        public int[] RequiredPayment(int CostOfLiving, List<int> extraCosts, int debtAmmount, int RepaymentTime);
        public List<Job> SortJobByPaymentReq(List<Job> jobs, string SalaryReq, string HourReq);
        State? GetState(string state);
        public string StateFinder(string stateCode);
        void UpdateTalents(string userId, List<string> talents);
        void updatePhoto(string userId, string fileName);
        public string RemoveComma(string Payment);
        //Anime AddPost(int id, Post post);
        //int getTotal();

    }
}




