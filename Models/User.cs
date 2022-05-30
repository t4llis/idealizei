using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.Security;
using WebApp.Models.UserModels;
using WebApp.Utils;
using WebApp.Enum;

namespace WebApp.Models
{
    public class User
    {
        public int ID { get; set; }
        public string NOME { get; set; }
        public string SENHA { get; set; }
        public string EMAIL { get; set; }
        public string ROLE { get; set; }
        public string URLIMAGEM { get; set; }
        public string NIVELACESSO { get; set; }


        public string MASTER_ID { get; set; }
        public int QTD_USUARIOS { get; set; }
        public int QTD_IDEIAS { get; set; }
        public int QTD_AVALIACOES { get; set; }


        public User()
        {
            ID = 0;
            NOME = "";
            SENHA = "";
            EMAIL = "";
            ROLE = "";
            URLIMAGEM = "";
            NIVELACESSO = "";

            MASTER_ID = "";
            QTD_USUARIOS = 0;
            QTD_IDEIAS = 0;
            QTD_AVALIACOES = 0;
        }



        public bool VerificaContratoUsuario(int master_idUsuario)
        {
            var db = new ClassDb();
            var qtd = 0;
            try
            {
                try
                {
                    var sql = "select IFNULL((sum(p.quantUsuarios) - (select count(*) as qtdUsuarios from usuarios where master_idUsuario = @idUser)), 0) as usuarios " +
                    "from ecom_contratos as c JOIN ecom_planos as p ON c.idPlano = p.idPlano AND c.idUsuario = @idUser AND c.status = 1";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idUser", master_idUsuario);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtd = Convert.ToInt32(dr["usuarios"]);
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

        public User GetUsuario(string idUser)
        {
            var db = new ClassDb();
            try
            {
                try
                {

                    var sql = "SELECT * FROM usuarios WHERE id = @id";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    User user = new User();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {

                        user.ID = Convert.ToInt32(dr["id"]);
                        user.NOME = dr["nome"].ToString();
                        user.EMAIL = dr["email"].ToString();
                        user.ROLE = dr["role"].ToString().Split('_')[1];
                        user.URLIMAGEM = dr["URLIMAGEM"].ToString();
                        user.NIVELACESSO = dr["NIVELACESSO"].ToString();
                    }
                    return user;
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

        public List<User> GetUsuarioContrato(string nomeUser)
        {
            var db = new ClassDb();
            var Users = new List<User>();
            try
            {
                try
                {
                    var sql = "select * FROM usuarios where nome LIKE '%" + nomeUser + "%' ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    //cmd.Parameters.AddWithValue("@nome", nomeUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    //User user = new List<User>();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var user = new User();
                        user.ID = Convert.ToInt32(dr["id"]);
                        user.NOME = dr["nome"].ToString();
                        user.SENHA = dr["senha"].ToString();
                        user.EMAIL = dr["email"].ToString();
                        user.ROLE = dr["role"].ToString();
                        user.URLIMAGEM = dr["urlimagem"].ToString();
                        user.NIVELACESSO = dr["NIVELACESSO"].ToString();

                        Users.Add(user);
                    }
                    return Users;
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

        public List<User> GetAll()
        {
            var db = new ClassDb();
            List<User> lista = new List<User>();
            try
            {
                try
                {

                    var sql = "SELECT * FROM usuarios order by nome asc";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    
                    db.AbrirConexao();
                    MySqlDataReader dr;

                    
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        User user = new User();
                        user.ID = Convert.ToInt32(dr["id"]);
                        user.NOME = dr["nome"].ToString();
                        user.EMAIL = dr["email"].ToString();
                        if(dr["role"].ToString().Split('_').Length > 1)
                            user.ROLE = dr["role"].ToString().Split('_')[1];
                        else
                            user.ROLE = dr["role"].ToString();
                        user.URLIMAGEM = dr["URLIMAGEM"].ToString();
                        user.NIVELACESSO = dr["NIVELACESSO"].ToString();
                        lista.Add(user);
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

        public User GetUsuario(string nome, string senha)
        {
            var db = new ClassDb();
            try
            {
                try
                {

                    var sql = "SELECT * FROM usuarios WHERE email = @nome and senha = @senha";

                    var EncriptSenha = new Encript().RetornarMD5(senha);
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);

                    cmd.Parameters.AddWithValue("@nome", nome);
                    cmd.Parameters.AddWithValue("@senha", EncriptSenha);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    User user = new User();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {

                        user.ID = Convert.ToInt32(dr["id"]);
                        user.NOME = dr["nome"].ToString();
                        user.EMAIL = dr["email"].ToString();
                        user.ROLE = dr["role"].ToString().Split('_')[1];
                        user.URLIMAGEM = dr["URLIMAGEM"].ToString();
                        user.NIVELACESSO = dr["NIVELACESSO"].ToString();
                    }
                    return user;
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

        public User GetUsuarioFacebook(User param)
        {
            var db = new ClassDb();
            var user = new User();
            try
            {
                try
                {
                    var sql = "";
                    if ((param.EMAIL == "undefined") || (param.EMAIL == ""))
                    {
                        sql = "SELECT * FROM usuarios WHERE nome = @nome ";
                    }
                    else
                    {
                        sql = "SELECT * FROM usuarios WHERE ((nome = @nome ) or (email = @email))";
                    }
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        if ((param.EMAIL == "undefined") || (param.EMAIL == ""))
                        {
                            cmd.Parameters.AddWithValue("@nome", param.NOME);
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@nome", param.NOME);
                            cmd.Parameters.AddWithValue("@email", param.EMAIL);
                        }


                        db.AbrirConexao();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                user.ID = Convert.ToInt32(dr["id"]);
                                user.NOME = dr["nome"].ToString();
                                user.EMAIL = dr["email"].ToString();
                                user.ROLE = dr["role"].ToString().Split('_')[1];
                                user.URLIMAGEM = dr["URLIMAGEM"].ToString();
                                user.NIVELACESSO = dr["NIVELACESSO"].ToString();
                            }
                        }
                        cmd.Connection.Close();
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
            finally
            {

            }
        }

        public User GetUsuarioGoogle(User param)
        {
            var db = new ClassDb();
            var user = new User();
            try
            {
                try
                {
                    var sql = "SELECT * FROM usuarios WHERE ((nome = @nome ) or (email = @email))";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        cmd.Parameters.AddWithValue("@nome", param.NOME);
                        cmd.Parameters.AddWithValue("@email", param.EMAIL);

                        db.AbrirConexao();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {

                                user.ID = Convert.ToInt32(dr["id"]);
                                user.NOME = dr["nome"].ToString();
                                user.EMAIL = dr["email"].ToString();
                                user.ROLE = dr["role"].ToString().Split('_')[1];
                                user.URLIMAGEM = dr["URLIMAGEM"].ToString();
                                user.NIVELACESSO = dr["NIVELACESSO"].ToString();
                            }
                        }
                        cmd.Connection.Close();
                    }
                    return user;
                }
                catch (Exception ex)
                {
                    throw ex;

                }

            }
            finally
            {

            }
        }

        public User CadastrarUsuario(User param)
        {
            var db = new ClassDb();
            User userRetorno;
            try
            {
                try
                {
                    var sql = "INSERT INTO usuarios (nome, email, role, urlimagem, data_hora, ativo, senha, nivelacesso) " +
                              "               VALUES(@nome, @email, @role, @urlimagem, @data_hora, @ativo, @senha, @nivelacesso) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        var EncriptSenha = new Encript().RetornarMD5(param.SENHA);
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nome", param.NOME);
                        cmd.Parameters.AddWithValue("@email", param.EMAIL);
                        cmd.Parameters.AddWithValue("@role", "ROLE_USER");
                        cmd.Parameters.AddWithValue("@urlimagem", param.URLIMAGEM);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ativo", Convert.ToByte(true));
                        cmd.Parameters.AddWithValue("@senha", EncriptSenha);
                        cmd.Parameters.AddWithValue("@nivelacesso", 1);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }

                    userRetorno = GetUsuario(param.EMAIL, param.SENHA);

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

            if (userRetorno != null)
            {
                try
                {
                    var idPlanoFree = 42; // ID PLANO FREE
                    var planoFree = new AdmPlano().SelectPlano(idPlanoFree);

                    var contrato = new AdmContrato();
                    contrato.CONTRATO_IDUSUARIO = userRetorno.ID.ToString();
                    contrato.CONTRATO_IDPLANO = planoFree.ID_PLANO.ToString();
                    contrato.CONTRATO_DATACONTRATO = DateTime.Today.ToString("yyyy/MM/dd");
                    contrato.CONTRATO_DATACANCELAMENTO = null;
                    contrato.CONTRATO_DATARENOVACAO = null;
                    contrato.CONTRATO_QUANTUSUARIOS = planoFree.PLANO_QUANTUSUARIOS.ToString();
                    contrato.CONTRATO_QUANTIDEIAS = planoFree.PLANO_QUANTIDEIAS.ToString();
                    contrato.CONTRATO_QUANTVALIDACOES = planoFree.PLANO_QUANTVALIDACOES.ToString();
                    contrato.CONTRATO_VALORUSUARIO = Convert.ToDecimal(planoFree.PLANO_VALORUSUARIO);
                    contrato.CONTRATO_VALORIDEIA = Convert.ToDecimal(planoFree.PLANO_VALORIDEIA);
                    contrato.CONTRATO_VALORVALIDACAO = Convert.ToDecimal(planoFree.PLANO_VALORVALIDACAO);
                    contrato.CONTRATO_VALORTOTAL = planoFree.PLANO_VALORTOTAL;
                    contrato.CreateContrato(contrato);
                }
                finally
                {
                    db.FecharConexao();
                }
            }

            return userRetorno;
        }

        public User CadastrarUsuarioFacebook(User param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO usuarios (nome, email, role, urlimagem, data_hora, ativo, senha) " +
                              "               VALUES(@nome, @email, @role, @urlimagem, @data_hora, @ativo, @senha) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        var EncriptSenha = new Encript().RetornarMD5("0");
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nome", param.NOME);
                        cmd.Parameters.AddWithValue("@email", param.EMAIL);
                        cmd.Parameters.AddWithValue("@role", "ROLE_USER");
                        cmd.Parameters.AddWithValue("@urlimagem", param.URLIMAGEM);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ativo", Convert.ToByte(true));
                        cmd.Parameters.AddWithValue("@senha", EncriptSenha);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return GetUsuarioFacebook(param);
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

        public User CadastrarUsuarioGoogle(User param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO usuarios (nome, email, role, urlimagem, data_hora, ativo, senha) " +
                              "               VALUES(@nome, @email, @role, @urlimagem, @data_hora, @ativo, @senha) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        var EncriptSenha = new Encript().RetornarMD5("0");
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nome", param.NOME);
                        cmd.Parameters.AddWithValue("@email", param.EMAIL);
                        cmd.Parameters.AddWithValue("@role", "ROLE_USER");
                        cmd.Parameters.AddWithValue("@urlimagem", param.URLIMAGEM);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
                        cmd.Parameters.AddWithValue("@ativo", Convert.ToByte(true));
                        cmd.Parameters.AddWithValue("@senha", EncriptSenha);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return GetUsuarioGoogle(param);
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

        public bool VerificarEmail(string email)
        {
            var db = new ClassDb();
            var qtdRegistros = 0;
            try
            {
                try
                {
                    var sql = " select count(*) as qtd  " +
                              "   from usuarios a       " +
                              "  where a.email = @email ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@email", email.Trim());

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtdRegistros = Convert.ToInt32(dr["qtd"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    if (qtdRegistros > 0)
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

        public string CriarAcessoMudarSenha(string email)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    db.AbrirConexao();
                    int qtdRegistros = 0;
                    string stringRandom = Encript.RandomString();

                    var sql = " select count(*) as qtd    " +
                              "   from recuperar_senha a  " +
                              "  where a.email = @email   " +
                              "    and a.valido = 'S'     ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@email", email.Trim());

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        qtdRegistros = Convert.ToInt32(dr["qtd"]);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();


                    if (qtdRegistros > 0)
                    {
                        db.AbrirConexao();
                        sql = " update recuperar_senha set acesso = @acesso ";

                        cmd = new MySqlCommand(sql, db._conn);
                        cmd.Parameters.AddWithValue("@acesso", stringRandom);
                        cmd.ExecuteNonQuery();

                        cmd.Connection.Close();
                        db.FecharConexao();
                    }
                    else
                    {

                        db.AbrirConexao();
                        sql = " insert recuperar_senha (acesso, email, valido) " +
                              "   values(@acesso, @email, @valido)       ";

                        cmd = new MySqlCommand(sql, db._conn);
                        cmd.Parameters.AddWithValue("@acesso", stringRandom);
                        cmd.Parameters.AddWithValue("@email", email.Trim());
                        cmd.Parameters.AddWithValue("@valido", "S");

                        cmd.ExecuteNonQuery();

                        cmd.Connection.Close();
                        db.FecharConexao();
                    }

                    return stringRandom;

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

        public bool EnviarLinkRecuperacao(string email, string urlSite)
        {
            try
            {
                var acesso = CriarAcessoMudarSenha(email);
                var Mail = new Mail();
                Mail.ASSUNTO = "IDEALIZEI - Alteração de senha.";
                Mail.EMAIL_DESTINATARIO = email;
                Mail.MENSAGEM = "Acesse o link : " + urlSite + "?acesso=" + acesso + Environment.NewLine +
                                "Para alterar sua senha";
                var enviado = Mail.EnviarEmail();
                return enviado;
            }
            catch
            {
                return false;
            }
        }

        public User ValidarAlteracaoSenha(string acesso)
        {
            var db = new ClassDb();
            var user = new User();
            try
            {
                try
                {
                    db.AbrirConexao();
                    string email = "";
                    var sql = " select *                   " +
                              "   from recuperar_senha a   " +
                              "  where a.acesso = @acesso  " +
                              "    and a.valido = 'S'      ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@acesso", acesso);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        email = dr["email"].ToString();
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    if (email != "")
                    {
                        db.AbrirConexao();
                        sql = " select *                 " +
                              "   from usuarios a        " +
                              "  where a.email = @email  ";

                        cmd = new MySqlCommand(sql, db._conn);
                        cmd.Parameters.AddWithValue("@email", email);

                        db.AbrirConexao();
                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (dr.Read())
                        {
                            user.ID = Convert.ToInt32(dr["ID"]);
                            user.NOME = dr["NOME"].ToString();
                            user.EMAIL = dr["EMAIL"].ToString();
                            user.SENHA = dr["SENHA"].ToString();
                            break;
                        }

                        cmd.Connection.Close();
                        db.FecharConexao();
                    }
                    return user;

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

        public bool AlterarSenha(string email, string senha)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    // ATUALIZAÇÃO DA SENHA
                    db.AbrirConexao();
                    var EncriptSenha = new Encript().RetornarMD5(senha);
                    string sql = " update usuarios set senha = @senha where email = @email";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@senha", EncriptSenha);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    db.FecharConexao();

                    // LINK DE MUDANÇA DE SENHA
                    db.AbrirConexao();
                    sql = " update recuperar_senha set valido = 'N' where email = @email ";
                    cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.ExecuteNonQuery();
                    cmd.Connection.Close();
                    db.FecharConexao();

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

        public void AtualizarImagem(User param)
        {
            var db = new ClassDb();
            string sql = "";
            string urlAtual = "";
            bool atualiza = false;

            try
            {
                try
                {
                    sql = "select urlimagem from usuarios where id = @usuario";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@usuario", param.ID);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        urlAtual = dr["urlimagem"].ToString();
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();
                }
                catch (Exception ex)
                {
                    throw ex;
                }

                /*
                if (urlAtual.Contains("https://platform-lookaside.fbsbx.com/"))
                {
                    atualiza = true;
                }
                else if( (urlAtual.Contains("images/Profile/")) && (urlAtual != param.URLIMAGEM))
                {
                    atualiza = true;
                }
                */
                atualiza = true;


                if (atualiza)
                {
                    try
                    {
                        sql = "update usuarios set urlimagem = @novaurl where id = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@novaurl", param.URLIMAGEM);
                            cmd.Parameters.AddWithValue("@id", param.ID);
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }

            }
            finally
            {
                db.FecharConexao();
            }
        }

        public List<IdeiasProfile> BuscarTextosCoCriacao(string iduser = "")
        {
            List<IdeiasProfile> lista = new List<IdeiasProfile>();
            var db = new ClassDb();
            try
            {
                try
                {
                    string where = iduser == "" ? " order by c.nome_projeto, a.id_questao_ideias "
                                                : "  where b.id = @usuario order by c.nome_projeto, a.id_questao_ideias";

                    var sql = " select a.id,                                   " +
                              "  c.id as projeto_id,                           " +
                              "  c.nome_projeto,                               " +
                              "  a.resposta,                                   " +
                              "  a.data_hora,                                  " +
                              "  a.cor_card                                    " +
                              "  from resposta_cocriadores a                   " +
                              "  inner join usuarios b on b.id = a.id_usuario  " +
                              "  inner join projetos c on c.id = a.id_projeto  " 
                              +  where;

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    if(iduser != "")
                        cmd.Parameters.AddWithValue("@usuario", iduser);
                    
                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdeiasProfile();
                        item.ID = dr["id"].ToString();
                        item.ID_IDEIA = dr["projeto_id"].ToString();
                        item.NOME_IDEIA = dr["nome_projeto"].ToString();
                        item.RESPOSTA = dr["resposta"].ToString();
                        item.DATA_HORA = dr["data_hora"].ToString();
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

        public List<IdeiasEnvolvidasProfile> BuscarIdeiasEnvolvidas(string idUser = "")
        {
            List<IdeiasEnvolvidasProfile> lista = new List<IdeiasEnvolvidasProfile>();
            var db = new ClassDb();
            try
            {
                try
                {
                    string where1 = idUser == "" ? " " : "   and b.id = @usuario ";
                    string where2 = idUser == "" ? " " : " where c.id = @usuario ";

                    var sql = " select distinct a.nome_projeto, a.lider from projetos a where a.id in( " +
                              "  select distinct c.id_projeto as id                                    " +
                              "    from usuarios b,                                                    " +
                              "         ideias_cocriadores c                                           " +
                              "   where b.email = c.email_cocriador                                    " 
                              + where1 +
                              "  union all                                                             " +
                              "  select distinct a.id                                                  " +
                              "    from projetos a                                                     " +
                              "   inner join resposta_cocriadores b on b.id_projeto = a.id             " +
                              "   inner join usuarios c            on c.id = b.id_usuario              " 
                              + where2 +
                              "); ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    if(idUser != "")
                        cmd.Parameters.AddWithValue("@usuario", idUser);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new IdeiasEnvolvidasProfile();
                        item.NOME_IDEIA = dr["nome_projeto"].ToString();
                        item.LIDER = dr["lider"].ToString();
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
    
        public void AtualizarUser(User param)
        {
            var db = new ClassDb();
            string sql = "";
            
            try
            {
                try
                {
                    sql = "update usuarios set nome = @nome, role = @role, nivelacesso = @nivelacesso where id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nome", param.NOME);
                        cmd.Parameters.AddWithValue("@role", param.ROLE.Trim());
                        cmd.Parameters.AddWithValue("@nivelacesso", param.NIVELACESSO);
                        cmd.Parameters.AddWithValue("@id", param.ID);
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

        public List<string> GetPerfilAcessoUser(string idUser)
        {
            List<string> items = new List<string>();
            var user = new User().GetUsuario(idUser);
            items.Add(user.ROLE);
            items.Add(user.NIVELACESSO == "1" ? "BASICO" : user.NIVELACESSO == "2" ? "INTERMEDIARIO" : user.NIVELACESSO == "3" ? "AVANCADO" : "");
            items.Add(user.URLIMAGEM);
            items.Add(user.NOME);
            items.Add(user.EMAIL);
            return items;

        }

        public List<string> GetNivelAcesso()
        {
            List<string> items = new List<string>();    
            items.Add(NivelAcessoEnum.BASICO.ToString());
            items.Add(NivelAcessoEnum.INTERMEDIARIO.ToString());
            items.Add(NivelAcessoEnum.AVANCADO.ToString());
            
            return items;
        }

        public List<string> GetTiposPerfil()
        {
            List<string> items = new List<string>();
            items.Add("ROLE_ADMIN");
            items.Add("ROLE_USER");

            return items;
        }


        public List<GraficoAdm> GetDadosGraficoPerfilAdmin(string idUser = "", string data_de = "", string data_ate = "")
        {
            List<GraficoAdm> lista = new List<GraficoAdm>();
            var db = new ClassDb();
            try
            {
                try
                {
                    string whereUser = idUser == "" ? " " :  " and x.id_usuario = "+ idUser;
                    string whereData = data_de == "" ? " " : " and date(x.data_hora) between '"+ data_de +"' and '"+ data_ate +"'";

                    var sql = "  select IFNULL(tabela_A.qtd_ideias, 0) qtd_ideias,                                                " +
                              "         IFNULL(tabela_A.mes_ano, tabela_B.mes_ano) mes_ano_ideia,                                 " +
                              "                                                                                                   " +
                              "         IFNULL(tabela_B.qtd_cocriacao, 0) qtd_cocriacao,                                          " +
                              "         IFNULL(tabela_B.mes_ano, tabela_A.mes_ano) mes_ano_cocriacao                              " +
                              "      from                                                                                         " +
                              "                                                                                                   " +
                              "             --IDEIAS CRIADAS E SUAS DATAS                                                         " +
                              "             (SELECT COUNT(a.id) qtd_ideias, concat(a.mes_desc,'/',a.ano) mes_ano from(            " +
                              "                 SELECT x.id,                                                                      " +
                              "                        MONTH(x.data_hora) mes_id,                                                 " +
                              "                        CASE                                                                       " +
                              "                             WHEN MONTH(x.data_hora) = 1 THEN 'JAN'                                " +
                              "                             WHEN MONTH(x.data_hora) = 2 THEN 'FEV'                                " +
                              "                             WHEN MONTH(x.data_hora) = 3 THEN 'MAR'                                " +
                              "                             WHEN MONTH(x.data_hora) = 4 THEN 'ABR'                                " +
                              "                             WHEN MONTH(x.data_hora) = 5 THEN 'MAI'                                " +
                              "                             WHEN MONTH(x.data_hora) = 6 THEN 'JUN'                                " +
                              "                             WHEN MONTH(x.data_hora) = 7 THEN 'JUL'                                " +
                              "                             WHEN MONTH(x.data_hora) = 8 THEN 'AGO'                                " +
                              "                             WHEN MONTH(x.data_hora) = 9 THEN 'SET'                                " +
                              "                             WHEN MONTH(x.data_hora) = 10 THEN 'OUT'                               " +
                              "                             WHEN MONTH(x.data_hora) = 11 THEN 'NOV'                               " +
                              "                             ELSE 'DEZ'                                                            " +
                              "                         END mes_desc,                                                             " +
                              "                         YEAR(x.data_hora) ano                                                     " +
                              "                   FROM projetos x                                                                 " +
                              "                  WHERE YEAR(x.data_hora) <> 1                                                     " 

                                                + whereUser + whereData +
                              "                                                                                                   " +
                              "                    order by MONTH(x.data_hora))a                                                  " +
                              "                    group by a.mes_desc                                                            " +
                              "                    order by a.mes_id)tabela_A                                                     " +

                              "                 left join                                                                         " +

                              "              -- COCRIACOES FEITAS E SUAS DATAS                                                    " +
                              "             (select count(id_projeto) qtd_cocriacao, concat(mes_desc, '/', ano) mes_ano from(     " +
                              "             select x.id_projeto,                                                                  " +
                              "                    x.id_usuario,                                                                  " +
                              "                    MONTH(x.data_hora) mes_id,                                                     " +
                              "                    CASE                                                                           " +
                              "                         WHEN MONTH(x.data_hora) = 1 THEN 'JAN'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 2 THEN 'FEV'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 3 THEN 'MAR'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 4 THEN 'ABR'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 5 THEN 'MAI'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 6 THEN 'JUN'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 7 THEN 'JUL'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 9 THEN 'SET'                                    " +
                              "                         WHEN MONTH(x.data_hora) = 10 THEN 'OUT'                                   " +
                              "                         WHEN MONTH(x.data_hora) = 11 THEN 'NOV'                                   " +
                              "                         ELSE 'DEZ'                                                                " +
                              "                     END mes_desc,                                                                 " +
                              "                     YEAR(x.data_hora) ano                                                         " +
                              "                 from resposta_cocriadores x                                                       " +
                              "                 where YEAR(x.data_hora) <> 1                                                      " +
                              "                                                                                                   " 
                                               + whereUser + whereData +
                              "                                                                                                   " +
                              "                 group by x.id_projeto, x.id_usuario                                               " +
                              "                 order by x.id_projeto)b                                                           " +
                              "                 group by b.mes_desc                                                               " +
                              "                 order by b.mes_id) tabela_B on tabela_B.mes_ano = tabela_A.mes_ano;               ";


                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    
                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new GraficoAdm();
                        item.QTD_IDEIAS = dr["qtd_ideias"].ToString();
                        item.MES_ANO_IDEIAS = dr["mes_ano_ideia"].ToString();
                        item.QTD_COCRIACAO = dr["qtd_cocriacao"].ToString();
                        item.MES_ANO_COCRIACAO = dr["mes_ano_cocriacao"].ToString();
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


        
        public User ValidacaoSession(int idUsuario)
        {
            var db = new ClassDb();
            var user = new User();
            try
            {
                try
                {
                    db.AbrirConexao();
                    string master_idUsuario = idUsuario.ToString();
                    var sql = "select master_idUsuario from usuarios where id = @idUser";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idUser", idUsuario);

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        master_idUsuario = dr["master_idUsuario"].ToString() == "" ? idUsuario.ToString() : dr["master_idUsuario"].ToString();
                    }
                    db.FecharConexao();

                    if (master_idUsuario != "")
                    {
                        db.AbrirConexao();
                        sql = " select sum(p.quantIdeias) as ideias, sum(p.quantUsuarios) as users, sum(p.quantValidacoes) as validacoes "+
                              "   from ecom_contratos as c JOIN ecom_planos as p ON c.idPlano = p.idPlano AND c.idUsuario = @master_idUsuario AND c.status = 1 ";

                        cmd = new MySqlCommand(sql, db._conn);
                        cmd.Parameters.AddWithValue("@master_idUsuario", master_idUsuario);
                        
                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        while (dr.Read())
                        {
                            user.MASTER_ID = master_idUsuario;
                            user.QTD_USUARIOS   = dr["users"] != null && dr["users"].ToString() != ""  ? Convert.ToInt32(dr["users"]) : 0;
                            user.QTD_IDEIAS     = dr["ideias"] != null &&  dr["ideias"].ToString() != "" ? Convert.ToInt32(dr["ideias"]): 0;
                            user.QTD_AVALIACOES = dr["validacoes"] != null && dr["validacoes"].ToString() != "" ? Convert.ToInt32(dr["validacoes"]) : 0;
                            break;
                        }

                        cmd.Connection.Close();
                        db.FecharConexao();
                    }
                    return user;

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


        public bool GerarLinkConvite(User param)
        {
            var db = new ClassDb();
            
            try
            {
                try
                {
                    db.AbrirConexao();

                    string ticket = Encript.RandomString() + Encript.RandomString();
                    string sql = "INSERT INTO Link_Convite (id_user, ticket) VALUES (@user, @ticket)";
                    
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        cmd.Parameters.AddWithValue("@user", param.ID);
                        cmd.Parameters.AddWithValue("@ticket", ticket);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                        return true;
                    }
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

        public List<string> GetAllLinkConvite(User param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    db.AbrirConexao();

                    List<string> Items = new List<string>();
                    string ticket = Encript.RandomString() + Encript.RandomString();
                    string sql = "select * from Link_Convite where id_user = @user";
                    
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@user", param.ID);

                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                        Items.Add(dr["ticket"].ToString());
                    
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

    }
}