using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace WebApp.Models
{
    public class Avaliacao
    {
        public string ID_USUARIO_AVALIADOR { get; set; }
        public string NOME_AVALIADOR { get; set; }
        public DateTime DATA_AVALIACAO { get; set; }
        public double I { get; set; }
        public double D { get; set; }
        public double E { get; set; }
        public double A { get; set; }
        public double L { get; set; }
        public string OBSERVACAO { get; set; }        

        public Avaliacao()
        {
            ID_USUARIO_AVALIADOR = "";
            NOME_AVALIADOR = "";
            DATA_AVALIACAO = DateTime.Now;
            I = 0.0;
            D = 0.0;
            E = 0.0;
            A = 0.0;
            L = 0.0;
            OBSERVACAO = "";
        }

        public bool VerificaContratoAvaliacoes(int master_idUsuario)
        {
            var db = new ClassDb();
            var qtd = 0;
            try
            {
                try
                {
                    var sql = "select IFNULL((sum(pl.quantValidacoes) - ( " +
                        "select count(*) as qtdAvaliacoes from avaliacao_ideias as a where a.id in ( " +
                            "select p.id from projetos as p " +
                            "where p.master_idUsuario = @idUser " +
                        ")" +
                    ")), 0) as avaliacoes "+
                    "from ecom_contratos as c JOIN ecom_planos as pl ON c.idPlano = pl.idPlano AND c.idUsuario = @idUser AND c.status = 1";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idUser", master_idUsuario);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtd = Convert.ToInt32(dr["avaliacoes"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    if (qtd > 0)
                        return true;
                    else
                        return false;
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

        public List<Avaliacao> GetAllAvaliacaoFeedbacksByIdIdeia(string idIdeia)
        {
            var db = new ClassDb();
            List<Avaliacao> lista = new List<Avaliacao>();
            try
            {
                try
                {

                    var sql = "select                                                                                                   " +
                           "        avaliacoes.id_avaliador as id_usuario_avaliador,                                                    " +
                           "        avaliacoes.nome_avaliador as nome_avaliador,                                                        " +
                           "        avaliacoes.data_avaliacao as data_avaliacao,                                                        " +
	                       "        sum(I) as I,                                                                                        " +
                           "        sum(D) as D,                                                                                        " +
                           "        sum(E) as E,                                                                                        " +
                           "        sum(A) as A,                                                                                        " +
                           "        sum(L) as L                                                                                         " +
                           "    from                                                                                                    " +
                           "        (select                                                                                             " +                           
                           "            u.id as id_avaliador,                                                                           " +
                           "            a.data_hora as data_avaliacao,                                                                  " +
                           "            u.nome as nome_avaliador,                                                                       " +
                           "            case when q.letra = 'I' then sum(a.pontuacao) / count(q.id_questao_ideias) else 0 end as I,     " +
                           "            case when q.letra = 'D' then sum(a.pontuacao) / count(q.id_questao_ideias) else 0 end as D,     " +
                           "            case when q.letra = 'E' then sum(a.pontuacao) / count(q.id_questao_ideias) else 0 end as E,     " +
                           "            case when q.letra = 'A' then sum(a.pontuacao) / count(q.id_questao_ideias) else 0 end as A,     " +
                           "            case when q.letra = 'L' then sum(a.pontuacao) / count(q.id_questao_ideias) else 0 end as L      " +                           
                           "        from avaliacao_ideias a                                                                             " +                           
                           "        inner                                                                                               " +
                           "        join usuarios u on u.id = a.id_usuario_avaliador                                                    " +                           
                           "        inner                                                                                               " +
                           "        join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias                            " +
                           "                                                                                                            " +
                           "        where id_projeto = @id                                                                              " +
                           "          and q.ativo    = 'S'                                                                              " +
                           "        group by q.letra, u.id                                                                              " +
                           "        order by u.id, q.id_questao_ideias) avaliacoes                                                      " +                           
                           "    group by avaliacoes.id_avaliador                                                                         ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);

                    cmd.Parameters.AddWithValue("@id", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        Avaliacao avaliacao = new Avaliacao();
                        avaliacao.ID_USUARIO_AVALIADOR = dr["id_usuario_avaliador"].ToString();
                        avaliacao.NOME_AVALIADOR = dr["nome_avaliador"].ToString();
                        avaliacao.DATA_AVALIACAO = DateTime.Parse(dr["data_avaliacao"].ToString());
                        avaliacao.I = Math.Round(Convert.ToDouble(dr["I"]), 2);
                        avaliacao.D = Math.Round(Convert.ToDouble(dr["D"]), 2);
                        avaliacao.E = Math.Round(Convert.ToDouble(dr["E"]), 2);
                        avaliacao.A = Math.Round(Convert.ToDouble(dr["A"]), 2);
                        avaliacao.L = Math.Round(Convert.ToDouble(dr["L"]), 2);
                        lista.Add(avaliacao);
                    }
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

        public List<Avaliacao> BuscarComentariosAvaliacao(string idIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = " select distinct a.id_usuario_avaliador, u.nome, a.observacao " +
                              "   from avaliacao_ideias as a                                 " +
                              "  inner join usuarios u on u.id = a.id_usuario_avaliador      " +
                              "  where a.id_projeto = @id                                    " +
                              "    and a.observacao is not null                              " +
                              "    and a.observacao <> ''                                    " +
                              "    order by a.id                                             ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", idIdeia);
                    db.AbrirConexao();

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    List<Avaliacao> avaliacoes = new List<Avaliacao>();
                    while (dr.Read())
                    {
                        Avaliacao avaliacao = new Avaliacao();
                        avaliacao.ID_USUARIO_AVALIADOR = dr["id_usuario_avaliador"].ToString();
                        avaliacao.NOME_AVALIADOR = dr["nome"].ToString();
                        avaliacao.OBSERVACAO = dr["observacao"].ToString();
                        avaliacoes.Add(avaliacao);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return avaliacoes;
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