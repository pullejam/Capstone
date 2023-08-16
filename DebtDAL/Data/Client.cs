using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebService.Careerjet;
/*using Namespace = "WebService.Careerjet";*/
using Newtonsoft.Json;
using System.Collections;

namespace DebtDAL.Data
{
   
    public class Client : WebService.Careerjet.Client
    {
        public Client(string locale) : base(locale)
        {

        }

        /*  public Client(string locale)
          {

          }*/
    }
}
