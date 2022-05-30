using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class IdeiaCabecalho
    {
        public string ID { get; set; }
        public string NOMEIDEIA { get; set; }
        public string DOR { get; set; }
        public string LIDER { get; set; }
        public string COCRIADORES { get; set; }
        public string LINK_PIT { get; set; }
        public string ID_USUARIO { get; set; }
        public string STATUS { get; set; }



        public IdeiaCabecalho()
        {
            ID = "0";
            NOMEIDEIA = "";
            DOR = "";
            LIDER = "";
            COCRIADORES = "";
            LINK_PIT = "";
            ID_USUARIO = "";
            STATUS = "0";
        }

        public IdeiaCabecalho BuscarCabecalhoIdeia(string idIdeia)
        {
            var db = new ClassDb();
            var Cabecalho = new IdeiaCabecalho();
            try
            {
                try
                {
                    var sql = " select p.id,                      " +
                              "        p.nome_projeto,            " +
                              "        p.lider,                   " +
                              "        p.linkpit,                 " +
                              "        p.id_usuario,              " +
                              "        p.dor,                     " +
                              "        p.status                   " +
                              "  from  projetos p                 " +
                              " where p.id = @projeto             ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        Cabecalho.ID = dr["id"].ToString();
                        Cabecalho.NOMEIDEIA = dr["nome_projeto"].ToString();
                        Cabecalho.LIDER = dr["lider"].ToString();
                        Cabecalho.LINK_PIT = dr["linkpit"].ToString();
                        Cabecalho.ID_USUARIO = dr["id_usuario"].ToString();
                        Cabecalho.DOR = dr["dor"].ToString();
                        Cabecalho.STATUS = dr["status"] == null || dr["status"].ToString() == ""  ? "0" : dr["status"].ToString();
                        Cabecalho.COCRIADORES = new Ideia().BuscarNomesCoCriadores(idIdeia);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();
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

            return Cabecalho;
        }

        public bool EditCabecalhoIdeia(string idIdeia, string nomeIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "update projetos                  " +
                              "   set nome_projeto = @nomeIdeia " +
                              " where id = @idIdeia ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nomeIdeia", nomeIdeia);                      
                        cmd.Parameters.AddWithValue("@idIdeia", idIdeia);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    return false;
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