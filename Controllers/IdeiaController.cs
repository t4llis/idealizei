using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Enum;
using WebApp.Models;
using WebApp.Utils;

namespace WebApp.Controllers
{
    public class IdeiaController : BaseController
    {
        
        public ActionResult Index(string aba)
        {
            StartController("IdeiaController", "Index");
            try
            {
                if (!Autorized())
                    return RedirectToAction("Index", "Login");

                if (aba != null)
                    ViewBag.AbaActiva = aba;
                else
                    ViewBag.AbaActiva = "1";

            }
            finally {
                EndController("IdeiaController", "Index");
            }
            return View();
        }

        public JsonResult CadastrarIdeia(Ideia model)
        {
            StartController("IdeiaController", "CadastrarIdeia");
            long IdIdeia = 0;
            try
            {
                model.PadrozinarLinkPit();
                model.NOMEPROJETO = model.NOMEPROJETO.Replace("'", "\"");
                model.TEXTOIDEIAINOVADORA = model.TEXTOIDEIAINOVADORA.Replace("'", "\"");
                model.TEXTOPROBLEMAS = model.TEXTOPROBLEMAS.Replace("'", "\"");
                model.TEXTOPRATICA = model.TEXTOPRATICA.Replace("'", "\"");
                model.TEXTORESULTADOS = model.TEXTORESULTADOS.Replace("'", "\"");
                model.TEXTOIMPACTO = model.TEXTOIMPACTO.Replace("'", "\"");

                IdIdeia = model.CadastrarIdeia();
            }
            finally {
                EndController("IdeiaController", "CadastrarIdeia");
            }

            return Json(IdIdeia, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Details(string idIdeia)
        {
         
            if (!Autorized())
                return RedirectToAction("Index", "Login");
            else
            {
                if (HttpContext.Session["IdIdeiaCadastrada"] == null)
                    HttpContext.Session["IdIdeiaCadastrada"] = idIdeia;
                else if(idIdeia == "")
                    return RedirectToAction("Index", "Home");
            }
            try
            {
                return RedirectToAction("Edit", "Ideia", new { idIdeia  = idIdeia });

                /*
                var NewIdeia = new Ideia().BuscarIdeiasCoCriador(idIdeia, "0");
                var NewRespostas = new Ideia().BuscarRespostas(idIdeia);
                var status = new Ideia().BuscarStatusProjeto(idIdeia);

                ViewBag.Ideia = NewIdeia;
                ViewBag.Respostas = NewRespostas;                
                ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(idIdeia);
                ViewBag.Status = status;
                ViewBag.IdUsuarioLogado = HttpContext.Session["Id"].ToString();
                ViewBag.QtdIdeias = GetRespostaCoCriadores(idIdeia);
                
                return View();
                */
            }
            catch (Exception erro)
            {
                throw erro;
            }
            
        }

        [HttpGet]
        public ActionResult Edit(string idIdeia, string TipoIdeia, string nomeIdeia)
        {
            StartController("IdeiaController", "Edit");

            if (!Autorized())
                return RedirectToAction("Index", "Login");

            try
            {
                if ((idIdeia == null) || (idIdeia == "0"))
                {
                    long IdIdeiaCadastrada = 0;
                    var ObjIdeia = new Ideia();
                    ObjIdeia.STATUS = "1";
                    ObjIdeia.LIDER = Session["NOME"].ToString();
                    ObjIdeia.TIPOPROJETO = TipoIdeia;
                    ObjIdeia.NOMEPROJETO = nomeIdeia;
                    IdIdeiaCadastrada = ObjIdeia.CadastrarIdeia();
                    
                    if (IdIdeiaCadastrada == 0)
                        return Redirect("https://idealizei.com.br/idealizacao/#planos");

                    idIdeia = IdIdeiaCadastrada.ToString();
                    ObjIdeia.GerarHashId(idIdeia);
                    idIdeia = new Ideia().GetHashId(idIdeia);
                    
                }
                
                var newIdIdeia = new Ideia().GetIdHash(idIdeia);
                if (newIdIdeia == "")
                    newIdIdeia = idIdeia;
                ViewBag.Cabecalho = new IdeiaCabecalho().BuscarCabecalhoIdeia(newIdIdeia);
                ViewBag.Perguntas = new IdeiaPergunta().BuscarPerguntasIdeia();
                ViewBag.quntCardsCoCriadores = GetRespostaCoCriadores(newIdIdeia);

            }
            finally
            {
                EndController("IdeiaController", "Edit");
            }
            return View();
        }


        [HttpGet]
        public JsonResult BuscarCabecalhoIdeia(string idIdeia)
        {
            return Json(new IdeiaCabecalho().BuscarCabecalhoIdeia(idIdeia), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool EditCabecalhoIdeia(IdeiaCabecalho Ideia)
        {
            return new IdeiaCabecalho().EditCabecalhoIdeia(Ideia.ID, Ideia.NOMEIDEIA);
        }

        [HttpGet]
        public JsonResult BuscarDescricaoProblema(string idIdeia)
        {
            return Json(new IdeiaDescricaoProblema().BuscarDescricaoProblema(idIdeia), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool EditDescricaoProblema(IdeiaCabecalho Ideia)
        {
            return new IdeiaDescricaoProblema().EditDescricaoProblema(Ideia.ID, Ideia.DOR);
        }

        [HttpGet]
        public JsonResult BuscarSolucoes(string idIdeia)
        {
            return Json(new IdeiaSolucoes().BuscarIdeiaSolucoes(idIdeia), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SalvarSolucoes(List<IdeiaSolucoes> param)
        {
            return Json(new IdeiaSolucoes().Salvar(param), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public bool EditSolucaoProjeto(IdeiaSolucoes param)
        {
            return new IdeiaSolucoes().EditSolucaoProjeto(param.ID, param.ID_PROJETO);
        }

        [HttpPost]
        public string CadastrarRespostaPergunta(IdeiaRespostaPergunta dados)
        {
            if (dados.RESPOSTA.Length > 800)
            {
                return "Máximo de 800 caracteres!";
            }

            return new IdeiaRespostaPergunta().CadastrarPergunta(dados).ToString();
        }

        [HttpGet]
        public string BuscarRespostaPergunta(string idIdeia, string idPergunta)
        {
            return new IdeiaRespostaPergunta().BuscarRespostaPergunta(idIdeia, idPergunta);
        }

        [HttpGet]
        public string BuscarTotalRespostaPergunta(string idIdeia)
        {
            return new IdeiaRespostaPergunta().BuscarTotalRespostaPergunta(idIdeia);
        }

        [HttpGet]
        public int GetRespostaCoCriadores(string idIdeia)
        {
            return new Ideia().GetQtdRespostaCoCriadores(idIdeia);
        }

        [HttpGet]
        public ActionResult Share(string idIdeia, ShareEnum shareEnum = ShareEnum.COCRIACAO)
        {
            StartController("IdeiaController", "Share");

            if (!Autorized())
                return RedirectToAction("Index", "Login");

            if (HttpContext.Session["IdIdeiaCadastrada"] == null)
                HttpContext.Session["IdIdeiaCadastrada"] = idIdeia;

            if(shareEnum == ShareEnum.COCRIACAO)
                AtualizarStatusIdeia(idIdeia, Convert.ToInt32(StatusEnum.COCRIACAO));
            else
                AtualizarStatusIdeia(idIdeia, Convert.ToInt32(StatusEnum.AVALIACAO));

            ViewBag.Ideia = new Ideia().GetHashId(idIdeia);
            ViewBag.ShareEnum = shareEnum.ToString();

            EndController("IdeiaController", "Share");

            return View();
        }

        public JsonResult BuscarIdeias()
        {
            return Json(new Ideia().BuscarIdeiasAvaliar(HttpContext.Session["Id"].ToString()), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Ideias()
        {
            StartController("IdeiaController", "Ideias");
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            ViewBag.ListaIdeias = new Ideia().BuscarIdeiasCoCriador(HttpContext.Session["Email"].ToString(), Convert.ToInt64(HttpContext.Session["Id"]));
            EndController("IdeiaController", "Ideias");

            return View();
        }
    
        public JsonResult EnviarEmail(EmailParametros Model)
        {                        
            var listEmails = Model.EMAIL_DESTINATARIO.Split(',');
            bool enviado = false;
            var mail = new Mail()
            {
                ASSUNTO = Model.ASSUNTO,
                MENSAGEM = string.Format(Model.MENSAGEM, Session["NOME"].ToString()),
                NOME_DESTINATARIO = Model.NOME_DESTINATARIO
            };

            if (Model.TIPO_ENVIO == "COCRIACAO")
                new Ideia().CadastrarEmailCocriadores(listEmails, Model.ID_IDEIA);

            foreach (var item in listEmails)
            {
                try
                {
                    if (Model.TIPO_ENVIO == "AVALIACAO")
                        cadastraAvaliador(Model.ID_IDEIA, item.Trim());

                    mail.EMAIL_DESTINATARIO = item.Trim();
                    enviado = mail.EnviarEmail();
                }catch(Exception ex)
                {

                }
            }
            
            return Json(enviado, JsonRequestBehavior.AllowGet);
        }

        private void cadastraAvaliador(string idIdeia, string emailAvaliador)
        {
            new Ideia().cadastrarPergustasAvaliacao(Convert.ToInt64(idIdeia), emailAvaliador);
        }

        [HttpGet]
        public ActionResult CoCriador(string idIdeia)
        {
            StartController("IdeiaController", "CoCriador");

            if (idIdeia != "")
            {
                string idIdeiaHasg = new Ideia().GetIdHash(idIdeia).ToString();
                if(idIdeiaHasg != "")
                    HttpContext.Session["IdIdeiaCocriador"] = idIdeiaHasg;
            }

            if (!Autorized())
                return RedirectToAction("Index", "Login");
            new Ideia().CadastrarEmailCocriadores(Session["Email"].ToString(), idIdeia);

            EndController("IdeiaController", "CoCriador");

            return RedirectToAction("Edit", "Ideia", new { idIdeia = idIdeia });
        }
    
        [HttpGet]
        public ActionResult CardCoCriador(string idIdeia, string id_Questao_ideias)
        {
            StartController("IdeiaController", "CoCriador");
            
            if (!Autorized())
                return RedirectToAction("Index", "Login");
            else
            {
                var NewIdeia = new Ideia().BuscarIdeiasCoCriador(idIdeia, id_Questao_ideias);
                var NewRespostas = new Ideia().BuscarRespostas(idIdeia, id_Questao_ideias);
                ViewBag.Ideia = NewIdeia;
                ViewBag.Respostas = NewRespostas;                
                ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(idIdeia);
            }
            EndController("IdeiaController", "CoCriador");
            
            return View();
        }

        [HttpPost]
        public JsonResult AddDescricaoCardCoCriador(CardCoCriacao item)
        {
            item.RESPOSTA = item.RESPOSTA.Replace("'", "\"");

            if (item.RESPOSTA.Length > 400)
            {
                return Json("Máximo de 400 caracteres!");
            }

            return Json(new Ideia().AddDescricaoCardCoCriador(item, _cores), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EditDescricaoCardCoCriador(CardCoCriacao item)
        {
            item.RESPOSTA = item.RESPOSTA.Replace("'", "\"");

            if (item.RESPOSTA.Length > 400)
            {
                return Json("Máximo de 400 caracteres!");
            }

            return Json(new Ideia().EditDescricaoCardCoCriador(item), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDescricaoCardCoCriador(CardCoCriacao item)
        {
            HttpCookie cookie2 = Request.Cookies["Authorize"];
            string token = cookie2.Value.ToString();
            if (!TokenService.TokenOk(token))
                RedirectToAction("Index", "Login");

            var user = new User().GetUsuario(TokenService.GetId(token));
            TokenService.RefreshSession(user);
            return Json(new Ideia().DeleteDescricaoCardCoCriador(item), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult BuscarCardsCoCriacao(string idIdeia, string idPergunta)
        {
            return Json(new CardCoCriacao().BuscarCardsCoCriacao(idIdeia, idPergunta), JsonRequestBehavior.AllowGet);
        }
        

        [HttpGet]
        public ActionResult IdeiasConcluidas()
        {
            StartController("IdeiaController", "IdeiasConcluidas");

            if (!Autorized())
                return RedirectToAction("Index", "Login");

            string idUsuario = HttpContext.Session["Id"].ToString();
            List<Ideia> listaIdeias = new Ideia().BuscarIdeiasConcluidas(idUsuario);            

            AvaliacaoEnum avaliacaoEnum = AvaliacaoEnum.FEEDBACK;            
            foreach (Ideia ideia in listaIdeias)
            {
                List<Ideia> listaIdeiasComMediaAtualizada = new AvaliacaoController().GetlistaIdeiasComMediaAtualizadaComFeedbacks(avaliacaoEnum, idUsuario, ideia.ID.ToString());
                if (listaIdeiasComMediaAtualizada.Count > 0)
                {
                    ideia.MEDIA = listaIdeiasComMediaAtualizada[0].MEDIA;
                }                
            }

            ViewBag.IdeiasConcluidas = listaIdeias;

            EndController("IdeiaController", "IdeiasConcluidas");

            return View();
        }

        [HttpGet]
        public ActionResult Ideia(string idIdeia) 
        {
            StartController("IdeiaController", "Ideia");

            if (!Autorized())
                return RedirectToAction("Index", "Login");
            if (idIdeia != null && idIdeia != "")
            {
                ViewBag.Ideia = BuscarIdeias(idIdeia);
                ViewBag.Respostas = new Ideia().BuscarRespostas(idIdeia);                
                ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(idIdeia);
                ViewBag.idIdeia = idIdeia;
            }

            EndController("IdeiaController", "Ideia");

            return View();
        }

        public JsonResult BuscarIdeias(String idIdeia)
        {
            return Json(new Ideia().BuscarIdeias(idIdeia), JsonRequestBehavior.AllowGet);
        }

        public void AtualizarStatusIdeia(string idIdeia, int status)
        {
            var ideia = new Ideia();
            ideia.AtualizarStatus(idIdeia, status);
        }

        public JsonResult DuplicarIdeia(string idIdeiaOrigem, string nomeIdeia)
        {
            if(idIdeiaOrigem != null && nomeIdeia != null)
                return Json(new Ideia().DuplicarIdeia(idIdeiaOrigem, nomeIdeia), JsonRequestBehavior.AllowGet);
            else
                return Json("0", JsonRequestBehavior.AllowGet);
        }

        public JsonResult ValidaNomeIdeia(string nomeIdeia)
        {
            if (nomeIdeia != null)
            {
                try
                {
                    var qtdEncontrado = new Ideia().BuscarQtdProjetoPorIdUsuarioENomeIdeia(nomeIdeia);
                    return Json(qtdEncontrado == 0, JsonRequestBehavior.AllowGet);
                }
                catch (Exception e)
                {
                    throw e;
                }
            } else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }                
        }

        [HttpPost]
        public JsonResult EditIdeia(Ideia ideia)
        {
            ideia.TEXTOIDEIA = ideia.TEXTOIDEIA.Replace("'", "\"");
            return Json(new Ideia().EditIdeia(ideia.ID, ideia.ID_QUESTAO_IDEIAS, ideia.TEXTOIDEIA), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string EditLinkPit(string idieia, string linkPit)
        {
            if (Autorized())
            {
                var newideia = new Ideia();
                newideia.ID = Convert.ToInt32(idieia);
                newideia.LINKPIT = linkPit;
                if (linkPit.Trim() != "")
                {
                    newideia.PadrozinarLinkPit();
                }
                return newideia.EditLinkPit(newideia.ID, newideia.LINKPIT);
            }
            else
                return "";
        }

        [HttpGet]
        public JsonResult GetHashId(string id)
        {
            if (Autorized())
            {
                var item = new Ideia().GetHashId(id);
                return Json(item, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }



        [HttpGet]
        public JsonResult BuscarIdeiasConcluidas()
        {
            if (Autorized())
            {
                List<IdeiaCards> IdeiasConcluidas = new IdeiaCards().BuscarIdeiasConcluidas(HttpContext.Session["Id"].ToString());
                return Json(IdeiasConcluidas, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult BuscarIdeiasEmAndamento()
        {
            if (Autorized())
            {
                List<IdeiaCards> IdeiasEmAndamento = new IdeiaCards().BuscarIdeiasEmAndamento(HttpContext.Session["Email"].ToString(), HttpContext.Session["Id"].ToString());
                return Json(IdeiasEmAndamento, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult UploadFile(HttpPostedFileBase myfile, string idideia)
        {
            if (Autorized())
            {
                foreach (string item in Request.Files)
                {
                    HttpPostedFileBase hpf = Request.Files[item] as HttpPostedFileBase;
                    if (hpf.ContentLength > 0)
                    {
                        string folderPath = Server.MapPath("~/Files_PDF");
                        Directory.CreateDirectory(folderPath);

                        string savedFileName = Server.MapPath("~/Files_PDF/" + hpf.FileName);
                        hpf.SaveAs(savedFileName);

                        string nome = hpf.FileName;
                        Ideia ideia = new Ideia();
                        ideia.CadastrarAnexo(idideia, nome.Replace(".pdf", ""));
                    }
                }
            }

            return RedirectToAction("Edit", "Ideia", new { idIdeia = idideia });
        }


        [HttpGet]
        public JsonResult GetAnexos(string idIdeia)
        {
            if (Autorized())
            {
                List<string> Anexos = new Ideia().GetAnexos(idIdeia);
                return Json(Anexos, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }
    }
}