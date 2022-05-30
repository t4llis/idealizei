using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Equipe_Usuarios
    {
        public string ID { get; set; }
        public string ID_EQUIPE { get; set; }
        public string ID_USER { get; set; }


        public Equipe_Usuarios()
        {
            ID = "0";
            ID_EQUIPE = "0";
            ID_USER = "0";
        }
    }
}