using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;


using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WebApp.Models;

namespace WebApp.Models
{
    public class AdmCart
    {
        public int ID_USUARIO { get; set; }
        public string DATAHORA { get; set; }
        public string ID_CUPOM { get; set; }
        public string TOTAL_PRODUTOS { get; set; }
        public string DESCONTO_CUPOM { get; set; }
        public string VALOR_TOTAL { get; set; }
        public string ID_FORMAPAGTO { get; set; }
        public string STATUS { get; set; }

        public AdmCart() 
        {
            ID_USUARIO = 0;
            DATAHORA = DateTime.Now.ToString("yyyy-MM-dd");
            ID_CUPOM = "0";
            TOTAL_PRODUTOS = "0.00";
            DESCONTO_CUPOM = "";
            VALOR_TOTAL = "0.00";
            ID_FORMAPAGTO = "0";
            STATUS = "1";
        }

        public AdmCart SelectCart()
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select * ecom_carrinhos ORDER BY idCarrinho DESC LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    AdmCart cart = new AdmCart();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        cart.ID_USUARIO = Convert.ToInt32(dr["idUsuario"]);
                        cart.DATAHORA = dr["dataHora"].ToString();
                        cart.ID_CUPOM = dr["idCupom"].ToString();
                        cart.TOTAL_PRODUTOS = dr["totalProdutos"].ToString();
                        cart.DESCONTO_CUPOM = dr["descontoCupom"].ToString();
                        cart.VALOR_TOTAL = dr["valorTotal"].ToString();
                        cart.ID_FORMAPAGTO = dr["idFormaPagto"].ToString();
                        cart.STATUS = dr["status"].ToString();
                    }
                    return cart;
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

        public AdmCart CreateCarrinho(AdmCart cart)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    /*if (cart.ID_FORMAPAGTO = 1)
                    {
                        var status = 0;
                    }*/
                    var sql = "INSERT INTO ecom_carrinhos (idUsuario, dataHora, idCupom, totalProdutos, descontoCupom, valorTotal, idFormaPagto, status) " +
                          "VALUES (@idUsuario, Now(), @idCupom, @totalProdutos, @descontoCupom, @valorTotal, @idFormaPagto, 1) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@idUsuario", cart.ID_USUARIO);
                        cmd.Parameters.AddWithValue("@idCupom", cart.ID_CUPOM);
                        cmd.Parameters.AddWithValue("@totalProdutos", cart.TOTAL_PRODUTOS);
                        cmd.Parameters.AddWithValue("@descontoCupom", cart.DESCONTO_CUPOM);
                        cmd.Parameters.AddWithValue("@valorTotal", cart.VALOR_TOTAL);
                        cmd.Parameters.AddWithValue("@idFormaPagto", cart.ID_FORMAPAGTO);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return cart;
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