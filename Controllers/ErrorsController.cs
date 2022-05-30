using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Rs403()
        {
            ViewBag.TitlePage = "ERROR 403";
            return View();
        }

        public ActionResult Rs404()
        {
            ViewBag.TitlePage = "ERROR 404";
            return View();
        }

        public ActionResult Rs500()
        {
            ViewBag.TitlePage = "ERROR 500";
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}