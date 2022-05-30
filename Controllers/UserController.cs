using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.Models.UserModels;

namespace WebApp.Controllers
{
    public class UserController : BaseController
    {
        
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Profile()
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");
            else
            {
                if (Session["role"] != null && Session["role"].ToString() == "USER")
                    return View();
                else
                    return RedirectToAction("ProfileAdm", "User");
            }
        }

        public ActionResult ProfileAdm()
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");
            else
            {
                if (Session["role"] != null && Session["role"].ToString() == "USER")
                    return RedirectToAction("Profile", "User");
                else
                {
                    ViewBag.ListTags = new Tags().GetAll();
                    ViewBag.ListUsuarios = new User().GetAll();
                    return View();
                }
            }
        }

        [HttpGet]
        public JsonResult BuscarTextosCoCriacao(string iduser)
        {
            if (Autorized())
                return Json(new User().BuscarTextosCoCriacao(iduser), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarIdeiasEnvolvidas(string idUser)
        {
            if (Autorized())
                return Json(new User().BuscarIdeiasEnvolvidas(idUser), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SalvarInformacoes(HttpPostedFileBase file, string nome)
        {
            if (file != null && file.ContentLength > 0)
            {
                try
                {

                    System.Guid guid = System.Guid.NewGuid();
                    string[] extensaoImagem = file.FileName.Split('.');
                    string novoNomeImagem = guid.ToString();
                    string novoExtensaoImagem = extensaoImagem[extensaoImagem.Length - 1];

                    string path = Path.Combine(Server.MapPath("~/images/Profile/"), novoNomeImagem);
                    path += "." + novoExtensaoImagem;

                    file.SaveAs(path);
                    var newuser = new User().GetUsuario(Session["ID"].ToString());
                    newuser.NOME = nome.Trim();
                    newuser.URLIMAGEM = "../../images/Profile/" + novoNomeImagem + "." + novoExtensaoImagem;
                    newuser.ROLE = "ROLE_USER";
                    newuser.AtualizarImagem(newuser);
                    newuser.AtualizarUser(newuser);
                    Session["urlimagem"] = newuser.URLIMAGEM;

                } catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            }
            else
            {
                if (nome.Trim() != "")
                {
                    var newuser = new User().GetUsuario(Session["ID"].ToString());
                    newuser.NOME = nome.Trim();
                    newuser.ROLE = "ROLE_USER";
                    newuser.AtualizarUser(newuser);
                }
            }

            Session["NOME"] = nome;

            return RedirectToAction("Profile", "User");
        }

        [HttpPost]
        public ActionResult SalvarInformacoesAdm(string nome, string perfil, string nivelAcesso, string idUser, HttpPostedFileBase file)
        {
            try
            {

                System.Guid guid = System.Guid.NewGuid();
                string[] extensaoImagem = file.FileName.Split('.');
                string novoNomeImagem = guid.ToString();
                string novoExtensaoImagem = extensaoImagem[extensaoImagem.Length - 1];

                string path = Path.Combine(Server.MapPath("~/images/Profile/"), novoNomeImagem);
                path += "." + novoExtensaoImagem;

                file.SaveAs(path);


                var newuser = new User().GetUsuario(idUser);
                newuser.NOME = nome.Trim();
                newuser.ROLE = perfil.ToUpper().Trim();
                newuser.NIVELACESSO = nivelAcesso;
                newuser.URLIMAGEM = "../../images/Profile/" + novoNomeImagem + "." + novoExtensaoImagem;
                newuser.AtualizarImagem(newuser);
                newuser.AtualizarUser(newuser);
                
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR:" + ex.Message.ToString();
            }
            return RedirectToAction("Profile", "User");
        }
        
        [HttpPost]
        public JsonResult SalvarTags(List<Tags> tags)
        {
            if (Autorized())
                return Json(new Tags().Salvar(tags), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarEquipe(Equipe equipes)
        {
            if (Autorized())
                return Json(new Equipe().Salvar(equipes), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            if (Autorized())
                return Json(new Equipe().GetAll(), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ExcluirEquipe(string idEquipe)
        {
            if (Autorized())
                return Json(new Equipe().Delete(idEquipe), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPerfilAcessoUser(string idUser)
        {
            if (Autorized())
                return Json(new User().GetPerfilAcessoUser(idUser), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetNivelAcesso()
        {
            if (Autorized())
                return Json(new User().GetNivelAcesso(), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetTiposPerfil()
        {
            if (Autorized())
                return Json(new User().GetTiposPerfil(), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }
    

        [HttpGet]
        public JsonResult GetDadosGraficoPerfilAdmin(string idUser, string data_de, string data_ate)
        {
            if (Autorized())
                return Json(new User().GetDadosGraficoPerfilAdmin(idUser, data_de, data_ate), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetIdeal(string idUser)
        {
            if (Autorized())
                return Json(new IdealModel().GetIdeal(idUser), JsonRequestBehavior.AllowGet);
            else
                return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GerarLinkDeConvite()
        {
            if (Autorized())
            {
                var item = new User().GetUsuario(Session["ID"].ToString());
                return Json(new User().GerarLinkConvite(item), JsonRequestBehavior.AllowGet);
            }
            else
                return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult BuscarLinksDeConvite()
        {
            if (Autorized())
            {
                var item = new User().GetUsuario(Session["ID"].ToString());
                return Json(new User().GetAllLinkConvite(item), JsonRequestBehavior.AllowGet);
            }
            else
                return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}