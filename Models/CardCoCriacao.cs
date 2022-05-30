using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class CardCoCriacao
    {
        public string ID { get; set; }
        public string ID_IDEIA { get; set; }
        public string ID_QUESTAO_IDEIA { get; set; }
        public string RESPOSTA { get; set; }
        public string DATA_HORA { get; set; }
        public string ID_USUARIO { get; set; }
        public string NOME_USUARIO { get; set; }
        public string COR_CARD { get; set; }

        public CardCoCriacao()
        {
            ID = "";
            ID_IDEIA = "";
            ID_QUESTAO_IDEIA = "";
            RESPOSTA = "";
            DATA_HORA = "";
            ID_USUARIO = "";
            NOME_USUARIO = "";
            COR_CARD = "";
        }

        public List<CardCoCriacao> BuscarCardsCoCriacao(string idIdeia, string idPergunta)
        {
            var lista = new List<CardCoCriacao>();
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = " select a.id,                                       " +
                              "        a.id_usuario,                               " +
                              "        a.id_projeto,                               " +
                              "        a.id_questao_ideias,                        " +
                              "        b.nome,                                     " +
                              "        a.resposta,                                 " +
                              "        a.data_hora,                                " +
                              "        a.cor_card                                  " +
                              "   from resposta_cocriadores a                      " +
                              "  inner join usuarios b on b.id = a.id_usuario      " +
                              "  where a.id_projeto = @idIdeia                     " +
                              "    and a.id_questao_ideias = @idPergunta           " +
                              "   order by a.id_questao_ideias                     ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);
                    cmd.Parameters.AddWithValue("@idPergunta", idPergunta);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new CardCoCriacao();
                        item.ID = dr["id"].ToString();
                        item.ID_IDEIA = dr["id_projeto"].ToString();
                        item.ID_QUESTAO_IDEIA = dr["id_questao_ideias"].ToString();
                        item.RESPOSTA = dr["resposta"].ToString();
                        item.DATA_HORA  = dr["data_hora"].ToString();
                        item.ID_USUARIO  = dr["id_usuario"].ToString();
                        item.NOME_USUARIO = dr["nome"].ToString();
                        item.COR_CARD = dr["cor_card"].ToString();
                        lista.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return lista;
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