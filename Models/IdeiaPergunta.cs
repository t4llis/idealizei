using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class IdeiaPergunta
    {
        public string ID { get; set; }
        public string LETRA { get; set; }
        public string PERGUNTA { get; set; }
        public string DESCRICAO { get; set; }
        public string DESCRICAO_LETRA { get; set; }

        public string ORDEM { get; set; }

        public IdeiaPergunta()
        {
            ID = "";
            LETRA = "";
            PERGUNTA = "";
            DESCRICAO = "";
            DESCRICAO_LETRA = "";
            ORDEM = "";
        }


        public List<IdeiaPergunta> BuscarPerguntasIdeia()
        {
            var db = new ClassDb();
            var Perguntas = new List<IdeiaPergunta>();
            try
            {
                try
                {
                    var sql = "select id, letra, ordem, questao, titulo, descricao_letra from questoes_ideias order by ordem";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdeiaPergunta();
                        item.ID = dr["ID"].ToString();
                        item.LETRA = dr["LETRA"].ToString();
                        item.ORDEM = dr["ORDEM"].ToString();
                        item.PERGUNTA = dr["QUESTAO"].ToString();
                        item.DESCRICAO = dr["TITULO"].ToString();
                        item.DESCRICAO_LETRA = dr["DESCRICAO_LETRA"].ToString();
                        Perguntas.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Perguntas;
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