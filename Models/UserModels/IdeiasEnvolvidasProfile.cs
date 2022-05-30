using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.UserModels
{
    public class IdeiasEnvolvidasProfile
    {
        public string NOME_IDEIA { get; set; }
        public string LIDER { get; set; }

        public IdeiasEnvolvidasProfile()
        {
            NOME_IDEIA = "";
            LIDER = "";
        }

    }
}