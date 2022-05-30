using System;
using System.IO;
using System.Reflection;
namespace WebApp.Models
{
    public class RegistraLog
    {
        private static string caminhoExe = string.Empty;
        public static bool Log(string strMensagem, string strNomeArquivo = "ArquivoLog")
        {
            try
            {
                caminhoExe = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                caminhoExe = System.Web.HttpContext.Current.Server.MapPath("/Logs");
                string caminhoArquivo = Path.Combine(caminhoExe, strNomeArquivo);
                if (!File.Exists(caminhoArquivo))
                {
                    FileStream arquivo = File.Create(caminhoArquivo);
                    arquivo.Close();
                }
                using (StreamWriter w = File.AppendText(caminhoArquivo))
                {
                    AppendLog(strMensagem, w);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        private static void AppendLog(string logMensagem, TextWriter txtWriter)
        {
            try
            {
                txtWriter.Write("\r\n");
                txtWriter.WriteLine($"  :{logMensagem}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}