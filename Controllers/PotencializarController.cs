using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Utils;

namespace WebApp.Controllers
{
    public class PotencializarController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Idealizar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Prototipar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Acelerar()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Contatar(PotencializarContato model)
        {
            return Json(model.Cadastrar(), JsonRequestBehavior.AllowGet);
        }
    }
}