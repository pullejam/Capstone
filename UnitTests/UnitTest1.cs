using NUnit.Framework;
using DebtDAL.Models;
using DebtDAL.Interfaces;
using DebtDAL.Implementation;
using DebtDAL.Data;
using System.Collections.Generic;

namespace UnitTests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        [Test]
        public void PaymentTest()
        {
            IDebtDAL myDal = new EFDebtDAL();
            int[] HourAndSalary =  myDal.RequiredPayment(2000, new List<int>(), 50000, 10);
            int Hourly = HourAndSalary[0];
            int salary = HourAndSalary[1];  
            if(Hourly == 15 && salary == 31488)
            {
                Assert.Pass();
            }


        }
        [Test]
        public void NegatedPaymentTest()
        {
            IDebtDAL myDal = new EFDebtDAL();
            int[] HourAndSalary = myDal.RequiredPayment(2000, new List<int>(), 50000, 10);
            int Hourly = HourAndSalary[0];
            int salary = HourAndSalary[1];
            if (Hourly == 14 && salary == 31508)
            {
                Assert.Fail();
            }
            else
            {
                Assert.Pass();
            }


        }

    }
}