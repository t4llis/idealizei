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
    public class AdmContrato
    {

        public int ID_CONTRATO { get; set; }
        public string CONTRATO_IDUSUARIO { get; set; }
        public string CONTRATO_NOMEUSUARIO { get; set; }
        public string CONTRATO_IDPLANO { get; set; }
        public string CONTRATO_TITULOPLANO { get; set; }
        public string CONTRATO_DATACONTRATO { get; set; }
        public string CONTRATO_DATARENOVACAO { get; set; }
        public string CONTRATO_DATACANCELAMENTO { get; set; }
        public string CONTRATO_QUANTUSUARIOS { get; set; }
        public string CONTRATO_QUANTIDEIAS { get; set; }
        public string CONTRATO_QUANTVALIDACOES { get; set; }
        public decimal CONTRATO_VALORUSUARIO { get; set; }
        public decimal CONTRATO_VALORIDEIA { get; set; }
        public decimal CONTRATO_VALORVALIDACAO { get; set; }
        public string CONTRATO_VALORTOTAL { get; set; }
        public string CONTRATO_STATUS { get; set; }

        public AdmContrato()
        {
            ID_CONTRATO = 0;
            CONTRATO_IDUSUARIO = "";
            CONTRATO_IDPLANO = "";
            CONTRATO_DATACONTRATO = DateTime.Now.ToString("yyyy-MM-dd");
            CONTRATO_DATARENOVACAO = "";
            CONTRATO_DATACANCELAMENTO = "";
            CONTRATO_QUANTUSUARIOS = "";
            CONTRATO_QUANTIDEIAS = "";
            CONTRATO_QUANTVALIDACOES = "";
            CONTRATO_VALORUSUARIO = (decimal)0.00;
            CONTRATO_VALORIDEIA = (decimal)0.00;
            CONTRATO_VALORVALIDACAO = (decimal)0.00;
            CONTRATO_VALORTOTAL = "0,00";
            CONTRATO_STATUS = "1";

            CONTRATO_NOMEUSUARIO = "";
            CONTRATO_TITULOPLANO = "";
        }

        public List<AdmContrato> BuscarContratos()
        {
            var db = new ClassDb();
            var Contrato = new List<AdmContrato>();
            try
            {
                try
                {
                    var sql = "select c.idContrato, c.idUsuario, u.nome as nomeUsuario, c.idPlano, p.titulo as tituloPlano, " +
                        "DATE_FORMAT(c.dataContrato, '%d/%m/%Y') as dataContrato, "+
                        "DATE_FORMAT(c.dataRenovacao, '%d/%m/%Y') as dataRenovacao, "+
                        "DATE_FORMAT(c.dataCancelamento, '%d/%m/%Y') as dataCancelamento, "+
                        "c.quantUsuarios, c.quantIdeias, c.quantValidacoes, "+
                        "convert(replace(TRUNCATE(p.valorUsuario, 2), '.', ','), nchar) as valorUsuario, " +
                        "convert(replace(TRUNCATE(p.valorIdeia, 2), '.', ','), nchar) as valorIdeia, " +
                        "convert(replace(TRUNCATE(p.valorValidacao, 2), '.', ','), nchar) as valorValidacao, " +
                        "convert(replace(TRUNCATE(p.valorTotal, 2), '.', ','), nchar) as valorTotal, " +
                        "case c.status when 0 then 'noChecked' ELSE 'checked' END AS status "+
                        "from ecom_contratos as c JOIN usuarios as u JOIN ecom_planos as p " +
                        "ON u.id = c.idUsuario AND p.idPlano = c.idPlano";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var item = new AdmContrato();
                        item.ID_CONTRATO = Convert.ToInt32(dr["idContrato"]);
                        item.CONTRATO_IDUSUARIO = dr["idUsuario"].ToString();
                        item.CONTRATO_NOMEUSUARIO = dr["nomeUsuario"].ToString();
                        item.CONTRATO_IDPLANO = dr["idPlano"].ToString();
                        item.CONTRATO_TITULOPLANO = dr["tituloPlano"].ToString();
                        item.CONTRATO_DATACONTRATO = dr["dataContrato"].ToString();
                        item.CONTRATO_DATARENOVACAO = dr["dataRenovacao"].ToString();
                        item.CONTRATO_DATACANCELAMENTO = dr["dataCancelamento"].ToString();
                        item.CONTRATO_QUANTUSUARIOS = dr["quantUsuarios"].ToString();
                        item.CONTRATO_QUANTIDEIAS = dr["quantIdeias"].ToString();
                        item.CONTRATO_QUANTVALIDACOES = dr["quantValidacoes"].ToString();
                        item.CONTRATO_VALORUSUARIO = Convert.ToDecimal(dr["valorUsuario"]);
                        item.CONTRATO_VALORIDEIA = Convert.ToDecimal(dr["valorIdeia"]);
                        item.CONTRATO_VALORVALIDACAO = Convert.ToDecimal(dr["valorValidacao"]);
                        item.CONTRATO_VALORTOTAL = Convert.ToString(dr["valorTotal"]);
                        item.CONTRATO_STATUS = dr["status"].ToString();

                        Contrato.Add(item);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return Contrato;
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
        public AdmContrato BuscarContratoSingle(string contrIdUduario, string contrIdPlano, string contrData)
        {
            var db = new ClassDb();

            try
            {
                try
                {
                    var sql = "select c.idContrato, c.idUsuario, u.nome as nomeUsuario, c.idPlano, p.titulo as tituloPlano, " +
                        "DATE_FORMAT(c.dataContrato, '%d/%m/%Y') as dataContrato, " +
                        "DATE_FORMAT(c.dataRenovacao, '%d/%m/%Y') as dataRenovacao, " +
                        "DATE_FORMAT(c.dataCancelamento, '%d/%m/%Y') as dataCancelamento, " +
                        "c.quantUsuarios, c.quantIdeias, c.quantValidacoes, " +
                        "TRUNCATE(c.valorUsuario, 2) as valorUsuario, TRUNCATE(c.valorIdeia, 2) as valorIdeia, " +
                        "TRUNCATE(c.valorValidacao, 2) as valorValidacao, TRUNCATE(c.valorTotal, 2) as valorTotal, " +
                        "case c.status when 0 then '' ELSE 'checked' END AS status " +
                        "from ecom_contratos as c JOIN usuarios as u JOIN ecom_planos as p " +
                        "ON u.id = c.idUsuario AND c.idPlano = p.idPlano AND "+
                        "c.idUsuario = @idUsuario AND c.idPlano = @idPlano AND c.dataContrato = @dataContrato ORDER BY c.IdContrato DESC LIMIT 1";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idUsuario", contrIdUduario);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    AdmContrato Contrato = new AdmContrato();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        Contrato.ID_CONTRATO = Convert.ToInt32(dr["idContrato"]);
                        Contrato.CONTRATO_IDUSUARIO = dr["idUsuario"].ToString();
                        Contrato.CONTRATO_NOMEUSUARIO = dr["nomeUsuario"].ToString();
                        Contrato.CONTRATO_IDPLANO = dr["idPlano"].ToString();
                        Contrato.CONTRATO_TITULOPLANO = dr["tituloPlano"].ToString();
                        Contrato.CONTRATO_DATACONTRATO = dr["dataContrato"].ToString();
                        Contrato.CONTRATO_DATARENOVACAO = dr["dataRenovacao"].ToString();
                        Contrato.CONTRATO_DATACANCELAMENTO = dr["dataCancelamento"].ToString();
                        Contrato.CONTRATO_QUANTUSUARIOS = dr["quantUsuarios"].ToString();
                        Contrato.CONTRATO_QUANTIDEIAS = dr["quantIdeias"].ToString();
                        Contrato.CONTRATO_QUANTVALIDACOES = dr["quantValidacoes"].ToString();
                        Contrato.CONTRATO_VALORUSUARIO = Convert.ToDecimal(dr["valorUsuario"]);
                        Contrato.CONTRATO_VALORIDEIA = Convert.ToDecimal(dr["valorIdeia"]);
                        Contrato.CONTRATO_VALORVALIDACAO = Convert.ToDecimal(dr["valorValidacao"]);
                        Contrato.CONTRATO_VALORTOTAL = Convert.ToString(dr["valorTotal"]);
                        Contrato.CONTRATO_STATUS = dr["status"].ToString();

                    }
                    return Contrato;
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

        public AdmContrato SelectContrato(int idContrato)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "select c.idContrato, c.idUsuario, u.nome as nomeUsuario, c.idPlano, p.titulo as tituloPlano, " +
                        "DATE_FORMAT(c.dataContrato, '%d/%m/%Y') as dataContrato, " +
                        "DATE_FORMAT(c.dataRenovacao, '%d/%m/%Y') as dataRenovacao, " +
                        "DATE_FORMAT(c.dataCancelamento, '%d/%m/%Y') as dataCancelamento, " +
                        "c.quantUsuarios, c.quantIdeias, c.quantValidacoes, " +
                        "TRUNCATE(c.valorUsuario, 2) as valorUsuario, TRUNCATE(c.valorIdeia, 2) as valorIdeia, " +
                        "TRUNCATE(c.valorValidacao, 2) as valorValidacao, TRUNCATE(c.valorTotal, 2) as valorTotal, " +
                        "case c.status when 0 then 'noChecked' ELSE 'checked' END AS status " +
                        "from ecom_contratos as c JOIN usuarios as u JOIN ecom_planos as p " +
                        "ON u.id = c.idUsuario AND c.idPlano = p.idPlano and idContrato = @idContrato";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@idContrato", idContrato);

                    db.AbrirConexao();
                    MySqlDataReader dr;

                    AdmContrato Contrato = new AdmContrato();
                    dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        Contrato.ID_CONTRATO = Convert.ToInt32(dr["idContrato"]);
                        Contrato.CONTRATO_IDUSUARIO = dr["idUsuario"].ToString();
                        Contrato.CONTRATO_NOMEUSUARIO = dr["nomeUsuario"].ToString();
                        Contrato.CONTRATO_IDPLANO = dr["idPlano"].ToString();
                        Contrato.CONTRATO_TITULOPLANO = dr["tituloPlano"].ToString();
                        Contrato.CONTRATO_DATACONTRATO = dr["dataContrato"].ToString();
                        Contrato.CONTRATO_DATARENOVACAO = dr["dataRenovacao"].ToString();
                        Contrato.CONTRATO_DATACANCELAMENTO = dr["dataCancelamento"].ToString();
                        Contrato.CONTRATO_QUANTUSUARIOS = dr["quantUsuarios"].ToString();
                        Contrato.CONTRATO_QUANTIDEIAS = dr["quantIdeias"].ToString();
                        Contrato.CONTRATO_QUANTVALIDACOES = dr["quantValidacoes"].ToString();
                        Contrato.CONTRATO_VALORUSUARIO = Convert.ToDecimal(dr["valorUsuario"]);
                        Contrato.CONTRATO_VALORIDEIA = Convert.ToDecimal(dr["valorIdeia"]);
                        Contrato.CONTRATO_VALORVALIDACAO = Convert.ToDecimal(dr["valorValidacao"]);
                        Contrato.CONTRATO_VALORTOTAL = Convert.ToString(dr["valorTotal"]);
                        Contrato.CONTRATO_STATUS = dr["status"].ToString();

                    }
                    return Contrato;
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

        public AdmContrato AlterarContrato(AdmContrato contrato)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "UPDATE ecom_contratos  SET idUsuario = @idUsuario, idPlano = @idPlano, dataContrato = @dataContrato, dataRenovacao = @dataRenovacao, dataCancelamento = @dataCancelamento, quantUsuarios = @quantUsuarios, quantIdeias = @quantIdeias, quantValidacoes = @quantValidacoes, valorusuario = @valorusuario, valorIdeia = @valorIdeia, valorValidacao = @valorValidacao, valorTotal = @valorTotal, status = @status) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@idUsuario", contrato.CONTRATO_IDUSUARIO);
                        cmd.Parameters.AddWithValue("@idPlano", contrato.CONTRATO_IDPLANO);
                        cmd.Parameters.AddWithValue("@dataContrato", contrato.CONTRATO_DATACONTRATO);
                        cmd.Parameters.AddWithValue("@dataRenovacao", contrato.CONTRATO_DATARENOVACAO);
                        cmd.Parameters.AddWithValue("@dataCancelamento", contrato.CONTRATO_DATACANCELAMENTO);
                        cmd.Parameters.AddWithValue("@quantUsuarios", contrato.CONTRATO_QUANTUSUARIOS);
                        cmd.Parameters.AddWithValue("@quantIdeias", contrato.CONTRATO_QUANTIDEIAS);
                        cmd.Parameters.AddWithValue("@quantValidacoes", contrato.CONTRATO_QUANTVALIDACOES);
                        cmd.Parameters.AddWithValue("@valorusuario", contrato.CONTRATO_VALORUSUARIO);
                        cmd.Parameters.AddWithValue("@valorIdeia", contrato.CONTRATO_VALORIDEIA);
                        cmd.Parameters.AddWithValue("@valorValidacao", contrato.CONTRATO_VALORVALIDACAO);
                        cmd.Parameters.AddWithValue("@valorTotal", contrato.CONTRATO_VALORTOTAL);
                        cmd.Parameters.AddWithValue("@status", contrato.CONTRATO_STATUS);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return contrato; //SelectCupom(cupom.ID_CUPOM);
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

        public AdmContrato CreateContrato(AdmContrato contrato)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO ecom_contratos (idUsuario, idPlano, dataContrato, dataRenovacao, dataCancelamento, quantUsuarios, quantIdeias, quantValidacoes, valorusuario, valorIdeia, valorValidacao, valorTotal, status) " +
                          "VALUES (@idUsuario, @idPlano, @dataContrato, @dataRenovacao, @dataCancelamento, @quantUsuarios, @quantIdeias, @quantValidacoes, @valorusuario, @valorIdeia, @valorValidacao, @valorTotal, @status) ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@idUsuario", contrato.CONTRATO_IDUSUARIO);
                        cmd.Parameters.AddWithValue("@idPlano", contrato.CONTRATO_IDPLANO);
                        cmd.Parameters.AddWithValue("@dataContrato", contrato.CONTRATO_DATACONTRATO);
                        cmd.Parameters.AddWithValue("@dataRenovacao", contrato.CONTRATO_DATARENOVACAO);
                        cmd.Parameters.AddWithValue("@dataCancelamento", contrato.CONTRATO_DATACANCELAMENTO);
                        cmd.Parameters.AddWithValue("@quantUsuarios", contrato.CONTRATO_QUANTUSUARIOS);
                        cmd.Parameters.AddWithValue("@quantIdeias", contrato.CONTRATO_QUANTIDEIAS);
                        cmd.Parameters.AddWithValue("@quantValidacoes", contrato.CONTRATO_QUANTVALIDACOES);
                        cmd.Parameters.AddWithValue("@valorusuario", contrato.CONTRATO_VALORUSUARIO);
                        cmd.Parameters.AddWithValue("@valorIdeia", contrato.CONTRATO_VALORIDEIA);
                        cmd.Parameters.AddWithValue("@valorValidacao", contrato.CONTRATO_VALORVALIDACAO);
                        cmd.Parameters.AddWithValue("@valorTotal", contrato.CONTRATO_VALORTOTAL);
                        cmd.Parameters.AddWithValue("@status", contrato.CONTRATO_STATUS);
                        cmd.ExecuteNonQuery();
                        cmd.Connection.Close();
                    }
                    return contrato;
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
        public bool DeleteContrato(AdmContrato contrato)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "DELETE FROM ecom_contratos WHERE idContrato = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", contrato.ID_CONTRATO);
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