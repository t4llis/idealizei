using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WebApp.Models
{
    public class IdeiaSolucoes
    {
        public string ID { get; set; }
        public string SOLUCAO { get; set; }
        public int ID_PROJETO { get; set; }
        public string ID_SOLUCAO_SELECIONADO { get; set; }

        public IdeiaSolucoes()
        {
            ID = "0";
            SOLUCAO = "";
            ID_PROJETO = 0;
            ID_SOLUCAO_SELECIONADO = "0";
        }

        public List<IdeiaSolucoes> BuscarIdeiaSolucoes(string idIdeia)
        {
            var db = new ClassDb();
            var listaSolucoes = new List<IdeiaSolucoes>();
            try
            {
                try
                {
                    var sql = " select s.id,                                  " +
                              "        s.solucao,                             " +
                              "        s.id_projeto,                          " +
                              "        p.id_solucoes                          " +
                              "  from  solucoes s                             " +
                              "  left join projetos p on p.id_solucoes = s.id " +
                              " where s.id_projeto = @projeto                 " +
                              " order by s.id                                 ";

                    MySqlCommand cmd = new MySqlCommand(sql, db._conn);
                    cmd.Parameters.AddWithValue("@projeto", idIdeia);

                    db.AbrirConexao();
                    MySqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    while (dr.Read())
                    {
                        var solucao = new IdeiaSolucoes();
                        solucao.ID = dr["id"].ToString();
                        solucao.SOLUCAO = dr["solucao"].ToString();
                        solucao.ID_PROJETO = Convert.ToInt32(dr["id_projeto"]);
                        solucao.ID_SOLUCAO_SELECIONADO = dr["id_solucoes"].ToString();
                        listaSolucoes.Add(solucao);
                    }
                    cmd.Connection.Close();
                    db.FecharConexao();

                    return listaSolucoes;
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

        public bool Salvar(List<IdeiaSolucoes> listaSolucoes)
        {            
            try
            {
                if (listaSolucoes != null)
                {                    
                    List<IdeiaSolucoes> listaSolucoesProjetoAtual = BuscarIdeiaSolucoes(listaSolucoes[0].ID_PROJETO.ToString());
                    if (listaSolucoesProjetoAtual.Count == 0)
                    {
                        CadastrarSolucao(listaSolucoes);

                        return true;
                    }

                    var listaIdSolucoesProjetoAtual = listaSolucoesProjetoAtual.Select(i => i.ID).ToList();
                    var listaIdSolucoesAtualizacao = listaSolucoes.Select(i => i.ID).ToList();
                    var diferencaListaSolucoes = listaIdSolucoesProjetoAtual.Except(listaIdSolucoesAtualizacao).ToList();

                    DeletarSolucoes(diferencaListaSolucoes);

                    CadastrarSolucao(listaSolucoes);

                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        private void DeletarSolucoes(List<String> listaIdSolucoes)
        {
            foreach (var idSolucao in listaIdSolucoes)
            {
                Deletar(Convert.ToInt32(idSolucao));
            }
        }

        private void CadastrarSolucao(List<IdeiaSolucoes> listaSolucoes)
        {
            int id;
            foreach (var solucao in listaSolucoes)
            {
                try
                {
                    id = Convert.ToInt32(solucao.ID);
                }
                catch
                {
                    solucao.ID = "0";
                }

                if (Convert.ToInt32(solucao.ID) == 0)
                {
                    Cadastrar(solucao);
                }
            }
        }

        public void Cadastrar(IdeiaSolucoes param)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "INSERT INTO solucoes (solucao, id_projeto)   " +
                              "           VALUES(@solucao, @id_projeto)     ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@solucao", param.SOLUCAO);
                        cmd.Parameters.AddWithValue("@id_projeto", param.ID_PROJETO);
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

        public void Deletar(int id)
        {
            var db = new ClassDb();
            try
            {
                try
                {                    
                    var sql = "DELETE FROM solucoes WHERE id = @id";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@id", id);
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

        public bool EditSolucaoProjeto(string idSolucao, int idIdeia)
        {
            var db = new ClassDb();
            try
            {
                try
                {
                    var sql = "update projetos                  " +
                              "   set id_solucoes = @idSolucao " +
                              " where id = @idIdeia ";
                    using (MySqlCommand cmd = new MySqlCommand(sql, db._conn))
                    {
                        db.AbrirConexao();
                        cmd.Parameters.AddWithValue("@idSolucao", idSolucao);
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