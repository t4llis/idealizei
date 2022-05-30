using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApp.Controllers;
using WebApp.Enum;
using WebApp.Utils;

namespace WebApp.Models
{
    public class IdeiaCards
    {
        public string ID { get; set; }
        public string IDHASH { get; set; }
        public string NOME_IDEIA { get; set; }
        public string LETRA { get; set; }
        public int PONTUACAO { get; set; }
        public string ORDEM { get; set; }
        public string TIPOIDEIA { get; set; }
        public string URLIMAGEM { get; set; }
        public string STATUS { get; set; }
        public double MEDIA { get; set; }
        public string LEGENDA_GRAFICO { get; set; }
        public int QTD_QUESTOES_AVALIACAO { get; set; }
        public DateTime DATA_CRIACAO { get; set; }

        public IdeiaCards()
        {
            ID = "";
            IDHASH = "";
            NOME_IDEIA = "";
            LETRA = "";
            PONTUACAO = 0;
            ORDEM = "";
            TIPOIDEIA = "";
            URLIMAGEM = "";
            STATUS = "";
            MEDIA = 0;
            LEGENDA_GRAFICO = "";
            QTD_QUESTOES_AVALIACAO = 0;
            DATA_CRIACAO = DateTime.Now;
        }

        public List<IdeiaCards> BuscarIdeiasConcluidas(string idUsuarioLogado)
        {

            var db = new ClassDb();
            var Ideias = new List<IdeiaCards>();
            try
            {
                try
                {
                    var sql = " select p.id as id_projeto,                                                                                                                               " +
                              "        p.nome_projeto,                                                                                                                                   " +
                              "        p.lider,                                                                                                                                          " +
                              "        p.tipo_projeto,                                                                                                                                   " +
                              "        c.hash                                                                                                                                            " +
                              "  from projetos p,                                                                                                                                        " +
                              "       hash_registro c                                                                                                                                    " +
                              " where ((p.id_usuario = @usuario) or (                                                                                                                    " +
                              "        (p.id in (select distinct id_projeto from resposta_cocriadores where id_usuario = @usuario)) or                                                   " +
                              "        (p.id in (select distinct id_projeto from ideias_cocriadores where email_cocriador in (select distinct email from usuarios where id = @usuario) ))" +
                              "       ))                                                                                                                                                 " +
                              "   and p.id     = c.id_registro                                                                                                                           " +
                              "   and p.status = 2                                                                                                                                       " +
                              " order by p.data_hora desc                                                                                                                                ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUsuarioLogado);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdeiaCards();
                        item.ID = dr["id_projeto"].ToString();
                        item.IDHASH = dr["hash"].ToString();
                        item.NOME_IDEIA = dr["nome_projeto"].ToString();
                        item.TIPOIDEIA = dr["tipo_projeto"].ToString();
                        item.URLIMAGEM = "/images/icon_pages.png";                        

                        Ideias.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    
                    // calcula media das ideias, ja que foram concluidas...
                    AvaliacaoEnum avaliacaoEnum = AvaliacaoEnum.FEEDBACK;
                    List<Ideia> listaIdeiasComMediaAtualizada = new AvaliacaoController().GetlistaIdeiasComMediaAtualizadaComFeedbacks(avaliacaoEnum, idUsuarioLogado);
                    foreach (var itemLista in listaIdeiasComMediaAtualizada)
                    {
                        int posicao = Ideias.FindIndex(x => x.ID.ToString() == itemLista.IDPROJETO.ToString());
                        if(posicao != -1)
                            Ideias[posicao].MEDIA = itemLista.MEDIA;
                    }
                    
                    return Ideias;
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

        public List<IdeiaCards> BuscarIdeiasEmAndamento(string email, string idUser)
        {
            var db = new ClassDb();
            var Lista = new List<IdeiaCards>();
            try
            {
                try
                {
                    var sql = " select distinct a.id, c.hash, a.nome_projeto, a.id_usuario, a.tipo_projeto, a.data_hora   " +
                              "   from projetos a,                                                                        " +
                              "        hash_registro c                                                                    " +
                              "  where (a.id in (select id_projeto                                                        " +
                              "                    from ideias_cocriadores                                                " +
                              "                   where email_cocriador = @user_email                                     " +
                              "                     and id_projeto = a.id) or (a.id_usuario = @user_logado))              " +
                              "    and a.id     = c.id_registro                                                           " +
                              "    and a.status = 1                                                                       " +
                              "  order by a.data_hora desc                                                                ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@user_email", email);
                    cmd.Parameters.AddWithValue("@user_logado", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdeiaCards();
                        item.ID = dr["ID"].ToString();
                        item.IDHASH = dr["hash"].ToString();
                        item.NOME_IDEIA = dr["NOME_PROJETO"].ToString();
                        item.TIPOIDEIA = dr["tipo_projeto"].ToString();
                        item.DATA_CRIACAO = DateTime.Parse(dr["data_hora"].ToString());
                        item.URLIMAGEM = "/images/icon_pages.png";
                        
                        Lista.Add(item);

                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Lista;
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