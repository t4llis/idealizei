using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Utils;

namespace WebApp.Controllers
{
    public class BaseController : Controller
    {
        public string[] _cores = { "#20B2AA", "#4682B4", "#A0522D", "#3CB371", "#CD853F", "#1E90FF", "#9932CC", "#F08080", "#8B008B", "#B0E0E6", "#E6E6FA",
                           "#20B2AA", "#4682B4", "#1E90FF", "#3CB371", "#CD853F", "#A0522D", "#8B008B", "#9932CC", "#F08080", "#B0E0E6", "#E6E6FA",
                           "#20B2AA", "#4682B4", "#1E90FF", "#3CB371", "#F08080", "#CD853F", "#A0522D", "#8B008B", "#9932CC", "#B0E0E6", "#E6E6FA"};

        public void StartController(string controller, string metodo)
        {
            RegistraLog.Log(String.Format($"{"Log criado em "} : {DateTime.Now}"), controller.Trim()+"_"+metodo.Trim());

        }

        public void EndController(string controller, string metodo)
        {
            RegistraLog.Log(String.Format($"{"Log finalizado em "} : {DateTime.Now}"), controller.Trim() + "_" + metodo.Trim());
        }

        public bool Autorized()
        {
            HttpCookie cookie2 = Request.Cookies["Authorize"];
            string token = cookie2 != null ? cookie2.Value.ToString() : "";
            if ((HttpContext.Session["UserIsAuthenticated"] != null) && (token != null) && (TokenService.TokenOk(token)))
                return true;    
            else
                return false;
            
        }
    }
}