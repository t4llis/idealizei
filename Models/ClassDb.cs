using System;
using System.Data;
using System.Web.Mvc;
using MySql.Data.MySqlClient;

namespace WebApp.Models
{
    public class ClassDb
    {
        public string _conectionString { get; set; }
        public MySqlConnection _conn { get; set; }

        public ClassDb()
        {
            _conectionString = "server=arqueiros.com.br;port=3306;User id=arquei73_iddev;database=arquei73_iddev; password=ideal2019";
            _conn = new MySqlConnection(_conectionString);
        }

        public ClassDb(string param)
        {
            _conectionString = param;
            _conn = new MySqlConnection(_conectionString);
        }

        public void AbrirConexao()
        {
            try
            {
                if (!_conn.State.Equals(ConnectionState.Open))
                    _conn.Open();
            }
            catch
            {
                new Exception("Erro ao tentar conectar no banco");
            }
        }

        public void FecharConexao()
        {
            if (_conn.State.Equals(ConnectionState.Open))
                _conn.Close();
        }

    }
}