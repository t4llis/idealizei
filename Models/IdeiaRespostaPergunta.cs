using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class IdeiaRespostaPergunta
    {
        public string ID { get; set; }
        public string ID_IDEIA { get; set; }
        public string ID_PERGUNTA { get; set; }
        public string EMAIL { get; set; }
        public string RESPOSTA { get; set; }
        
        public IdeiaRespostaPergunta()
        {

            ID = "";
            ID_IDEIA = "";
            ID_PERGUNTA = "";
            EMAIL = "";
            RESPOSTA = "";

        }

        public bool CadastrarPergunta(IdeiaRespostaPergunta dados)
        {
            var db = new ClassDb();
            try
            {
                try
                {

                    // PERGUNTAS
                    var sql = "delete from cadastro_ideias where id_projeto = @id_projeto and  id_questao_ideias = @id_questao_ideias";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id_projeto", dados.ID_IDEIA);
                        cmd.Parameters.AddWithValue("@id_questao_ideias", dados.ID_PERGUNTA);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        cmd.Connection.Close();
                    }


                    sql = "INSERT INTO cadastro_ideias (email_avaliador, resposta, id_projeto, id_questao_ideias, data_hora) " +
                          "                      VALUES(@email_avaliador, @resposta, @id_projeto, @id_questao_ideias, @data_hora) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@email_avaliador", dados.EMAIL);
                        cmd.Parameters.AddWithValue("@id_projeto", dados.ID_IDEIA);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@resposta", dados.RESPOSTA);
                        cmd.Parameters.AddWithValue("@id_questao_ideias", dados.ID_PERGUNTA);

                        cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
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

        public string BuscarRespostaPergunta(string idIdeia, string idPergunta)
        {
            var db = new ClassDb();
            string resposta = "";
            try
            {
                try
                {
                    var sql = "select resposta from cadastro_ideias where id_projeto = @id_projeto and id_questao_ideias = @id_pergunta";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_pergunta", idPergunta);
                    cmd.Parameters.AddWithValue("@id_projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        resposta = dr["resposta"].ToString();
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return resposta;
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

        public string BuscarTotalRespostaPergunta(string idIdeia)
        {
            var db = new ClassDb();
            string resposta = "0";
            try
            {
                try
                {
                    var sql = "select count(resposta) qtd from cadastro_ideias where id_projeto = @id_projeto and trim(resposta) <> ''";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        resposta = dr["qtd"].ToString();
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return resposta;
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