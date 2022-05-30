using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// referencia
//https://lacerda53.medium.com/requisi%C3%A7%C3%B5es-ajax-em-asp-net-core-mvc-cd985b1186d5

namespace WebApp.Controllers
{
    public class CheckoutController : Controller
    {
        // GET: Checkout
        public ActionResult Plano()
        {
            return View();
        }

    }
}