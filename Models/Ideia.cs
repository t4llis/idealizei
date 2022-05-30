using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApp.Enum;
using WebApp.Utils;

namespace WebApp.Models
{
    public class Ideia
    {
        public int ID { get; set; }
        public string NOMEPROJETO { get; set; }
        public string TIPOPROJETO { get; set; }
        public string LIDER { get; set; }
        public string ID_USUARIO { get; set; }
        public string TEXTOIDEIAINOVADORA { get; set; }
        public string TEXTOPROBLEMAS { get; set; }
        public string TEXTOPRATICA { get; set; }
        public string TEXTORESULTADOS { get; set; }
        public string TEXTOIMPACTO { get; set; }
        public string LINKPIT { get; set; }
        public string TITULO { get; set; } /*Codigo provisorio, ajustar depois que ajustarmos os cadastros */
        public int IDQUESTAOAVALIACAOIDEIA { get; set; }  /*Codigo provisorio, ajustar depois que ajustarmos os cadastros */
        public int PONTUACAO { get; set; }  /*Codigo provisorio, ajustar depois que ajustarmos os cadastros */
        public int ID_QUESTAO_IDEIAS { get; set; }  /*Codigo provisorio, ajustar depois que ajustarmos os cadastros */
        public int IDPROJETO { get; set; } /*Codigo provisorio, ajustar depois que ajustarmos os cadastros */
        public double MEDIA { get; set; }
        public double MEDIA_QUESTAO_IDEIA { get; set; }
        public double MEDIA_QUESTAO_AVALIACAO_IDEIA { get; set; }
        public string STATUS { get; set; }
        public int QTD_FEEDBACKS { get; set; }
        public string TEXTOIDEIA { get; set; }
        public string OBSERVACAO { get; set; }
        public int IDTAGS { get; set; }
        public string SUBTITULO { get; set; }

        public Ideia()
        {
            ID = 0;
            NOMEPROJETO = "";
            TIPOPROJETO = "";
            LIDER = "";
            ID_USUARIO = "";
            TEXTOIDEIAINOVADORA = "";
            TEXTOPROBLEMAS = "";
            TEXTOPRATICA = "";
            TEXTORESULTADOS = "";
            TEXTOIMPACTO = "";
            LINKPIT = "";
            TITULO = "";
            IDQUESTAOAVALIACAOIDEIA = 0;
            PONTUACAO = 0;
            ID_QUESTAO_IDEIAS = 0;
            IDPROJETO = 0;
            MEDIA = 0.0;
            STATUS = "0";
            QTD_FEEDBACKS = 0;
            TEXTOIDEIA = "";
            OBSERVACAO = "";
            IDTAGS = 0;
            SUBTITULO = "";
        }

        public bool VerificaContratoIdeias(int master_idUsuario)
        {
            var db = new ClassDb();
            var qtd = 0;
            try
            {
                try
                {
                    var sql = "select IFNULL(sum(p.quantIdeias) - (select count(*) as qtdProjetos from projetos where master_idUsuario = @idUser), 0) as ideias " +
                    "from ecom_contratos as c JOIN ecom_planos as p ON c.idPlano = p.idPlano AND c.idUsuario = @idUser AND c.status = 1";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idUser", master_idUsuario);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtd = Convert.ToInt32(dr["ideias"]);
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

        public long CadastrarIdeia()
        {
            var db = new ClassDb();
            long idprojeto = 0;
            try
            {
                try
                {

                    var sql = "INSERT INTO projetos (nome_projeto, id_usuario, tipo_projeto, data_hora, linkpit, lider, status, master_idusuario) " +
                                "               VALUES(@nome_projeto, @id_usuario, @tipo_projeto, @data_hora, @linkpit, @lider, @status, @master_idusuario) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nome_projeto", this.NOMEPROJETO);
                        cmd.Parameters.AddWithValue("@id_usuario", HttpContext.Current.Session["Id"].ToString());
                        cmd.Parameters.AddWithValue("@tipo_projeto", this.TIPOPROJETO);
                        cmd.Parameters.AddWithValue("@linkpit", this.LINKPIT);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@lider", HttpContext.Current.Session["Nome"].ToString());
                        cmd.Parameters.AddWithValue("@status", this.STATUS);
                        cmd.Parameters.AddWithValue("@master_idusuario", HttpContext.Current.Session["master_idUsuario"].ToString());

                        cmd.ExecuteNonQuery();
                        idprojeto = cmd.LastInsertedId;
                        cmd.Connection.Close();
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

            try
            {

                cadastrarPergustasAvaliacao(idprojeto, HttpContext.Current.Session["Email"].ToString());
                HttpContext.Current.Session["IdIdeiaCadastrada"] = idprojeto;
                return idprojeto;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public List<IdeiaTotais> GetTotaisIdeia(string idUser, string email)
        {
            var Ideias = new List<IdeiaTotais>();
            var db = new ClassDb();
            try
            {
                try
                {
                    #region [TOTAIS PEDIDO]

                    var sql = "SELECT count(*)QTD FROM projetos WHERE id_usuario = @usuario and status = " + Convert.ToInt32(StatusEnum.AVALIACAO);
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        total1.TIPO = "PROJETO";
                        total1.VALOR = dr["QTD"].ToString();
                    }
                    Ideias.Add(total1);
                    cmd.Connection.Close();
                    db.FecharConexao();
                    #endregion

                    #region [TOTAIS FEEDBACKS]
                    sql = "SELECT count(distinct id_usuario_avaliador, id_projeto) QTD     " +
                          "  FROM avaliacao_ideias                            " +
                          " WHERE id_projeto IN(SELECT id FROM projetos WHERE id_usuario = @usuario)";

                    cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);

                    db.AbrirConexao();

                    var total3 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        total3.TIPO = "FEEDBACKS";
                        total3.VALOR = dr["QTD"].ToString();
                    }
                    Ideias.Add(total3);
                    cmd.Connection.Close();
                    db.FecharConexao();

                    #endregion

                    #region [TOTAIS LABORATORIO]
                    sql = " select count(distinct a.id) QTD                                         " +
                          "   from projetos a,                                                      " +
                          "        ideias_cocriadores b                                             " +
                          "  where ( (b.email_cocriador = @user_email and a.id = b.id_projeto) or   " +
                          "          (a.id_usuario = @user_logado) )                                " +
                          "    and a.status = 1                                                     " +
                          "  order by a.data_hora desc                                              ";

                    cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@user_email", email);
                    cmd.Parameters.AddWithValue("@user_logado", idUser);

                    db.AbrirConexao();

                    var total2 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        total2.TIPO = "LABORATORIO";
                        total2.VALOR = dr["QTD"].ToString();
                    }
                    Ideias.Add(total2);
                    cmd.Connection.Close();
                    db.FecharConexao();

                    #endregion

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
                
        public List<Ideia> BuscarIdeiasAvaliar(string idUser)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select distinct p.id,                                 " +
                              "                 p.nome_projeto,                        " +
                              "                 p.tipo_projeto                        " +
                              "  from cadastro_ideias c                               " +
                              "  inner join projetos p on p.id = c.id_projeto         " +
                              "  inner join usuarios u on u.email = c.email_avaliador " +
                              "  where u.id = @usuario                                " +
                              "  and p.id_usuario <> @usuario                         " +
                              "  and p.id not in (select distinct id_projeto          " +
                              "                      from avaliacao_ideias            " +
                              "                   where id_usuario_avaliador = @usuario) " +
                              "  order by p.data_hora desc                            ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.ID = Convert.ToInt32(dr["ID"]);
                        item.NOMEPROJETO = dr["NOME_PROJETO"].ToString();
                        item.TIPOPROJETO = dr["tipo_projeto"].ToString();
                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public List<Ideia> BuscarIdeias(string idProjeto)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select c.id as id_cadastro_ideia,                           " +
                              "        p.id as id_projeto,                                  " +
                              "        p.nome_projeto,                                      " +
                              "        p.lider,                                             " +
                              "        c.id_questao_ideias,                                 " +
                              "        c.resposta,                                          " +
                              "        q.titulo,                                            " +
                              "        p.linkpit,                                           " +
                              "        q.ordem                                              " +
                              "  from cadastro_ideias c                                     " +
                              "  inner join projetos p on p.id = c.id_projeto               " +
                              "  inner join questoes_ideias q on q.id = c.id_questao_ideias " +
                              "  where p.id = @projeto                                      " +
                              "    and trim(c.resposta) <> ''                               " +
                              "  group by  p.id,                                            " +
                              "            p.nome_projeto,                                  " +
                              "            p.lider,                                         " +
                              "            c.id_questao_ideias,                             " +
                              "            c.resposta,                                      " +
                              "            q.titulo,                                        " +
                              "            p.linkpit,                                       " +
                              "            q.ordem                                          " +    
                              "  order by q.ordem asc                                       ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idProjeto);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.ID = Convert.ToInt32(dr["id_cadastro_ideia"]);
                        item.IDPROJETO = Convert.ToInt32(dr["id_projeto"]);
                        item.NOMEPROJETO = dr["nome_projeto"].ToString();
                        item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString(); // Codigo provisorio, ajustar depois que ajustarmos os cadastros
                        item.TITULO = dr["titulo"].ToString();
                        item.LINKPIT = dr["linkpit"].ToString();
                        item.LIDER = dr["lider"].ToString();
                        item.ID_QUESTAO_IDEIAS = Convert.ToInt32(dr["id_questao_ideias"]); // Codigo provisorio, ajustar depois que ajustarmos os cadastros

                        // Codigo abaixo provisorio, ajustar depois que ajustarmos os cadastros
                        switch (dr["ORDEM"].ToString())
                        {
                            case "1":
                                {
                                    item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString();
                                    break;
                                }
                            case "2":
                                {
                                    item.TEXTOPROBLEMAS = dr["resposta"].ToString();
                                    break;
                                }
                            case "3":
                                {
                                    item.TEXTOPRATICA = dr["resposta"].ToString();
                                    break;
                                }
                            case "4":
                                {
                                    item.TEXTORESULTADOS = dr["resposta"].ToString();
                                    break;
                                }
                            case "5":
                                {
                                    item.TEXTOIMPACTO = dr["resposta"].ToString();
                                    break;
                                }
                        }

                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public List<Ideia> QuestoesAvaliacaoIdeias(string idIdeia)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select a.id as id_questao_avaliacao_ideia,                            " +
                              "        q.id as id_questao_ideia,                                      " +
                              "        p.nome_projeto,                                                " +
                              "        p.id as id_projeto,                                            " +
                              "        c.resposta,                                                    " +
                              "        q.titulo,                                                      " +
                              "        a.questao,                                                     " +
                              "        p.lider                                                        " +
                              "  from cadastro_ideias c                                               " +
                              "  inner join projetos p on p.id = c.id_projeto                         " +
                              "  inner join questoes_ideias q on q.id = c.id_questao_ideias           " +
                              "  inner join questoes_avaliacao_ideias a on a.id_questao_ideias = q.id " +
                              "  where c.id = @ideia                                                  " +
                              "    and a.ativo = 'S'                                                  " +
                              "  order by a.ordem asc                                                 ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@ideia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.IDQUESTAOAVALIACAOIDEIA = Convert.ToInt32(dr["id_questao_avaliacao_ideia"]);
                        item.ID_QUESTAO_IDEIAS = Convert.ToInt32(dr["id_questao_ideia"]);
                        item.NOMEPROJETO = dr["nome_projeto"].ToString();
                        item.IDPROJETO = Convert.ToInt32(dr["id_projeto"]);
                        item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString(); // Codigo provisorio, ajustar depois que ajustarmos os cadastros
                        item.TITULO = dr["titulo"].ToString();
                        item.TEXTOPRATICA = dr["questao"].ToString(); // Codigo provisorio, ajustar depois que ajustarmos os cadastros
                        item.LIDER = dr["lider"].ToString();

                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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

        public List<Ideia> BuscaTodasQuestoesAvaliacaoIdeias()
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select q.id id_questao_avaliacao_ideia, " +
                                " q.questao questao, " +
                                " q.id_questao_ideias id_questao_ideia, " +
                                " q.subtitulo subtitulo                 " +
                                " from questoes_avaliacao_ideias q where q.ativo = 'S' ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.IDQUESTAOAVALIACAOIDEIA = Convert.ToInt32(dr["id_questao_avaliacao_ideia"]);
                        item.TEXTOPRATICA = dr["questao"].ToString();
                        item.ID_QUESTAO_IDEIAS = Convert.ToInt32(dr["id_questao_ideia"]);
                        item.SUBTITULO = dr["subtitulo"].ToString();

                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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

        public List<RepostaCocriador> BuscarRespostas(string idIdeia, string idQuestaoIdeia = "0")
        {
            var db = new ClassDb();
            var repostas = new List<RepostaCocriador>();
            try
            {
                try
                {
                    var sql = " select *                     " +
                              "   from resposta_cocriadores  " +
                              "  where id_projeto = @projeto ";

                    if (idQuestaoIdeia != "0")
                        sql += " and id_questao_ideias = @idQuestaoIdeia";

                    sql += "  order by id_questao_ideias  ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idIdeia);
                    if (idQuestaoIdeia != "0")
                        cmd.Parameters.AddWithValue("@idQuestaoIdeia", idQuestaoIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new RepostaCocriador();
                        item.ID = Convert.ToInt32(dr["ID"]);
                        item.DATA_HORA = Convert.ToDateTime(dr["DATA_HORA"]);
                        item.RESPOSTA = dr["RESPOSTA"].ToString();
                        item.ID_PROJETO = Convert.ToInt32(dr["ID_PROJETO"]);
                        item.ID_QUESTAO_IDEIAS = Convert.ToInt32(dr["ID_QUESTAO_IDEIAS"]);
                        item.ID_USUARIO = Convert.ToInt32(dr["ID_USUARIO"]);
                        item.NOME_USUARIO = new User().GetUsuario(item.ID_USUARIO.ToString()).NOME;
                        item.COR_CARD = dr["COR_CARD"].ToString();
                        repostas.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return repostas;
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
        public long CadastrarAvaliacao(List<Ideia> listIdeia, string idUsuario)
        {
            var db = new ClassDb();
            long idAvaliacao = 0;
            int idProjeto = listIdeia[0].IDPROJETO;

            try
            {
                try
                {
                    foreach (var ideia in listIdeia)
                    {
                        var pontuacao = ideia.PONTUACAO;
                        var idquestaoavaliacaoideia = ideia.IDQUESTAOAVALIACAOIDEIA;
                        var observacao = ideia.OBSERVACAO;
                        var idtags = ideia.IDTAGS;

                        // AVALIAÇÃO
                        var sql = "INSERT INTO avaliacao_ideias (data_hora, observacao, pontuacao, id_projeto, id_questao_avaliacao_ideias, id_usuario_avaliador, id_tags) " +
                                    "               VALUES(@data_hora, @observacao, @pontuacao, @id_projeto, @id_questao_avaliacao_ideias, @id_usuario_avaliador, @id_tags) ";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                            cmd.Parameters.AddWithValue("@observacao", observacao);
                            cmd.Parameters.AddWithValue("@pontuacao", pontuacao);
                            cmd.Parameters.AddWithValue("@id_projeto", idProjeto);
                            cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                            cmd.Parameters.AddWithValue("@id_questao_avaliacao_ideias", idquestaoavaliacaoideia);
                            cmd.Parameters.AddWithValue("@id_usuario_avaliador", idUsuario);
                            cmd.Parameters.AddWithValue("@id_tags", idtags);
                            cmd.ExecuteNonQuery();
                            idAvaliacao = cmd.LastInsertedId;
                            cmd.Connection.Close();
                        }
                    }

                    return idAvaliacao;
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

        public List<Ideia> BuscarIdeiasCoCriador(string idProjeto, string idQuestaoIdeia = "0")
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select p.id,                                                " +
                              "        p.nome_projeto,                                      " +
                              "        p.lider,                                             " +
                              "        p.id_usuario,                                        " +
                              "        c.id_questao_ideias,                                 " +
                              "        c.resposta,                                          " +
                              "        q.titulo,                                            " +
                              "        p.linkpit,                                           " +
                              "        q.ordem                                              " +
                              "  from cadastro_ideias c                                     " +
                              "  inner join projetos p on p.id = c.id_projeto               " +
                              "  inner join questoes_ideias q on q.id = c.id_questao_ideias " +
                              "  where p.id = @projeto                                      ";

                    if (idQuestaoIdeia != "0")
                        sql += " and q.id = @idQuestaoIdeia";
                    sql += "  order by q.ordem asc                                       ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idProjeto);

                    if (idQuestaoIdeia != "0")
                        cmd.Parameters.AddWithValue("@idQuestaoIdeia", idQuestaoIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.ID = Convert.ToInt32(dr["ID"]);
                        item.NOMEPROJETO = dr["NOME_PROJETO"].ToString();
                        item.ID_USUARIO = dr["id_usuario"].ToString();
                        item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString(); // Codigo provisorio, ajustar depois que ajustarmos os cadastros
                        item.TITULO = dr["titulo"].ToString();
                        item.LINKPIT = dr["linkpit"].ToString();
                        item.LIDER = dr["lider"].ToString();
                        item.ID_QUESTAO_IDEIAS = Convert.ToInt32(dr["id_questao_ideias"]); // Codigo provisorio, ajustar depois que ajustarmos os cadastros

                        // Codigo abaixo provisorio, ajustar depois que ajustarmos os cadastros
                        switch (dr["ORDEM"].ToString())
                        {
                            case "1":
                                {
                                    item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString();
                                    break;
                                }
                            case "2":
                                {
                                    item.TEXTOPROBLEMAS = dr["resposta"].ToString();
                                    break;
                                }
                            case "3":
                                {
                                    item.TEXTOPRATICA = dr["resposta"].ToString();
                                    break;
                                }
                            case "4":
                                {
                                    item.TEXTORESULTADOS = dr["resposta"].ToString();
                                    break;
                                }
                            case "5":
                                {
                                    item.TEXTOIMPACTO = dr["resposta"].ToString();
                                    break;
                                }
                        }

                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public List<Ideia> BuscarIdeiasCoCriador(string user_email, long idUser)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select distinct a.id, a.nome_projeto, a.id_usuario, a.tipo_projeto      " +
                              "   from projetos a,                                                      " +
                              "        ideias_cocriadores b                                             " +
                              "  where ( (b.email_cocriador = @user_email and a.id = b.id_projeto) or   " +
                              "          (a.id_usuario = @user_logado) )                                " +
                              "    and a.status = 1                                                     " +
                              "  order by a.data_hora desc                                              ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@user_email", user_email);
                    cmd.Parameters.AddWithValue("@user_logado", idUser.ToString());

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.ID = Convert.ToInt32(dr["ID"]);
                        item.NOMEPROJETO = dr["NOME_PROJETO"].ToString();
                        item.TIPOPROJETO = dr["tipo_projeto"].ToString();
                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public List<Ideia> BuscarCardIdeiaCoCriador(string idProjeto, string idQuestaoIdeia)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select p.id,                                                " +
                              "        p.nome_projeto,                                      " +
                              "        p.lider,                                             " +
                              "        c.id_questao_ideias,                                 " +
                              "        c.resposta,                                          " +
                              "        q.titulo,                                            " +
                              "        p.linkpit,                                           " +
                              "        q.ordem                                              " +
                              "  from cadastro_ideias c                                     " +
                              "  inner join projetos p on p.id = c.id_projeto               " +
                              "  inner join questoes_ideias q on q.id = c.id_questao_ideias " +
                              "  where p.id = @projeto                                      " +
                              "    and q.id = @QuestaoIdeia                                 " +
                              "  order by q.ordem asc                                       ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idProjeto);
                    cmd.Parameters.AddWithValue("@QuestaoIdeia", idQuestaoIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.ID = Convert.ToInt32(dr["ID"]);
                        item.NOMEPROJETO = dr["NOME_PROJETO"].ToString();
                        item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString(); // Codigo provisorio, ajustar depois que ajustarmos os cadastros
                        item.TITULO = dr["titulo"].ToString();
                        item.LINKPIT = dr["linkpit"].ToString();
                        item.LIDER = dr["lider"].ToString();
                        item.ID_QUESTAO_IDEIAS = Convert.ToInt32(dr["id_questao_ideias"]); // Codigo provisorio, ajustar depois que ajustarmos os cadastros

                        // Codigo abaixo provisorio, ajustar depois que ajustarmos os cadastros
                        switch (dr["ORDEM"].ToString())
                        {
                            case "1":
                                {
                                    item.TEXTOIDEIAINOVADORA = dr["resposta"].ToString();
                                    break;
                                }
                            case "2":
                                {
                                    item.TEXTOPROBLEMAS = dr["resposta"].ToString();
                                    break;
                                }
                            case "3":
                                {
                                    item.TEXTOPRATICA = dr["resposta"].ToString();
                                    break;
                                }
                            case "4":
                                {
                                    item.TEXTORESULTADOS = dr["resposta"].ToString();
                                    break;
                                }
                            case "5":
                                {
                                    item.TEXTOIMPACTO = dr["resposta"].ToString();
                                    break;
                                }
                        }

                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public bool AddDescricaoCardCoCriador(CardCoCriacao param, string[] coresCard)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    string corCard = this.buscaCorCardCocriador(HttpContext.Current.Session["Id"].ToString(), param.ID_IDEIA);

                    if (corCard == null || corCard == "")
                    {
                        corCard = this.buscaNovaCorCardCocriador(param.ID_IDEIA, coresCard);
                    }

                    // PROJETO
                    var sql = "INSERT INTO resposta_cocriadores (resposta, data_hora, id_usuario, id_projeto, id_questao_ideias, cor_card) " +
                                "               VALUES(@resposta, @data_hora, @id_usuario, @id_projeto, @id_questao_ideias, @cor_card) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@resposta", param.RESPOSTA);
                        cmd.Parameters.AddWithValue("@id_usuario", HttpContext.Current.Session["Id"].ToString());
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@id_projeto", param.ID_IDEIA);
                        cmd.Parameters.AddWithValue("@id_questao_ideias", param.ID_QUESTAO_IDEIA);
                        cmd.Parameters.AddWithValue("@cor_card", corCard);
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

        public bool EditDescricaoCardCoCriador(CardCoCriacao param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    // PROJETO
                    var sql = "update resposta_cocriadores set resposta = @resposta where id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@resposta", param.RESPOSTA);
                        cmd.Parameters.AddWithValue("@id", param.ID);
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

        public bool DeleteDescricaoCardCoCriador(CardCoCriacao param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    // PROJETO
                    var sql = "delete from resposta_cocriadores where id = @id and id_usuario = @id_usuario";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", param.ID);
                        cmd.Parameters.AddWithValue("@id_usuario", HttpContext.Current.Session["Id"].ToString());
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

        public string buscaCorCardCocriador(string idUsuario, string idIdeia)
        {
            var db = new ClassDb();
            string corCard = "";
            try
            {
                try
                {
                    var sql = "select distinct cor_card           " +
                              "  from resposta_cocriadores a      " +
                              " where a.id_usuario = @id_usuario  " +
                              " and a.id_projeto = @id_projeto    ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_usuario", idUsuario);
                    cmd.Parameters.AddWithValue("@id_projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        corCard = dr["cor_card"].ToString();
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return corCard;
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

        public string buscaNovaCorCardCocriador(string idIdeia, string[] coresCard)
        {
            var db = new ClassDb();
            var listaCorCard = new List<string>();
            string corCard = "";
            try
            {
                try
                {
                    var sql = "select distinct cor_card           " +
                              "  from resposta_cocriadores a      " +
                              " where a.id_projeto = @id_projeto  ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        corCard = dr["cor_card"].ToString();
                        listaCorCard.Add(corCard);
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    corCard = getNovaCorCard(listaCorCard, coresCard);

                    return corCard;
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

        private string getNovaCorCard(List<string> listaCorCard, string[] coresCard)
        {
            List<string> copiaCoresCard = coresCard.ToList();

            foreach (var cor in listaCorCard)
            {
                copiaCoresCard.Remove(cor);
            }

            return copiaCoresCard[0];
        }

        public string BuscarNomesCoCriadores(string idIdeia)
        {
            var db = new ClassDb();
            string nomes = "";
            try
            {
                try
                {
                    var sql = "  select distinct b.nome                                      " +
                              "    from usuarios b,                                          " +
                              "         ideias_cocriadores c                                 " +
                              "   where b.email = c.email_cocriador                          " +
                              "     and c.id_projeto = @id_ideia                             " +
                              "  union                                                       " +
                              "  select distinct c.nome                                      " +
                              "    from projetos a                                           " +
                              "   inner join resposta_cocriadores b on b.id_projeto = a.id   " +
                              "   inner join usuarios c            on c.id = b.id_usuario    " +
                              "   where a.id = @id_ideia                                     " +
                              "   order by nome                                              ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_ideia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        nomes += dr["nome"].ToString() + ", ";
                    }
                    if (nomes.Length > 0)
                        nomes = nomes.Substring(0, nomes.Length - 2);

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return nomes;
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
        public List<Ideia> BuscarIdeiasConcluidas(string idUsuario)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select p.id as id_projeto,                                                                                                                               " +
                              "        p.nome_projeto,                                                                                                                                   " +
                              "        p.lider,                                                                                                                                          " +
                              "        p.tipo_projeto                                                                                                                                    " +
                              "  from projetos p                                                                                                                                         " +
                              " where ((p.id_usuario = @usuario) or (                                                                                                                    " +
                              "        (p.id in (select distinct id_projeto from resposta_cocriadores where id_usuario = @usuario)) or                                                   " +
                              "        (p.id in (select distinct id_projeto from ideias_cocriadores where email_cocriador in (select distinct email from usuarios where id = @usuario) ))" +
                              "       ))                                                                                                                                                 " +
                              "   and p.status = 2                                                                                                                                       " +
                              " order by p.data_hora desc                                                                                                                                ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUsuario);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.ID = Convert.ToInt32(dr["id_projeto"]);
                        item.NOMEPROJETO = dr["nome_projeto"].ToString();
                        item.LIDER = dr["lider"].ToString();
                        item.TIPOPROJETO = dr["tipo_projeto"].ToString();

                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public List<IdeiaCards> BuscarInfoCardsIdeiasAvaliadas(string idUser, AvaliacaoEnum avaliacaoEnum, string idIdeia)
        {
            var db = new ClassDb();
            var InfoCards = new List<IdeiaCards>();
            try
            {
                try
                {
                    var sql = " select p.id,                                                                    " +
                              " p.nome_projeto,                                                                 " +
                              " q.letra,                                                                        " +
                              " sum(a.pontuacao)pontuacao,                                                      " +
                              " b.hash,                                                                         " +
                              " q.ordem,                                                                        " +
                              " l.legenda,                                                                      " +
                              " count(q.id) qtd_questoes_avaliacao                                              " +
                              "  from avaliacao_ideias a                                                        " +
                              "  inner join projetos p on p.id                  = a.id_projeto                  " +
                              "  inner join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias " +
                              "  inner join hash_registro b on b.id_registro    = p.id                          " +
                              "  inner join legenda_grafico l on l.id_questao_ideias = q.id_questao_ideias      ";

                    if (idIdeia == "0")
                    {
                        if (avaliacaoEnum == AvaliacaoEnum.AVALIADOR)
                        {
                            sql += " where a.id_usuario_avaliador = @usuario                        ";
                        }
                        else
                        {
                            sql += " where p.id_usuario = @usuario                                  ";
                        }
                    }
                    else
                    {
                        sql += " where p.id = @idIdeia                                                ";
                    }

                    sql += " group by p.id,                                                           " +
                    "              p.nome_projeto,                                                    " +
                    "              q.letra                                                            " +
                    "  order by p.nome_projeto,                                                       " +
                    "              q.id                                                               ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdeiaCards();
                        item.ID = dr["ID"].ToString();
                        item.IDHASH = dr["HASH"].ToString();  //new Ideia().GetHashId(item.ID);
                        item.NOME_IDEIA = dr["NOME_PROJETO"].ToString();
                        item.LETRA = dr["LETRA"].ToString();
                        item.PONTUACAO = Convert.ToInt32(dr["PONTUACAO"]);
                        item.ORDEM = dr["ORDEM"].ToString();
                        item.LEGENDA_GRAFICO = dr["LEGENDA"].ToString();
                        item.QTD_QUESTOES_AVALIACAO = Convert.ToInt32(dr["QTD_QUESTOES_AVALIACAO"]);
                        InfoCards.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return InfoCards;
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
        public int BuscarQntdCardsIdeiasAvaliadas(string idUser, AvaliacaoEnum avaliacaoEnum)
        {
            var db = new ClassDb();
            int contador = 0;
            try
            {
                try
                {
                    var sql = " select distinct p.id                                                            " +
                              "  from avaliacao_ideias a                                                        " +
                              "  inner join projetos p on p.id = a.id_projeto                                   " +
                              "  inner join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias ";

                    if (avaliacaoEnum == AvaliacaoEnum.AVALIADOR)
                    {
                        sql += " where a.id_usuario_avaliador = @usuario                               ";
                    }
                    else
                    {
                        sql += " where p.id_usuario = @usuario                                         ";
                    }

                    sql += " group by p.id,                                                           " +
                    "              p.nome_projeto,                                                    " +
                    "              q.letra                                                            " +
                    "  order by p.nome_projeto,                                                       " +
                    "              q.ordem                                                            ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total1 = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        contador++;
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return contador;
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
        public List<Ideia> BuscarMediaPontuacaoIdeiasAvaliadas(string idUser, AvaliacaoEnum avaliacaoEnum, string idIdeia)
        {
            var db = new ClassDb();
            var Ideia = new List<Ideia>();
            try
            {
                try
                {
                    var sql = " select id_ideia,                                                                    " +
                              "        pontuacao / total as media                                                   " +
                              "  from                                                                               " +
                              "   (select p.id as id_ideia,                                                         " +
                              "         sum(a.pontuacao) as pontuacao                                               " +
                              "     from avaliacao_ideias a                                                         " +
                              "      inner join projetos p on p.id = a.id_projeto                                   " +
                              "      inner join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias ";

                    if (idIdeia == "0")
                    {
                        if (avaliacaoEnum == AvaliacaoEnum.AVALIADOR)
                        {
                            sql += " where a.id_usuario_avaliador = @usuario                        ";
                        }
                        else
                        {
                            sql += " where p.id_usuario = @usuario                                  ";
                        }
                    }
                    else
                    {
                        sql += " where p.id = @idIdeia                                                ";
                    }

                    sql += " group by p.id                                                                 " +
                     "     order by p.nome_projeto,                                                        " +
                     "               q.id) as pontuacao,                                                " +
                     "  (select count(*) as total from questoes_avaliacao_ideias) as total                ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", idUser);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Ideia();
                        item.IDPROJETO = Convert.ToInt32(dr["id_ideia"]);
                        item.MEDIA = Convert.ToDouble(dr["media"]);
                        Ideia.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public Ideia BuscarMediaPontuacaoPorIdQuestaoIdeia(string idIdeia, int idQuestaoIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = " select id_ideia,                                                                    " +
                              "        pontuacao / total as media                                                   " +
                              "  from                                                                               " +
                              "   (select p.id as id_ideia,                                                         " +
                              "         sum(a.pontuacao) as pontuacao                                               " +
                              "     from avaliacao_ideias a                                                         " +
                              "      inner join projetos p on p.id = a.id_projeto                                   " +
                              "      inner join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias " +
                              "   where p.id = @idIdeia                                                             " +
                              "   and q.id_questao_ideias = @idQuestaoIdeia) as pontuacao,                          " +
                              " (select count(*) as total from questoes_avaliacao_ideias                            " +
                              "     where id_questao_ideias = @idQuestaoIdeia) as total                             ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idQuestaoIdeia", idQuestaoIdeia);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    var Ideia = new Ideia();
                    while (dr.Read())
                    {
                        Ideia.IDPROJETO = Convert.ToInt32(dr["id_ideia"]);
                        Ideia.MEDIA_QUESTAO_IDEIA = Convert.ToDouble(dr["media"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public void CadastrarEmailCocriadores(string Email, string idIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO ideias_cocriadores (data_hora, email_cocriador, id_projeto)    " +
                              "                         VALUES(@data_hora, @email_cocriador, @id_projeto) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@id_projeto", idIdeia);
                        cmd.Parameters.AddWithValue("@email_cocriador", Email);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
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

        }
        public void CadastrarEmailCocriadores(string[] listEmails, string idIdeia)
        {
            foreach (var item in listEmails)
            {
                CadastrarEmailCocriadores(item.Trim(), idIdeia);
            }
        }
        public Ideia BuscarPontuacaoPorIdQuestaoAvaliacaoIdeia(string idIdeia, int idQuestaoAvaliacaoIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = " select p.id as id_ideia,                                                          " +
                              "        sum(a.pontuacao) as pontuacao                                              " +
                              "  from avaliacao_ideias a                                                          " +
                              "    inner join projetos p  on p.id = a.id_projeto                                  " +
                              "    inner join questoes_avaliacao_ideias q on q.id = a.id_questao_avaliacao_ideias " +
                              "   where p.id = @idIdeia                                                           " +
                              "   and q.id = @idQuestaoAvaliacaoIdeia                                             ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idQuestaoAvaliacaoIdeia", idQuestaoAvaliacaoIdeia);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    var Ideia = new Ideia();
                    while (dr.Read())
                    {
                        Ideia.IDPROJETO = Convert.ToInt32(dr["id_ideia"]);
                        Ideia.PONTUACAO = Convert.ToInt32(dr["pontuacao"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Ideia;
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
        public IdeiaTotais BuscarQtdFeedbacksPorIdeia(string idIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = " select count(distinct id_usuario_avaliador) as qtdFeedback " +
                              "   from avaliacao_ideias  	                               " +
                              "   where id_projeto = @idIdeia                              ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    var total = new IdeiaTotais();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        total.TIPO = "FEEDBACKS";
                        total.VALOR = dr["qtdFeedback"].ToString();
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return total;
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

        public int BuscarQtdFeedbacksPorIdeiaEPorIdUsuarioAvaliador(string idIdeia, string idUsuario)
        {
            var db = new ClassDb();
            var qtdFeedback = 0;
            try
            {
                try
                {
                    var sql = " select count(distinct id_usuario_avaliador) as qtdFeedback " +
                              "   from avaliacao_ideias  	                               " +
                              "   where id_projeto = @idIdeia                              " +
                              "   and id_usuario_avaliador = @usuario                      ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);
                    cmd.Parameters.AddWithValue("@usuario", idUsuario);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    while (dr.Read())
                    {
                        qtdFeedback = Convert.ToInt32(dr["qtdFeedback"]);
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return qtdFeedback;
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

        public void PadrozinarLinkPit()
        {
            string linkPronto = "";
            string link = this.LINKPIT;
            if (link.Trim() != "" && (link.ToLower().Contains("youtube") || link.ToLower().Contains("youtu.be")))
            {
                //string tipoLink1 = "https://www.youtube.com/watch?v=ypQZV03l80g&ab_channel=GravityMusic";
                //string tipoLink2 = "https://youtu.be/ypQZV03l80g";

                if (link.ToLower().Contains("watch?v="))
                    link = link.Replace("watch?v=", "").Replace("&", "/");

                var list = link.Split('/');
                linkPronto = "https://www.youtube.com/embed/" + list[list.Count() - 1];

            }
            this.LINKPIT = linkPronto;
        }
        public void AtualizarStatus(string idIdeia, int status)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "UPDATE projetos set status = @status where id = @id_projeto";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@status", status);
                        cmd.Parameters.AddWithValue("@id_projeto", idIdeia);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
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
        }
        public long DuplicarIdeia(string idIdeia, string nomeIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    string sql = " select * from projetos where id = @idIdeia";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        this.ID = Convert.ToInt32(dr["id"]);
                        this.NOMEPROJETO = nomeIdeia;
                        this.TIPOPROJETO = dr["tipo_projeto"].ToString();
                        this.LIDER = dr["lider"].ToString();
                        this.LINKPIT = dr["linkpit"].ToString();
                        this.TITULO = dr["lider"].ToString();
                        this.STATUS = "1";
                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    sql = "select * from cadastro_ideias where id_projeto = @idIdeia order by id_questao_ideias asc";

                    cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idIdeia", idIdeia);

                    db.AbrirConexao();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        switch (dr["id_questao_ideias"].ToString())
                        {
                            case "1":
                                {
                                    this.TEXTOIDEIAINOVADORA = dr["resposta"].ToString();
                                    break;
                                }
                            case "2":
                                {
                                    this.TEXTOPROBLEMAS = dr["resposta"].ToString();
                                    break;
                                }
                            case "3":
                                {
                                    this.TEXTOPRATICA = dr["resposta"].ToString();
                                    break;
                                }
                            case "4":
                                {
                                    this.TEXTORESULTADOS = dr["resposta"].ToString();
                                    break;
                                }
                            case "5":
                                {
                                    this.TEXTOIMPACTO = dr["resposta"].ToString();
                                    break;
                                }
                        }

                    }

                    cmd.Connection.Close();
                    db.FecharConexao();

                    return CadastrarIdeia();
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

        public int BuscarStatusProjeto(string idProjeto)
        {
            var db = new ClassDb();
            var status = 0;
            try
            {
                try
                {
                    var sql = " select a.status       " +
                              "   from projetos a     " +
                              "  where a.id = @idProjeto ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idProjeto", idProjeto);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        status = Convert.ToInt32(dr["status"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return status;
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

        public int BuscarQtdProjetoPorIdUsuarioENomeIdeia(string nomeProjeto)
        {
            var db = new ClassDb();
            var qtdEncontrado = 0;
            try
            {
                try
                {
                    var sql = " select count(*) as qtd " +
                              "   from projetos p     " +
                              "  where p.id_usuario = @idUsuario " +
                              "  and p.nome_projeto = @nomeProjeto ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idUsuario", HttpContext.Current.Session["Id"].ToString());
                    cmd.Parameters.AddWithValue("@nomeProjeto", nomeProjeto);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtdEncontrado = Convert.ToInt32(dr["qtd"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return qtdEncontrado;
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

        public bool EditIdeia(int idProjeto, int idQuestaoIdeia, string textoideia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "update cadastro_ideias " +
                        " set resposta = @textoideia " +
                        "where id_projeto = @idProjeto " +
                        "and id_questao_ideias = @idQuestaoIdeia";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@textoideia", textoideia);
                        cmd.Parameters.AddWithValue("@idProjeto", idProjeto);
                        cmd.Parameters.AddWithValue("@idQuestaoIdeia", idQuestaoIdeia);
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

        public int GetQtdRespostaCoCriadores(string idIdeia)
        {
            var db = new ClassDb();
            int qtdEncontrado = 0;

            try
            {
                try
                {
                    var sql = " select count(*)qtd from resposta_cocriadores where id_projeto = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtdEncontrado = Convert.ToInt32(dr["qtd"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return qtdEncontrado;

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

        public string EditLinkPit(int idProjeto, string linkPit)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "update projetos        " +
                              "   set linkpit = @link " +
                              "where id = @id         ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", idProjeto);
                        cmd.Parameters.AddWithValue("@link", linkPit);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }

                    return linkPit;
                }
                catch (Exception ex)
                {
                    return "";
                    throw ex;
                }
            }
            finally
            {
                db.FecharConexao();
            }

        }

        public string GetIdHash(string hash)
        {
            var db = new ClassDb();
            string idideia = "";
            try
            {
                try
                {
                    var sql = "select * from hash_registro where hash = @hash";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@hash", hash);
                    db.AbrirConexao();

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        idideia = dr["id_registro"].ToString();
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

            return idideia;
        }

        public string GetHashId(string id)
        {
            var db = new ClassDb();
            string hashId = "";
            try
            {
                try
                {
                    var sql = "select * from hash_registro where id_registro = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    db.AbrirConexao();

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        hashId = dr["hash"].ToString();
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
            return hashId;

        }

        public string GerarHashId(string id)
        {
            var db = new ClassDb();
            string hashId = "";

            try
            {
                try
                {
                    var sql = "select * from hash_registro where id_registro = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", id);
                    db.AbrirConexao();

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        hashId = dr["hash"].ToString();
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

            if (hashId == "")
            {
                try
                {
                    try
                    {
                        string hash = Encript.RandomString();
                        var sql = "INSERT INTO hash_registro (id_registro, hash) VALUES (@id_registro, @hash) ";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@id_registro", id);
                            cmd.Parameters.AddWithValue("@hash", hash);
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                            hashId = hash;
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
            }

            return hashId;
        }



        public void CadastrarAnexo(string IdIdeia, string NomeAnexo)
        {
            var db = new ClassDb();
            try
            {
                try
                {

                    // PROJETO
                    var sql = "INSERT INTO Anexos_Ideia (id_ideia, nome_Anexo) " +
                              "                   VALUES(@id_ideia, @nome_Anexo) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id_ideia", IdIdeia);
                        cmd.Parameters.AddWithValue("@nome_Anexo", NomeAnexo);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
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

        }

        public List<string> GetAnexos(string IdIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    db.AbrirConexao();

                    List<string> Items = new List<string>();
                    string ticket = Encript.RandomString() + Encript.RandomString();
                    string sql = "select * from Anexos_Ideia where id_ideia = @id_ideia";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_ideia", IdIdeia);

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                        Items.Add(dr["nome_Anexo"].ToString());

                    return Items;
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

        public void cadastrarPergustasAvaliacao(Int64 idprojeto, string emailAvaliador)
        {
            var db = new ClassDb();
            try
            {
                Boolean JaCadastrado = false;
                var sqlBusca = "SELECT count(*)QTD FROM cadastro_ideias WHERE email_avaliador = @email_usuario and id_projeto = " + idprojeto.ToString();
                using (MySqlCommand cmdBusca = new MySqlCommand(sqlBusca, db._conn))
                {
                    db.AbrirConexao();

                    cmdBusca.Parameters.AddWithValue("@email_usuario", emailAvaliador);
                    MySqlDataReader drBusca = cmdBusca.ExecuteReader(CommandBehavior.CloseConnection);
                    while (drBusca.Read())
                    {
                        JaCadastrado = Convert.ToInt64(drBusca["QTD"].ToString()) > 0 ? true : false;
                    }
                    db.FecharConexao();
                }

                if (!JaCadastrado)
                {
                    // PERGUNTAS
                    var sql = "INSERT INTO cadastro_ideias (email_avaliador, resposta, id_projeto, id_questao_ideias, data_hora) " +
                              "                      VALUES(@email_avaliador, @resposta, @id_projeto, @id_questao_ideias, @data_hora) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        for (int i = 0; i < 5; i++)
                        {
                            cmd.Parameters.AddWithValue("@email_avaliador", emailAvaliador);
                            cmd.Parameters.AddWithValue("@id_projeto", idprojeto);
                            cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                            switch (i)
                            {
                                case 0:
                                    {
                                        cmd.Parameters.AddWithValue("@resposta", this.TEXTOIDEIAINOVADORA);
                                        cmd.Parameters.AddWithValue("@id_questao_ideias", 1);
                                        break;
                                    }
                                case 1:
                                    {
                                        cmd.Parameters.AddWithValue("@resposta", this.TEXTOPROBLEMAS);
                                        cmd.Parameters.AddWithValue("@id_questao_ideias", 2);
                                        break;
                                    }
                                case 2:
                                    {
                                        cmd.Parameters.AddWithValue("@resposta", this.TEXTOPRATICA);
                                        cmd.Parameters.AddWithValue("@id_questao_ideias", 3);
                                        break;
                                    }
                                case 3:
                                    {
                                        cmd.Parameters.AddWithValue("@resposta", this.TEXTORESULTADOS);
                                        cmd.Parameters.AddWithValue("@id_questao_ideias", 4);
                                        break;
                                    }
                                case 4:
                                    {
                                        cmd.Parameters.AddWithValue("@resposta", this.TEXTOIMPACTO);
                                        cmd.Parameters.AddWithValue("@id_questao_ideias", 5);
                                        break;
                                    }
                            }

                            cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();
                        }

                        cmd.Connection.Close();
                    }
                }
            }
            finally
            {
                db.FecharConexao();
            }
        }

    }
}