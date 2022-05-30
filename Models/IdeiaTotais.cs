using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class IdeiaTotais
    {
        public string TIPO { get; set; }
        public string VALOR { get; set; }

        public IdeiaTotais()
        {
            TIPO = "";
            VALOR = "";
        }
    }
}