using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace WebApp.Models
{
    public class IdeiaDescricaoProblema
    {
        public int ID { get; set; }      
        public string DOR { get; set; }

        public IdeiaDescricaoProblema()
        {
            ID = 0;           
            DOR = "";
        }

        public IdeiaDescricaoProblema BuscarDescricaoProblema(string idIdeia)
        {
            var db = new ClassDb();
            var Ideia = new IdeiaDescricaoProblema();
            try
            {
                try
                {
                    var sql = " select p.id,                      " +
                              "        p.dor                      " +
                              "  from  projetos p                 " +
                              " where p.id = @projeto             ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        Ideia.ID = Convert.ToInt32(dr["id"]);
                        Ideia.DOR = dr["dor"].ToString();
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

            return Ideia;
        }
     
        public bool EditDescricaoProblema(string idIdeia, string dorIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "update projetos      " +
                              "   set dor = @dor    " +                              
                              " where id = @idIdeia ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@dor", dorIdeia);
                        cmd.Parameters.AddWithValue("@idIdeia", idIdeia);
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
    }
}