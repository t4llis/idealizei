using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Security;
using WebApp.Utils;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AdmPagamento
    {
        public int ID_CART { get; set; }
        public int CARRINHO_IDUSUARIO { get; set; }
        public string CARRINHO_USUARIONOME { get; set; }
        public string CARRINHO_DATAHORA { get; set; }
        public string CARRINHO_IDCUPOM { get; set; }
        public string CARRINHO_CUPOM { get; set; }
        public string CARRINHO_TOTALPRODUTOS { get; set; }
        public string CARRINHO_DESCONTOCUPOM { get; set; }
        public string CARRINHO_VALORTOTAL { get; set; }
        public string CARRINHO_FORMAPAGAMENTO { get; set; }
        public string CARRINHO_STATUS { get; set; }

        public List<AdmPagamento> BuscarPagamentos()
        {
            var db = new ClassDb();
            var Pagamento = new List<AdmPagamento>();
            try
            {
                try
                {
                    var sql = "SELECT "+
                        "c.idCarrinho, c.idUsuario, u.nome, "+
                        "DATE_FORMAT(c.dataHora, '%d/%m/%Y') as dataHora, "+
                        "c.idCupom, cp.cupom, c.totalProdutos, c.descontoCupom, c.valortotal, " +
                        "case c.idFormaPagto " +
                            "when 0 then 'Boleto' " +
                            "when 1 then 'Cartão de Crédito' " +
                            "when 2 then 'Débito Online' " +
                            "when 3 then 'Pix' " +
                        "end as formaPagamento, " +
                        "case c.`status` " +
                            "when 0 then 'Reprovado' " +
                            "when 1 then 'Aprovado' " +
                        "end as status " +
                        "FROM ecom_carrinhos as c JOIN usuarios as u JOIN ecom_cupons as cp " +
                        "ON c.idCupom = cp.idCupom AND c.idUsuario = u.id";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new AdmPagamento();
                        item.ID_CART = Convert.ToInt32(dr["idCarrinho"]);
                        item.CARRINHO_IDUSUARIO = Convert.ToInt32(dr["idUsuario"]);
                        item.CARRINHO_USUARIONOME = dr["nome"].ToString();
                        item.CARRINHO_DATAHORA = dr["dataHora"].ToString();
                        item.CARRINHO_IDCUPOM = dr["idCupom"].ToString();
                        item.CARRINHO_CUPOM = dr["cupom"].ToString();
                        item.CARRINHO_TOTALPRODUTOS = dr["totalProdutos"].ToString();
                        item.CARRINHO_DESCONTOCUPOM = dr["descontoCupom"].ToString();
                        item.CARRINHO_VALORTOTAL = dr["valortotal"].ToString();
                        item.CARRINHO_FORMAPAGAMENTO = dr["formaPagamento"].ToString();
                        item.CARRINHO_STATUS = dr["status"].ToString();

                        Pagamento.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Pagamento;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            finally
            {
                db.FecharConexao();
            }
        }
    }
}