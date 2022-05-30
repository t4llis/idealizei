using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AdmController : BaseController
    {
        // GET: Cupom
        public ActionResult Index()
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            List<AdmCupom> CuponsList = new AdmCupom().BuscarCupons();
            List<AdmPagamento> PagamentosList = new AdmPagamento().BuscarPagamentos();
            List<AdmContrato> ContratosList = new AdmContrato().BuscarContratos();
            List<AdmPlano> PlanosList = new AdmPlano().BuscarPlanos();

            ViewBag.Cupons = CuponsList;
            ViewBag.Pagamentos = PagamentosList;
            ViewBag.Contratos = ContratosList;
            ViewBag.Planos = PlanosList;

            return View();
        }

        // Cupom
        public ActionResult SelectCup(AdmCupom cup)
        {
            var ddCup = cup.SelectCup(cup.CUPOM);
            //HttpContext.Session["cupom_id"] = ddCup.ID_CUPOM;
            return Json(ddCup, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectCupom(AdmCupom cupom)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            var dadosCupom = cupom.SelectCupom(cupom.ID_CUPOM);
            return Json(dadosCupom, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreateCupom(AdmCupom ddFormCupom)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                return Json(ddFormCupom.CreateCupom(ddFormCupom), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AlterarCupom(AdmCupom ddFormCupom)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                return Json(ddFormCupom.AlterarCupom(ddFormCupom), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteCupom(AdmCupom cupom)
        {
            return Json(new AdmCupom().DeleteCupom(cupom), JsonRequestBehavior.AllowGet);
        }




        public ActionResult SelectUserContrato(User usuario)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            var usuarios = usuario.GetUsuarioContrato(usuario.NOME);
            return Json(usuarios, JsonRequestBehavior.AllowGet);
        }
        public ActionResult SelectContrato(AdmContrato contrato)
        {
            var dadosCupom = contrato.SelectContrato(contrato.ID_CONTRATO);
            return Json(dadosCupom, JsonRequestBehavior.AllowGet);
        }
        // Criar Contrato
        public ActionResult CreateContrato(AdmContrato ddFormContrato)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                return Json(ddFormContrato.CreateContrato(ddFormContrato), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult AlterarContrato(AdmContrato contrato)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                return Json(contrato.AlterarContrato(contrato), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeleteContrato(AdmContrato contrato)
        {
            return Json(new AdmContrato().DeleteContrato(contrato), JsonRequestBehavior.AllowGet);
        }



        // Plano
        public ActionResult SelectPlano(AdmPlano plano)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            var dadosPlano = plano.SelectPlano(plano.ID_PLANO);
            return Json(dadosPlano, JsonRequestBehavior.AllowGet);
        }
        public ActionResult CreatePlano(AdmPlano ddFormPlano)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                return Json(ddFormPlano.CreatePlano(ddFormPlano), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult DeletePlano(AdmPlano plano)
        {
            return Json(new AdmPlano().DeletePlano(plano), JsonRequestBehavior.AllowGet);
        }
        public ActionResult AlterarPlano(AdmPlano plano)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                return Json(plano.AlterarPlano(plano), JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
        }



        // Carrinho
        public ActionResult CreateCarrinho(AdmCart ddCart)
        {
            return Json(ddCart.CreateCarrinho(ddCart), JsonRequestBehavior.AllowGet);
        }



        public JsonResult Stable(bool producao)
        {
            string apiKey = "ak_test_KmntjbsCFktxJjbqcpWahEue8GgTT7";
            if (producao)
            {
                apiKey = "ak_live_AC9CioWdIZ5QKLk5Pcx3T1oYa3VPoV";
            }
            return Json(apiKey, JsonRequestBehavior.AllowGet);
        }

        /*public JsonResult VerificaContratoUsuario(AdmValidacao master_idUsuario)
        {
            try
            {
                //var ddRet = master_idUsuario;
                var ddRet = master_idUsuario.VerificaContratoUsuario(master_idUsuario.ID_USUARIO);
                return Json(ddRet, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json("Erro", JsonRequestBehavior.AllowGet);
            }
            
        }*/

    }
}