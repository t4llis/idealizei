using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models.UserModels
{
    public class IdealModel
    {
        public string ID { get; set; }
        public string LETRA { get; set; }
        public Double PONTUACAO { get; set; }
        public string ORDEM { get; set; }
        public Int32 QNTDFEEDBACK { get; set; }
        public Double MEDIA_LETRA { get; set; }
        public string LEGENDA_GRAFICO { get; set; }


        public IdealModel()
        {
            ID = "0";
            LETRA = "";
            PONTUACAO = 0;
            ORDEM = "";
            QNTDFEEDBACK = 0;
            MEDIA_LETRA = 0;
            LEGENDA_GRAFICO = "";
        }


        public List<IdealModel> GetIdeal(string idUser)
        {
            List<IdealModel> lista = new List<IdealModel>();
            var db = new ClassDb();
            string sql = "";
            string idprojeto_old = "";
            Int32 qntdfeedback_old = 0;

            try
            {
                try
                {
                    sql = " select p.id,                                                                             " +
                          "        q.letra,                                                                          " +
                          "        sum(a.pontuacao) pontuacao,                                                       " +
                          "        q.ordem,                                                                          " +
                          "        l.legenda                                                                         " +
                          "        from                                                                              " +
                          "  avaliacao_ideias a                                                                      " +
                          "  inner join projetos p on p.id = a.id_projeto                                            " +
                          "  inner join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias          " +
                          "  inner join legenda_grafico l on l.id_questao_ideias = q.id_questao_ideias               " +
                          "  where p.id_usuario = @usuario                                                           " +
                          "    and q.ativo      = 'S'                                                                " +
                          " group by p.id,                                                                           " +
                          "       p.nome_projeto,                                                                    " +
                          "       q.letra                                                                            " +
                          " order by p.nome_projeto, q.id";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdealModel();
                        item.ID = dr["id"].ToString();
                        item.LETRA = dr["letra"].ToString();
                        item.PONTUACAO = Convert.ToDouble(dr["pontuacao"]);
                        item.ORDEM = dr["ordem"].ToString();
                        item.LEGENDA_GRAFICO = dr["LEGENDA"].ToString();
                        lista.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    
                }
                catch (Exception ex)
                {
                    throw ex;
                }


                try
                {
                    foreach (var item in lista)
                    {
                        if (idprojeto_old == "" || idprojeto_old != item.ID)
                        {
                            sql = " select count(distinct id_usuario_avaliador) as qtdFeedback " +
                                  "    from avaliacao_ideias                                   " +
                                  "    where id_projeto = @projeto                                  ";

                            MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                            cmd.Parameters.AddWithValue("@projeto", item.ID);

                            db.AbrirConexao();
                            MySqlDataReader dr;

                            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                            while (dr.Read())
                            {
                                item.QNTDFEEDBACK = Convert.ToInt32(dr["qtdFeedback"]);
                                idprojeto_old = item.ID;
                                qntdfeedback_old = Convert.ToInt32(dr["qtdFeedback"]);
                            }

                            cmd.Connection.Close();
                            db.FecharConexao();
                        }
                        else
                        {
                            item.QNTDFEEDBACK = qntdfeedback_old;
                        }
                    }

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

            List<IdealModel> totais = new List<IdealModel>();
            if (lista.Count > 0)
            {
                var letrai = new IdealModel();
                var letrad = new IdealModel();
                var letrae = new IdealModel();
                var letraa = new IdealModel();
                var letral = new IdealModel();

                foreach (var item in lista)
                {
                    if (item.LETRA == "I")
                    {
                        letrai.LETRA = item.LETRA;
                        letrai.PONTUACAO += item.PONTUACAO;
                        letrai.QNTDFEEDBACK += item.QNTDFEEDBACK;
                        letrai.LEGENDA_GRAFICO = item.LEGENDA_GRAFICO;

                    }
                    else if (item.LETRA == "D")
                    {
                        letrad.LETRA = item.LETRA;
                        letrad.PONTUACAO += item.PONTUACAO;
                        letrad.QNTDFEEDBACK += item.QNTDFEEDBACK;
                        letrad.LEGENDA_GRAFICO = item.LEGENDA_GRAFICO;

                    }
                    else if (item.LETRA == "E")
                    {
                        letrae.LETRA = item.LETRA;
                        letrae.PONTUACAO += item.PONTUACAO;
                        letrae.QNTDFEEDBACK += item.QNTDFEEDBACK;
                        letrae.LEGENDA_GRAFICO = item.LEGENDA_GRAFICO;

                    }
                    else if (item.LETRA == "A")
                    {
                        letraa.LETRA = item.LETRA;
                        letraa.PONTUACAO += item.PONTUACAO;
                        letraa.QNTDFEEDBACK += item.QNTDFEEDBACK;
                        letraa.LEGENDA_GRAFICO = item.LEGENDA_GRAFICO;

                    }
                    else if (item.LETRA == "L")
                    {
                        letral.LETRA = item.LETRA;
                        letral.PONTUACAO += item.PONTUACAO;
                        letral.QNTDFEEDBACK += item.QNTDFEEDBACK;
                        letral.LEGENDA_GRAFICO = item.LEGENDA_GRAFICO;
                    }

                }


                letrai.MEDIA_LETRA = (letrai.PONTUACAO / (letrai.QNTDFEEDBACK * 5));
                letrad.MEDIA_LETRA = (letrad.PONTUACAO / (letrad.QNTDFEEDBACK * 5));
                letrae.MEDIA_LETRA = (letrae.PONTUACAO / (letrae.QNTDFEEDBACK * 5));
                letraa.MEDIA_LETRA = (letraa.PONTUACAO / (letraa.QNTDFEEDBACK * 5));
                letral.MEDIA_LETRA = (letral.PONTUACAO / (letral.QNTDFEEDBACK * 5));

                totais.Add(letrai);
                totais.Add(letrad);
                totais.Add(letrae);
                totais.Add(letraa);
                totais.Add(letral);
            }
            return totais;
        }
    }
}