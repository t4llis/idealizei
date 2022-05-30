using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public sealed class CupomModel
    {
        public int ID_CUPOM { get; set; }
        public String CUPOM { get; set; }
        public String STATUS { get; set; }
        public String DESCRICAO { get; set; }
        public DateTime VALIDADE { get; set; }
        public String DESCONTO { get; set; }
        public String QUANTIDADE { get; set; }
        public String DISPONIVEL { get; set; }
    }
}



