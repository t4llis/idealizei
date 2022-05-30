using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Enum;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class AvaliacaoController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            ViewBag.ListaIdeias = BuscarIdeiasAvaliar();

            AvaliacaoEnum avaliacaoEnum = AvaliacaoEnum.AVALIADOR;
            ViewBag.QntCardsIdeiasAvaliadas = BuscarQntdCardsIdeiasAvaliadas(avaliacaoEnum);
            ViewBag.InfoCardsIdeiasAvaliadas = BuscarInfoCardsIdeiasAvaliadas(avaliacaoEnum);
            ViewBag.MediaPontuacao = BuscarMediaPontuacaoIdeiasAvaliadas(avaliacaoEnum);

            return View();
        }
        
        public JsonResult BuscarIdeiasAvaliar()
        {
            return Json(new Ideia().BuscarIdeiasAvaliar(HttpContext.Session["Id"].ToString()), JsonRequestBehavior.AllowGet);            
        }

        [HttpGet]
        public ActionResult Ideias(string idIdeia)
        {
            if (idIdeia != "")
            {
                string idIdeiahash = new Ideia().GetIdHash(idIdeia);
                if (idIdeiahash != "")
                    idIdeia = idIdeiahash;

                if (Autorized())
                    new Ideia().cadastrarPergustasAvaliacao(Convert.ToInt64(idIdeia), HttpContext.Session["Email"].ToString());

                HttpContext.Session["IdIdeiaAvaliador"] = idIdeia;
            }

            if (!Autorized())
                return RedirectToAction("Index", "Login");

            var podeAvaliar = new Avaliacao().VerificaContratoAvaliacoes(Convert.ToInt32(HttpContext.Session["master_idUsuario"].ToString()));
            if (!podeAvaliar)
                return Redirect("https://idealizei.com.br/idealizacao/#planos");

            ViewBag.IdeiasAvaliacao = BuscarIdeias(idIdeia);

            Boolean isRealizouFeedback = new Ideia().BuscarQtdFeedbacksPorIdeiaEPorIdUsuarioAvaliador(idIdeia, HttpContext.Session["Id"].ToString()) > 0;
            if (isRealizouFeedback)
            {
                ViewBag.JaRealizouFeedback = isRealizouFeedback;
                return View();
            }

            ViewBag.QuestoesAvaliacaoIdeias = new Ideia().BuscaTodasQuestoesAvaliacaoIdeias();
            ViewBag.Respostas = new Ideia().BuscarRespostas(idIdeia);
            ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(idIdeia);
            ViewBag.JaRealizouFeedback = false;
            ViewBag.ListTags = new Tags().BuscarTags();

            return View();
        }

        public JsonResult BuscarIdeias(String idIdeia)
        {
            return Json(new Ideia().BuscarIdeias(idIdeia), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AvaliacaoIdeia(String idCadastroIdeia)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            ViewBag.QuestoesAvaliacaoIdeias = QuestoesAvaliacaoIdeias(idCadastroIdeia);
            string idIdeia = ViewBag.QuestoesAvaliacaoIdeias.Data[0].IDPROJETO.ToString();
            ViewBag.IdeiasAvaliacao = BuscarIdeias(idIdeia);

            ViewBag.IdCadastroIdeiaAtual = Convert.ToInt32(idCadastroIdeia);

            int qtdCadastroIdeiasAvaliacao = ViewBag.IdeiasAvaliacao.Data.Count;
            ViewBag.IdUltimoCadastroIdeia = ViewBag.IdeiasAvaliacao.Data[qtdCadastroIdeiasAvaliacao - 1].ID;
            
            for (int i = 0; i < qtdCadastroIdeiasAvaliacao; i++)
            {     
                // verifica se é o utlimo cadastro de ideia
                if (ViewBag.IdUltimoCadastroIdeia == Convert.ToInt32(idCadastroIdeia))
                {
                    break;
                }

                // pega o próximo cadastro de idea
                if (ViewBag.IdeiasAvaliacao.Data[i].ID == Convert.ToInt32(idCadastroIdeia))
                {
                    ViewBag.IdCadastroIdeiaProximo = ViewBag.IdeiasAvaliacao.Data[i + 1].ID;
                    break;
                }
            }

            ViewBag.Respostas = new Ideia().BuscarRespostas(idIdeia);
            ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(idIdeia);
            
            return View();
        }

        public JsonResult QuestoesAvaliacaoIdeias(String idIdeia)
        {
            return Json(new Ideia().QuestoesAvaliacaoIdeias(idIdeia), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CadastrarAvaliacao(List<Ideia> listIdeia)
        {
            if (new Ideia().CadastrarAvaliacao(listIdeia, HttpContext.Session["Id"].ToString()) != 0)
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }   

        [HttpGet]
        public ActionResult IdeiasAvaliadasFeedbacks()
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            AvaliacaoEnum avaliacaoEnum = AvaliacaoEnum.FEEDBACK;
            ViewBag.QntCardsIdeiasAvaliadasFeedbacks = BuscarQntdCardsIdeiasAvaliadas(avaliacaoEnum);
            ViewBag.InfoCardsIdeiasAvaliadasFeedbacks = BuscarInfoCardsIdeiasAvaliadas(avaliacaoEnum);
            ViewBag.MediaPontuacaoFeedbacks = BuscarMediaPontuacaoIdeiasAvaliadasComQuantidadeFeedback(avaliacaoEnum, HttpContext.Session["Id"].ToString());

            return View();
        }       

        [HttpGet]
        public ActionResult IdeiaFeedbacks(string idIdeia)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            if (idIdeia != null && idIdeia != "")
            {
                var newIdeia = new Ideia().GetIdHash(idIdeia);

                List<Ideia> listaIdeias = new Ideia().BuscarIdeias(newIdeia);
                ViewBag.Ideia = Json(listaIdeias, JsonRequestBehavior.AllowGet);
                ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(newIdeia);

                AvaliacaoEnum avaliacaoEnum = AvaliacaoEnum.FEEDBACK;
                ViewBag.InfoCardsIdeiasAvaliadasFeedbacks = BuscarInfoCardsIdeiasAvaliadas(avaliacaoEnum, newIdeia);
                ViewBag.MediaPontuacaoFeedbacks = BuscarMediaPontuacaoIdeiasAvaliadasComQuantidadeFeedback(avaliacaoEnum, HttpContext.Session["Id"].ToString(), newIdeia);
                ViewBag.QtdFeedbackRecebido = BuscarQtdFeedbacksPorIdeia(newIdeia);
                ViewBag.AvaliacoesFeedbacks = new Avaliacao().GetAllAvaliacaoFeedbacksByIdIdeia(newIdeia);

                ViewBag.ComentariosAvaliacao = new Avaliacao().BuscarComentariosAvaliacao(newIdeia);
            }

            return View();
        }

        public int BuscarQntdCardsIdeiasAvaliadas(AvaliacaoEnum avaliacaoEnum)
        {
            return new Ideia().BuscarQntdCardsIdeiasAvaliadas(HttpContext.Session["Id"].ToString(), avaliacaoEnum);
        }

        [HttpPost]
        public JsonResult BuscarInfoCardsIdeiasAvaliadas(AvaliacaoEnum avaliacaoEnum, string idIdeia = "0")
        {
            return Json(new Ideia().BuscarInfoCardsIdeiasAvaliadas(HttpContext.Session["Id"].ToString(), avaliacaoEnum, idIdeia), JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult BuscarMediaPontuacaoIdeiasAvaliadasComQuantidadeFeedback(AvaliacaoEnum avaliacaoEnum, string idUsuario, string idIdeia = "0")
        {            
            return Json(GetlistaIdeiasComMediaAtualizadaComFeedbacks(avaliacaoEnum, idUsuario, idIdeia), JsonRequestBehavior.AllowGet); 
        }
        
        public List<Ideia> GetlistaIdeiasComMediaAtualizadaComFeedbacks(AvaliacaoEnum avaliacaoEnum, string idUsuario, string idIdeia = "0")
        {
            var listaIdeiasComMediaAtualizada = new List<Ideia>();
            List<Ideia> listaIdeias = new Ideia().BuscarMediaPontuacaoIdeiasAvaliadas(idUsuario, avaliacaoEnum, idIdeia);

            foreach (Ideia ideia in listaIdeias)
            {
                IdeiaTotais qtdFeedbacks = new Ideia().BuscarQtdFeedbacksPorIdeia(Convert.ToString(ideia.IDPROJETO));
                ideia.QTD_FEEDBACKS = Convert.ToInt32(qtdFeedbacks.VALOR);
                double mediaFinal = calculaMedia(ideia.MEDIA, Convert.ToInt32(qtdFeedbacks.VALOR));
                ideia.MEDIA = Math.Round(mediaFinal, 2);
                listaIdeiasComMediaAtualizada.Add(ideia);
            }

            return listaIdeiasComMediaAtualizada;
        }

        public JsonResult BuscarMediaPontuacaoQuestaoIdeia(string idIdeia, List<Ideia> listaIdeias)
        {
            var listaIdeiasComMediaAtualizada = new List<Ideia>();            

            foreach (Ideia ideia in listaIdeias)
            {
                Ideia ideiaComMediaQuestaoIdeia = new Ideia().BuscarMediaPontuacaoPorIdQuestaoIdeia(idIdeia, ideia.ID_QUESTAO_IDEIAS);
                IdeiaTotais qtdFeedbacks = new Ideia().BuscarQtdFeedbacksPorIdeia(idIdeia);
                double mediaFinal = calculaMedia(ideiaComMediaQuestaoIdeia.MEDIA_QUESTAO_IDEIA, Convert.ToInt32(qtdFeedbacks.VALOR));
                ideia.MEDIA_QUESTAO_IDEIA = Math.Round(mediaFinal, 2);
                listaIdeiasComMediaAtualizada.Add(ideia);
            }

            return Json(listaIdeiasComMediaAtualizada, JsonRequestBehavior.AllowGet);
        }

        public double calculaMedia(double media, int qtdFeedbacks)
        {
            return media / qtdFeedbacks;
        }

        public JsonResult BuscarMediaPontuacaoIdeiasAvaliadas(AvaliacaoEnum avaliacaoEnum, string idIdeia = "0")
        {
            return Json(GetListaIdeiasComMediaAtualizada(avaliacaoEnum, HttpContext.Session["Id"].ToString(), idIdeia), JsonRequestBehavior.AllowGet);
        }

        public List<Ideia> GetListaIdeiasComMediaAtualizada(AvaliacaoEnum avaliacaoEnum, string idUsuario, string idIdeia = "0")
        {
            var listaIdeiasComMediaAtualizada = new List<Ideia>();
            List<Ideia> listaIdeias = new Ideia().BuscarMediaPontuacaoIdeiasAvaliadas(idUsuario, avaliacaoEnum, idIdeia);

            foreach (Ideia ideia in listaIdeias)
            {
                ideia.MEDIA = Math.Round(ideia.MEDIA, 2);
                listaIdeiasComMediaAtualizada.Add(ideia);
            }

            return listaIdeiasComMediaAtualizada;
        }

        public JsonResult BuscarQtdFeedbacksPorIdeia(string idIdeia)
        {
            return Json(new Ideia().BuscarQtdFeedbacksPorIdeia(idIdeia), JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarMediaPontuacaoPorIdQuestaoIdeia(string idIdeia, int idQuestaoIdeia)
        {
            return Json(new Ideia().BuscarMediaPontuacaoPorIdQuestaoIdeia(idIdeia, idQuestaoIdeia), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AvaliacaoIdeiaFeedback(string idCadastroIdeia, string mediaQuestaoIdeia)
        {
            if (!Autorized())
                return RedirectToAction("Index", "Login");

            List<Ideia> listaQuestoesAvaliacaoIdeia = new Ideia().QuestoesAvaliacaoIdeias(idCadastroIdeia);
            ViewBag.QuestoesAvaliacaoIdeias = listaQuestoesAvaliacaoIdeia;
            string idIdeia = listaQuestoesAvaliacaoIdeia[0].IDPROJETO.ToString();
            ViewBag.IdeiasAvaliacao = BuscarIdeias(idIdeia);
            ViewBag.MediaPontuacaoQuestaoIdeia = mediaQuestaoIdeia;

            ViewBag.MediaPontuacaoQuestaoAvaliacaoIdeia = BuscarMediaPontuacaoQuestaoAvaliacaoIdeia(idIdeia, listaQuestoesAvaliacaoIdeia);

            ViewBag.Respostas = new Ideia().BuscarRespostas(idIdeia);
            ViewBag.NomesCoCriadores = new Ideia().BuscarNomesCoCriadores(idIdeia);            

            return View();
        }

        public JsonResult BuscarMediaPontuacaoQuestaoAvaliacaoIdeia(string idIdeia, List<Ideia> listaQuestoesAvaliacaoIdeia)
        {
            var listaIdeiasComMediaAtualizada = new List<Ideia>();

            foreach (Ideia ideia in listaQuestoesAvaliacaoIdeia)
            {
                Ideia ideiaComPontuacaoQuestaoAvaliacaoIdeia = new Ideia().BuscarPontuacaoPorIdQuestaoAvaliacaoIdeia(idIdeia, ideia.IDQUESTAOAVALIACAOIDEIA);
                IdeiaTotais qtdFeedbacks = new Ideia().BuscarQtdFeedbacksPorIdeia(idIdeia);
                double mediaFinal = calculaMedia(Convert.ToDouble(ideiaComPontuacaoQuestaoAvaliacaoIdeia.PONTUACAO), Convert.ToInt32(qtdFeedbacks.VALOR));
                ideia.MEDIA_QUESTAO_AVALIACAO_IDEIA = Math.Round(mediaFinal);
                listaIdeiasComMediaAtualizada.Add(ideia);
            }

            return Json(listaIdeiasComMediaAtualizada, JsonRequestBehavior.AllowGet);
        }
    }
}