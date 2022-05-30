using CRUDAjax.UI.Models.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.Negocio
{
    public sealed class CupomNeg
    {
        public void Cadastrar(CupomModel cupom)
        {
            new CupomRep().Cadastrar(cupom);
        }

        public void Atualizar(CupomModel cupom)
        {
            new PessoaRep().Atualizar(cupom);
        }

        public void Deletar(int idCupom)
        {
            new CupomRep().Deletar(idCupom);
        }

        public CupomModel GetById(int id)
        {
            return new CupomRep().GetById(id);
        }

        public IEnumerable<CupomModel> Listar()
        {
            return new CupomRep().Listar();
        }
    }
}