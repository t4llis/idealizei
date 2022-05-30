using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;
using WebApp.Utils;

namespace WebApp.Controllers
{
    public class LoginController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (Autorized())
                return RedirectToAction("Index", "Home");
            else
            {
                if (HttpContext.Session["IdIdeiaCocriador"] == null && HttpContext.Session["IdIdeiaAvaliador"] == null)
                    LimparSession();
                return View();
            }
        }

        [HttpPost]
        public ActionResult ValidarLoginPadrao(User user)
        {
            var newuser = user.GetUsuario(user.NOME, user.SENHA);
            if (newuser.ID == 0)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                HttpCookie cookie = new HttpCookie("Authorize");
                DateTime dtNow = DateTime.Now;
                TimeSpan tsMinute = new TimeSpan(0, 0, 1440, 0);
                cookie.Expires = dtNow + tsMinute;
                cookie.Value = TokenService.GenerateToken(user);
                Response.Cookies.Add(cookie);

                CriarSessionUser(newuser);
                ViewBag.User = newuser;
                
                if((HttpContext.Session["IdIdeiaCocriador"] != null) 
                    && (HttpContext.Session["IdIdeiaCocriador"].ToString() != ""))
                {
                    return RedirectToAction("CoCriador", "Ideia", new { idIdeia = HttpContext.Session["IdIdeiaCocriador"].ToString() });
                }                    
                else if ((HttpContext.Session["IdIdeiaAvaliador"] != null) 
                    && (HttpContext.Session["IdIdeiaAvaliador"].ToString() != ""))
                {
                    return RedirectToAction("Ideias", "Avaliacao", new { idIdeia = HttpContext.Session["IdIdeiaAvaliador"].ToString() });
                }                    
                else
                {
                    return RedirectToAction("Index", "Home");
                }                    
            }
        }

        [HttpPost]
        public ActionResult ValidarLoginFacebook(User user)
        {
            try
            {
                User newUser = user.GetUsuarioFacebook(user);
                newUser.URLIMAGEM = user.URLIMAGEM;
                if (newUser.NOME == "")
                    newUser = user.CadastrarUsuarioFacebook(user);
                else
                    user.AtualizarImagem(newUser);

                CriarSessionUser(newUser);
                ViewBag.User = newUser;
                
                if((HttpContext.Session["IdIdeiaCocriador"] != null) 
                    && (HttpContext.Session["IdIdeiaCocriador"].ToString() != ""))
                {
                    return Json(new { COCRIACAO = HttpContext.Session["IdIdeiaCocriador"].ToString() }, JsonRequestBehavior.AllowGet);
                }                    
                else if ((HttpContext.Session["IdIdeiaAvaliador"] != null) 
                    && (HttpContext.Session["IdIdeiaAvaliador"].ToString() != ""))
                {
                    return Json(new { AVALIACAO = HttpContext.Session["IdIdeiaAvaliador"].ToString() }, JsonRequestBehavior.AllowGet);
                }                    
                else
                {
                    return RedirectToAction("Index", "Home");
                }             
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult ValidarLoginGoogle(User user)
        {
            try
            {
                User newUser = user.GetUsuarioGoogle(user);
                if (newUser.NOME == "")
                    newUser = user.CadastrarUsuarioGoogle(user);

                CriarSessionUser(newUser);
                ViewBag.User = newUser;
                
                if((HttpContext.Session["IdIdeiaCocriador"] != null) 
                    && (HttpContext.Session["IdIdeiaCocriador"].ToString() != ""))
                {
                    return Json(new { COCRIACAO = HttpContext.Session["IdIdeiaCocriador"].ToString() }, JsonRequestBehavior.AllowGet);                    
                }                    
                else if ((HttpContext.Session["IdIdeiaAvaliador"] != null) 
                    && (HttpContext.Session["IdIdeiaAvaliador"].ToString() != ""))
                {
                    return Json(new { AVALIACAO = HttpContext.Session["IdIdeiaAvaliador"].ToString() }, JsonRequestBehavior.AllowGet);                    
                }                    
                else
                {
                    return RedirectToAction("Index", "Home");
                }             

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet]
        public ActionResult LogOff()
        {
            LimparSession();
            return RedirectToAction("Index", "Login");
        }

        [HttpPost]
        public ActionResult CadastrarUsuario(User user)
        {
            if (!new User().VerificarEmail(user.EMAIL.Trim()))
            {
                var newuser = user.CadastrarUsuario(user);
                if (newuser.NOME != "")
                {
                    CriarSessionUser(newuser);
                    if ((HttpContext.Session["IdIdeiaCocriador"] != null)
                        && (HttpContext.Session["IdIdeiaCocriador"].ToString() != ""))
                    {
                        return RedirectToAction("CoCriador", "Ideia", new { idIdeia = HttpContext.Session["IdIdeiaCocriador"].ToString() });
                    }
                    else if ((HttpContext.Session["IdIdeiaAvaliador"] != null)
                        && (HttpContext.Session["IdIdeiaAvaliador"].ToString() != ""))
                    {
                        return RedirectToAction("Ideias", "Ideia", new { idIdeia = HttpContext.Session["IdIdeiaAvaliador"].ToString() });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Index", "User");
                }
            }
            else
            {
                return RedirectToAction("Index", "User");
            }

        }

        public void CriarSessionUser(User user)
        {
            
            HttpContext.Session["UserIsAuthenticated"] = true;
            HttpContext.Session["Id"] = user.ID;
            HttpContext.Session["Nome"] = user.NOME;
            HttpContext.Session["Email"] = user.EMAIL;
            HttpContext.Session["Role"] = user.ROLE;
            HttpContext.Session["urlimagem"] = user.URLIMAGEM != "" ? user.URLIMAGEM : "/images/user icon.jpeg";
            HttpContext.Session.Timeout = 2440; // 24 horas.

            var sessionDefault = new User().ValidacaoSession(user.ID);
            HttpContext.Session["master_idUsuario"] = sessionDefault.MASTER_ID;
            HttpContext.Session["master_qtdUsuarios"] = sessionDefault.QTD_USUARIOS;
            HttpContext.Session["master_qtdIdeias"] = sessionDefault.QTD_IDEIAS;
            HttpContext.Session["master_qtdAvaliacoes"] = sessionDefault.QTD_AVALIACOES;
            

        }
        
        public void LimparSession()
        {
            HttpContext.Session.Clear();
            HttpContext.Response.Cookies.Clear();
            Response.Cookies["Authorize"].Expires = DateTime.Now.AddDays(-1);

            FormsAuthentication.SignOut();
        }

        [HttpGet]
        public ActionResult RedefinirSenha()
        {
            return View();
        }

        [HttpGet]
        public ActionResult RedefinirSenhaAtual(string acesso)
        {
            var user = new User().ValidarAlteracaoSenha(acesso);
            if (user.ID > 0)
            {
                ViewBag.Usuario = user;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Login");
            }
        }

        [HttpGet]
        public bool ValidarEmailInformado(string email)
        {
            return new User().VerificarEmail(email);
        }

        [HttpGet]
        public bool EnviarLinkRecuperacao(string email, string urlSite)
        {
            return new User().EnviarLinkRecuperacao(email, urlSite);
        }


        [HttpGet]
        public bool AlterarSenha(string email, string senha)
        {
            var alterado = new User().AlterarSenha(email, senha);
            return alterado;
        }

        public JsonResult verificaLogin()
        {
            if (HttpContext.Session["UserIsAuthenticated"] != null)
            {
                return Json(HttpContext.Session["Id"], JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


    }
}