using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ApiController : Controller
    {
        [HttpGet]
        public int GetNumero(string email)
        {
            Random randNum = new Random();
            if (email.ToUpper() == "GABRIELLEITEMOREIRA@GMAIL.COM")
            {
                return 10;
            }
            else if (email.ToUpper() == "GABRIEL.MOREIRA@CIAHERING.COM.BR")
            {
                return 20;
            }
            else
            {
                return randNum.Next(100, 200);
            }
        }
    }
}