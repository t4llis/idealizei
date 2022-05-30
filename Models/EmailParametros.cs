using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class EmailParametros
    {
        public string NOME_DESTINATARIO { get; set; }
        public string EMAIL_DESTINATARIO { get; set; }
        public string ASSUNTO { get; set; }
        public string MENSAGEM { get; set; }
        public string ID_IDEIA { get; set; }
        public string TIPO_ENVIO { get; set; }

        public EmailParametros()
        {
            EMAIL_DESTINATARIO = "";
            ASSUNTO = "";
            MENSAGEM = "";
        }
    }
}