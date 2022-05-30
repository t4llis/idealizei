using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class GraficoAdm
    {
        public string QTD_IDEIAS { get; set; }
        public string MES_ANO_IDEIAS { get; set; }
        public string QTD_COCRIACAO { get; set; }
        public string MES_ANO_COCRIACAO { get; set; }

        public GraficoAdm()
        {
            QTD_IDEIAS = "0";
            QTD_COCRIACAO = "0";
            MES_ANO_IDEIAS = "";
            MES_ANO_COCRIACAO = "";
        }
    }
}