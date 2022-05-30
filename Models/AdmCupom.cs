using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Models
{
    public class AdmCupom
    {
        public int ID_CUPOM { get; set; }
        public string CUPOM { get; set; }
        public string STATUS { get; set; }
        public string DESCRICAO { get; set; }
        public string VALIDADE { get; set; }
        public string DESCONTO { get; set; }
        public string QUANTIDADE { get; set; }
        public string DISPONIVEL { get; set; }

        public AdmCupom()
        {
            ID_CUPOM = 0;
            CUPOM = "";
            STATUS = "1";
            DESCRICAO = "";
            VALIDADE = "";
            DESCONTO = "";
            QUANTIDADE = "";
            DISPONIVEL = "";
        }
        public List<AdmCupom> BuscarCupons()
        {
            var db = new ClassDb();
            var Cupom = new List<AdmCupom>();
            try
            {
                try
                {
                    var sql = "select *, " +
                        "case status when 0 then 'noChecked' ELSE 'checked' END AS statusFormat, " +
                        "DATE_FORMAT(validade, '%d/%m/%Y') as validadeFormat " +
                        "from ecom_cupons";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new AdmCupom();
                        item.ID_CUPOM = Convert.ToInt32(dr["idCupom"]);
                        item.CUPOM = dr["cupom"].ToString();
                        item.STATUS = dr["statusFormat"].ToString();
                        item.DESCRICAO = dr["descricao"].ToString();
                        item.VALIDADE = dr["validadeFormat"].ToString();
                        item.DESCONTO = dr["desconto"].ToString();
                        item.QUANTIDADE = dr["quantidade"].ToString();
                        item.DISPONIVEL = dr["disponivel"].ToString();

                        Cupom.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Cupom;
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
        public AdmCupom SelectCup(string strCupom)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select *, " +
                        "case status when 0 then 'noChecked' ELSE 'checked' END AS statusFormat, " +
                        "DATE_FORMAT(validade, '%d/%m/%Y') as validadeFormat " +
                        "from ecom_cupons WHERE cupom = @cupom and validade >= NOW() and disponivel > 0 "+
                        "ORDER BY idCupom DESC LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@cupom", strCupom);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    AdmCupom cupom = new AdmCupom();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        cupom.ID_CUPOM = Convert.ToInt32(dr["idCupom"]);
                        cupom.CUPOM = dr["cupom"].ToString();
                        cupom.STATUS = dr["statusFormat"].ToString();
                        cupom.DESCRICAO = dr["descricao"].ToString();
                        cupom.VALIDADE = dr["validadeFormat"].ToString();
                        cupom.DESCONTO = dr["desconto"].ToString();
                        cupom.QUANTIDADE = dr["quantidade"].ToString();
                        cupom.DISPONIVEL = dr["disponivel"].ToString();
                    }
                    return cupom;
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
        public AdmCupom SelectCupom(int idCupom)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select *, " +
                        "case status when 0 then 'noChecked' ELSE 'checked' END AS statusFormat, " +
                        "DATE_FORMAT(validade, '%d/%m/%Y') as validadeFormat " +
                        "from ecom_cupons WHERE idCupom = @id";
                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@id", idCupom);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    AdmCupom cupom = new AdmCupom();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        cupom.ID_CUPOM = Convert.ToInt32(dr["idCupom"]);
                        cupom.CUPOM = dr["cupom"].ToString();
                        cupom.STATUS = dr["statusFormat"].ToString();
                        cupom.DESCRICAO = dr["descricao"].ToString();
                        cupom.VALIDADE = dr["validadeFormat"].ToString();
                        cupom.DESCONTO = dr["desconto"].ToString();
                        cupom.QUANTIDADE = dr["quantidade"].ToString();
                        cupom.DISPONIVEL = dr["disponivel"].ToString();

                    }
                    return cupom;
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
        public bool DeleteCupom(AdmCupom cupom)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "DELETE FROM ecom_cupons WHERE idCupom = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", cupom.ID_CUPOM);
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
        public AdmCupom AlterarCupom(AdmCupom cupom)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "UPDATE ecom_cupons set cupom = @nome, status = @status, descricao = @descricao, validade = @validade, desconto = @desconto, quantidade = @quantidade, disponivel = @disponivel WHERE idCupom = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", cupom.ID_CUPOM);
                        cmd.Parameters.AddWithValue("@nome", cupom.CUPOM);
                        cmd.Parameters.AddWithValue("@status", cupom.STATUS);
                        cmd.Parameters.AddWithValue("@descricao", cupom.DESCRICAO);
                        cmd.Parameters.AddWithValue("@validade", cupom.VALIDADE);
                        cmd.Parameters.AddWithValue("@desconto", cupom.DESCONTO);
                        cmd.Parameters.AddWithValue("@quantidade", cupom.QUANTIDADE);
                        cmd.Parameters.AddWithValue("@disponivel", cupom.DISPONIVEL);

                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return cupom; //SelectCupom(cupom.ID_CUPOM);
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
        public AdmCupom CreateCupom(AdmCupom cupom)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO ecom_cupons (cupom, status, descricao, validade, desconto, quantidade, disponivel) " +
                          "VALUES (@nome, @status, @descricao, @validade, @desconto, @quantidade, @disponivel) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@nome", cupom.CUPOM);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@descricao", cupom.DESCRICAO);
                        cmd.Parameters.AddWithValue("@validade", cupom.VALIDADE);
                        cmd.Parameters.AddWithValue("@desconto", cupom.DESCONTO);
                        cmd.Parameters.AddWithValue("@quantidade", cupom.QUANTIDADE);
                        cmd.Parameters.AddWithValue("@disponivel", cupom.DISPONIVEL);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return SelectCup("0");
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