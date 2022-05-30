using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Security;
using WebApp.Utils;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AdmPlano
    {
        public int ID_PLANO { get; set; }
        public string PLANO_TITULO { get; set; }
        public string PLANO_DESCRICAO { get; set; }
        public string PLANO_QUANTIDEIAS { get; set; }
        public string PLANO_QUANTUSUARIOS { get; set; }
        public string PLANO_QUANTVALIDACOES { get; set; }
        public string PLANO_VALORIDEIA { get; set; }
        public string PLANO_VALORUSUARIO { get; set; }
        public string PLANO_VALORVALIDACAO { get; set; }
        public string PLANO_VALORTOTAL { get; set; }
        public string PLANO_STATUS { get; set; }

        public AdmPlano()
        {
            ID_PLANO = 0;
            PLANO_TITULO = "";
            PLANO_DESCRICAO = "";
            PLANO_QUANTIDEIAS = "";
            PLANO_QUANTUSUARIOS = "";
            PLANO_QUANTVALIDACOES = "";
            PLANO_VALORIDEIA = "";
            PLANO_VALORUSUARIO = "";
            PLANO_VALORVALIDACAO = "";
            PLANO_VALORTOTAL = "";
            PLANO_STATUS = "1";
        }

        public List<AdmPlano> BuscarPlanos()
        {
            var db = new ClassDb();
            var Pagamento = new List<AdmPlano>();
            try
            {
                try
                {
                    var sql = "select idPlano, titulo, descricao, quantIdeias, quantUsuarios, quantValidacoes, " +
                        "convert(replace(TRUNCATE(valorIdeia, 2), '.', ','), nchar) as valorIdeia, " +
                        "convert(replace(TRUNCATE(valorUsuario, 2), '.', ','), nchar) as valorUsuario, " +
                        "convert(replace(TRUNCATE(valorValidacao, 2), '.', ','), nchar) as valorValidacao, " +
                        "convert(replace(TRUNCATE(valorTotal, 2), '.', ','), nchar) as valorTotal, " +
                        "CASE status WHEN 1 THEN 'checked' ELSE 'noChecked' END AS status from ecom_planos";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new AdmPlano();
                        item.ID_PLANO = Convert.ToInt32(dr["idPlano"]);
                        item.PLANO_TITULO = dr["titulo"].ToString();
                        item.PLANO_DESCRICAO = dr["descricao"].ToString();
                        item.PLANO_QUANTIDEIAS = dr["quantIdeias"].ToString();
                        item.PLANO_QUANTUSUARIOS = dr["quantUsuarios"].ToString();
                        item.PLANO_QUANTVALIDACOES = dr["quantValidacoes"].ToString();
                        item.PLANO_VALORIDEIA = dr["valorIdeia"].ToString();
                        item.PLANO_VALORUSUARIO = dr["valorUsuario"].ToString();
                        item.PLANO_VALORVALIDACAO = dr["valorValidacao"].ToString();
                        item.PLANO_VALORTOTAL = dr["valorTotal"].ToString();
                        item.PLANO_STATUS = dr["status"].ToString();

                        Pagamento.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Pagamento;
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
        public AdmPlano SelectPlano(int idPlano)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select idPlano, titulo, descricao, quantIdeias, quantUsuarios, quantValidacoes, " +
                        "replace(TRUNCATE(valorIdeia, 2), ',', '.') as valorIdeia, " +
                        "replace(TRUNCATE(valorUsuario, 2), ',', '.') as valorUsuario, " +
                        "replace(TRUNCATE(valorValidacao, 2), ',', '.') as valorValidacao, " +
                        "replace(TRUNCATE(valorTotal, 2), ',', '.') as valorTotal, " +
                        "CASE status WHEN 1 THEN 'checked' ELSE 'noChecked' END AS status, status as status2 from ecom_planos  WHERE idPlano = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", idPlano);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    AdmPlano plano = new AdmPlano();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        plano.ID_PLANO = Convert.ToInt32(dr["idPlano"]);
                        plano.PLANO_TITULO = dr["titulo"].ToString();
                        plano.PLANO_DESCRICAO = dr["descricao"].ToString();
                        plano.PLANO_QUANTIDEIAS = dr["quantIdeias"].ToString();
                        plano.PLANO_QUANTUSUARIOS = dr["quantUsuarios"].ToString();
                        plano.PLANO_QUANTVALIDACOES = dr["quantValidacoes"].ToString();
                        plano.PLANO_VALORIDEIA = dr["valorIdeia"].ToString();
                        plano.PLANO_VALORUSUARIO = dr["valorUsuario"].ToString();
                        plano.PLANO_VALORVALIDACAO = dr["valorValidacao"].ToString();
                        plano.PLANO_VALORTOTAL = dr["valorTotal"].ToString();
                        plano.PLANO_STATUS = dr["status"].ToString();

                    }
                    return plano;
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
        public AdmPlano BuscarPlanoSingle(string planoTitulo)
        {
            var db = new ClassDb();

            try
            {
                try
                {
                    var sql = "select idPlano, titulo, descricao, quantIdeias, quantUsuarios, quantValidacoes, " +
                        "TRUNCATE(valorIdeia, 2) as valorIdeia, TRUNCATE(valorUsuario, 2) as valorUsuario, " +
                        "TRUNCATE(valorValidacao, 2) as valorValidacao, TRUNCATE(valorTotal, 2) as valorTotal, " +
                        "CASE status WHEN 1 THEN 'checked' ELSE 'noChecked' END AS status "+
                        "from ecom_planos where titulo = @planoTitulo ORDER BY idPlano DESC LIMIT 1";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    db.AbrirConexao();
                    cmd.Parameters.AddWithValue("@planoTitulo", planoTitulo);
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                    AdmPlano Plano = new AdmPlano();
                    while (dr.Read())
                    {
                        Plano.ID_PLANO = Convert.ToInt32(dr["idPlano"]);
                        Plano.PLANO_TITULO = dr["titulo"].ToString();
                        Plano.PLANO_DESCRICAO = dr["descricao"].ToString();
                        Plano.PLANO_QUANTIDEIAS = dr["quantIdeias"].ToString();
                        Plano.PLANO_QUANTUSUARIOS = dr["quantUsuarios"].ToString();
                        Plano.PLANO_QUANTVALIDACOES = dr["quantValidacoes"].ToString();
                        Plano.PLANO_VALORIDEIA = dr["valorIdeia"].ToString();
                        Plano.PLANO_VALORUSUARIO = dr["valorUsuario"].ToString();
                        Plano.PLANO_VALORVALIDACAO = dr["valorValidacao"].ToString();
                        Plano.PLANO_VALORTOTAL = dr["valorTotal"].ToString();
                        Plano.PLANO_STATUS = dr["status"].ToString();
                    }
                    return Plano;
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

        public AdmPlano CreatePlano(AdmPlano plano)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO ecom_planos (titulo, descricao, quantIdeias, quantUsuarios, quantValidacoes, valorIdeia, valorUsuario, valorValidacao, valorTotal, status) " +
                          "VALUES (@titulo, @descricao, @quantIdeias, @quantUsuarios, @quantValidacoes, @valorIdeia, @valorUsuario, @valorValidacao, @valorTotal, @status) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@titulo", plano.PLANO_TITULO);
                        cmd.Parameters.AddWithValue("@descricao", plano.PLANO_DESCRICAO);
                        cmd.Parameters.AddWithValue("@quantIdeias", plano.PLANO_QUANTIDEIAS);
                        cmd.Parameters.AddWithValue("@quantUsuarios", plano.PLANO_QUANTUSUARIOS);
                        cmd.Parameters.AddWithValue("@quantValidacoes", plano.PLANO_QUANTVALIDACOES);
                        cmd.Parameters.AddWithValue("@valorIdeia", plano.PLANO_VALORIDEIA);
                        cmd.Parameters.AddWithValue("@valorUsuario", plano.PLANO_VALORUSUARIO);
                        cmd.Parameters.AddWithValue("@valorValidacao", plano.PLANO_VALORVALIDACAO);
                        cmd.Parameters.AddWithValue("@valorTotal", plano.PLANO_VALORTOTAL);
                        cmd.Parameters.AddWithValue("@status", plano.PLANO_STATUS);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return plano;
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
        public AdmPlano AlterarPlano(AdmPlano plano)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "UPDATE ecom_planos set titulo = @titulo, descricao = @descricao, descricao = @descricao, quantIdeias = @quantIdeias, quantUsuarios = @quantUsuarios, quantValidacoes = @quantValidacoes, valorIdeia = @valorIdeia, valorUsuario = @valorUsuario, valorValidacao = @valorValidacao, valorTotal = @valorTotal, status = @status WHERE idPlano = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", plano.ID_PLANO);
                        cmd.Parameters.AddWithValue("@titulo", plano.PLANO_TITULO);
                        cmd.Parameters.AddWithValue("@descricao", plano.PLANO_DESCRICAO);
                        cmd.Parameters.AddWithValue("@quantIdeias", plano.PLANO_QUANTIDEIAS);
                        cmd.Parameters.AddWithValue("@quantUsuarios", plano.PLANO_QUANTUSUARIOS);
                        cmd.Parameters.AddWithValue("@quantValidacoes", plano.PLANO_QUANTVALIDACOES);
                        cmd.Parameters.AddWithValue("@valorIdeia", plano.PLANO_VALORIDEIA);
                        cmd.Parameters.AddWithValue("@valorUsuario", plano.PLANO_VALORUSUARIO);
                        cmd.Parameters.AddWithValue("@valorValidacao", plano.PLANO_VALORVALIDACAO);
                        cmd.Parameters.AddWithValue("@valorTotal", plano.PLANO_VALORTOTAL);
                        cmd.Parameters.AddWithValue("@status", plano.PLANO_STATUS);

                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return plano; //SelectCupom(cupom.ID_CUPOM);
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

        public bool DeletePlano(AdmPlano plano)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "DELETE FROM ecom_planos WHERE idPlano = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", plano.ID_PLANO);
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