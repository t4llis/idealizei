using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class RepostaCocriador
    {
        public int ID { get; set; }
        public string RESPOSTA { get; set; }
        public DateTime DATA_HORA { get; set; }
        public int ID_USUARIO { get; set; }
        public string NOME_USUARIO { get; set; }
        public int ID_PROJETO { get; set; }
        public int ID_QUESTAO_IDEIAS { get; set; }
        public string COR_CARD { get; set; }

        public RepostaCocriador()
        {
            ID = 0;
            RESPOSTA = ""; 
            DATA_HORA = new DateTime();
            ID_USUARIO = 0;
            ID_PROJETO = 0;
            ID_QUESTAO_IDEIAS = 0;
            COR_CARD = "";
        }
    }
}