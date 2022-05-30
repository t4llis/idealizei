using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PotencializarContato
    {
        public int ID { get; set; }
        public string NOME{ get; set; }
        public string EMAIL { get; set; }
        public string ASSUNTO { get; set; }
        public string MENSAGEM { get; set; }
        public DateTime DATA_HORA { get; set; }

        public PotencializarContato()
        {
            ID = 0;
            NOME = "";
            EMAIL = "";
            ASSUNTO = "";
            MENSAGEM = "";
            DATA_HORA = DateTime.Now;
        }


        public bool Cadastrar()
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO potencializar_ideia (id_usuario, nome, email, assunto, mensagem, data_hora) " +
                                "               VALUES(@id_usuario, @nome, @email, @assunto, @mensagem, @data_hora) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id_usuario", HttpContext.Current.Session["Id"].ToString());
                        cmd.Parameters.AddWithValue("@nome", this.NOME);
                        cmd.Parameters.AddWithValue("@email", this.EMAIL);
                        cmd.Parameters.AddWithValue("@assunto", this.ASSUNTO);
                        cmd.Parameters.AddWithValue("@mensagem", this.MENSAGEM);
                        cmd.Parameters.AddWithValue("@data_hora", DateTime.Now);
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