using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.UserModels
{
    public class IdeiasProfile
    {
        public string ID { get; set; }
        public string ID_IDEIA { get; set; }
        public string NOME_IDEIA { get; set; }
        public string RESPOSTA { get; set; }
        public string DATA_HORA { get; set; }
        public string COR_CARD { get; set; }


        public IdeiasProfile()
        {
            ID = "0";
            ID_IDEIA = "0";
            NOME_IDEIA = "";
            RESPOSTA = "";
            DATA_HORA = "";
            COR_CARD = "";
        }
    }
}