using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class Tags
    {
        public string ID { get; set; }
        public string DESCRICAO { get; set; }
        public string ATIVO { get; set; }
        public string ID_ADM { get; set; }

        public Tags()
        {
            ID = "0";
            DESCRICAO = "";
            ATIVO = "N";
            ID_ADM = "0";
        }

        public bool Salvar(List<Tags> param)
        {
            int id;
            try
            {
                AlterarStatusGeral(Convert.ToInt32(HttpContext.Current.Session["Id"]));

                if (param != null)
                {
                    foreach (var item in param)
                    {
                        try
                        {
                            id = Convert.ToInt32(item.ID);
                        }
                        catch
                        {
                            item.ID = "0";
                        }

                        if (Convert.ToInt32(item.ID) == 0)
                        {
                            Cadastrar(item);

                        }
                        else if (item.ATIVO == "N")
                        {
                            AlterarStatus(Convert.ToInt32(item.ID), "S");
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public void Cadastrar(Tags param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO tags (descricao, id_adm, ativo)    " +
                              "           VALUES(@descricao, @id_adm, @ativo) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@descricao", param.DESCRICAO);
                        cmd.Parameters.AddWithValue("@id_adm", param.ID_ADM);
                        cmd.Parameters.AddWithValue("@ativo", "S");
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

        public void AlterarStatus(Int32 id, string Status)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "UPDATE tags set ativo = @ativo WHERE ID = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@ativo", Status.ToUpper());
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

        public void AlterarStatusGeral(Int32 id_adm)
        {
            var db = new ClassDb();
            bool temRegistros = true;
            try
            {

                try
                {
                    var sql = "select * from tags where id_adm = @id_adm";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        cmd.Parameters.AddWithValue("@id_adm", id_adm);

                        db.AbrirConexao();
                        MySqlDataReader dr;

                        dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                        if (!dr.HasRows)
                        {
                            temRegistros = false;
                        }
                        cmd.Connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;

                }

                if (temRegistros)
                {
                    try
                    {
                        var sql = "UPDATE tags SET ativo = 'N' WHERE ID_ADM = @ID_ADM ";
                        using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                        {
                            db.AbrirConexao();
                            cmd.Parameters.AddWithValue("@ID_ADM", id_adm);
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

        public List<Tags> GetAll()
        {
            List<Tags> lista = new List<Tags>();
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select * from tags";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    //cmd.Parameters.AddWithValue("@usuario", HttpContext.Current.Session["Id"].ToString());

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Tags();
                        item.ID = dr["ID"].ToString();
                        item.DESCRICAO = dr["DESCRICAO"].ToString();
                        item.ATIVO = dr["ATIVO"].ToString();
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

        public List<Tags> BuscarTags()
        {
            List<Tags> lista = new List<Tags>();
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select * from tags";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new Tags();
                        item.ID = dr["ID"].ToString();
                        item.DESCRICAO = dr["DESCRICAO"].ToString();
                        item.ATIVO = dr["ATIVO"].ToString();
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