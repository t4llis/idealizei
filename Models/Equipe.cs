using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Equipe
    {
        public string ID { get; set; }
        public string ID_ADM { get; set; }
        public string NOME { get; set; }
        public string PARTICIPANTES { get; set; }
        public string PARTICIPANTES_ID { get; set; }
        public List<Equipe_Usuarios> USUARIOS { get; set; }

        public Equipe()
        {
            ID = "0";
            ID_ADM = "0";
            NOME = "";
            USUARIOS = new List<Equipe_Usuarios>();
        }


        public bool Salvar(Equipe param)
        {
            var db = new ClassDb();
            long idEquipe = 0;
            string sql = "";
                        
            try
            {
                if (param.ID == "0")
                {
                    try
                    {
                        // IDEIA
                        sql = "INSERT INTO equipe (nome, id_adm) VALUES(@nome, @id_adm) ";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@nome", param.NOME);
                            cmd.Parameters.AddWithValue("@id_adm", param.ID_ADM);
                            cmd.ExecuteNonQuery();
                            idEquipe = cmd.LastInsertedId;
                            cmd.Connection.Close();
                        }

                        if (idEquipe != 0)
                        {
                            foreach (var item in param.USUARIOS)
                            {
                                // IDEIA
                                sql = "INSERT INTO equipe_usuarios (id_equipe, id_user) VALUES(@id_equipe, @id_user) ";
                                using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                                {
                                    db.AbrirConexao();
                                    cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                                    cmd.Parameters.AddWithValue("@id_user", item.ID_USER);
                                    cmd.ExecuteNonQuery();
                                    cmd.Connection.Close();
                                }
                            }

                            return true;
                        }
                        else
                            return false;

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    try
                    {
                        // IDEIA
                        sql = "update equipe set nome = @nome where id = @id";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@nome", param.NOME);
                            cmd.Parameters.AddWithValue("@id", param.ID);
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }

                        // IDEIA
                        sql = "delete from equipe_usuarios where id_equipe = @id_equipe";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@id_equipe", param.ID);
                            cmd.ExecuteNonQuery();
                            cmd.Connection.Close();
                        }

                        foreach (var item in param.USUARIOS)
                        {
                            // IDEIA
                            sql = "INSERT INTO equipe_usuarios (id_equipe, id_user) VALUES(@id_equipe, @id_user) ";
                            using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                            {
                                db.AbrirConexao();
                                cmd.Parameters.AddWithValue("@id_equipe", param.ID);
                                cmd.Parameters.AddWithValue("@id_user", item.ID_USER);
                                cmd.ExecuteNonQuery();
                                cmd.Connection.Close();
                            }
                        }

                        return true;
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

        public List<Equipe> GetAll()
        {

            var db = new ClassDb();
            List<Equipe> lista = new List<Equipe>();
            string Participantes = "";
            string Participantes_id = "";
            string idEquipe = "0";
            string nomeEquipe = "";
            Equipe user;

            try
            {
                try
                {
                    var sql = " select a.id,                                                        " +
                              "        b.id_equipe,                                                 " +
                              "        a.nome,                                                      " +
                              "        b.id_user,                                                   " +
                              "        (select nome from usuarios where id = b.id_user)nome_user    " +
                              "   from equipe a                                                     " +
                              "  inner join equipe_usuarios b on b.id_equipe = a.id                 " +
                              "  where a.id_adm = @id_adm                                           " +
                              "  order by b.id_equipe desc                                          ";       

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id_adm", HttpContext.Current.Session["Id"].ToString());

                    db.AbrirConexao();
                    MySqlDataReader dr;


                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        if (idEquipe != "0" && idEquipe != dr["id_equipe"].ToString())
                        {
                            user = new Equipe();
                            user.ID = idEquipe;
                            user.NOME = nomeEquipe;
                            
                            Participantes = Participantes.Substring(0, Participantes.Length - 2);
                            Participantes_id = Participantes_id.Substring(0, Participantes_id.Length - 1);

                            user.PARTICIPANTES = Participantes;
                            user.PARTICIPANTES_ID = Participantes_id;
                            
                            Participantes = "";
                            Participantes_id = "";
                            idEquipe = "0";

                            lista.Add(user);

                            idEquipe = dr["id_equipe"].ToString();
                            nomeEquipe = dr["nome"].ToString();
                            Participantes += dr["nome_user"].ToString() + ", ";
                            Participantes_id += dr["id_user"].ToString() + ",";

                        } else {
                            idEquipe = dr["id_equipe"].ToString();
                            nomeEquipe = dr["nome"].ToString();
                            Participantes += dr["nome_user"].ToString() + ", ";
                            Participantes_id += dr["id_user"].ToString() + ",";
                        };
                    }

                    if (idEquipe != "0")
                    {
                        user = new Equipe();
                        user.ID = idEquipe;
                        user.NOME = nomeEquipe;
                        Participantes = Participantes.Substring(0, Participantes.Length - 2);
                        Participantes_id = Participantes_id.Substring(0, Participantes_id.Length - 1);
                        user.PARTICIPANTES = Participantes;
                        user.PARTICIPANTES_ID = Participantes_id;

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

        public bool Delete(string idEquipe)
        {
            var db = new ClassDb();
            string sql = "";
            try
            {
                try
                {
                    sql = "delete from equipe_usuarios where id_equipe = @id_equipe";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id_equipe", idEquipe);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }

                    sql = "delete from equipe where id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", idEquipe);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return true;
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