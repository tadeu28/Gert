using Gert.Model.DataBase.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Gert.Model.Utils
{
    public class FileUtils
    {
        public static string GravarArquivo(HttpPostedFileBase Arquivo, string fileName)
        {
            try
            {
                var path = "";
                if (Arquivo != null && Arquivo.ContentLength > 0)
                {
                    path = Path.Combine(HttpContext.Current.Server.MapPath("~/Uploads/Tarefas/"), fileName);
                    Arquivo.SaveAs(path);
                }

                return path;
            }
            catch(Exception ex)
            {
                throw new Exception("Não foi possível realizar o upload do arquivo.", ex);
            }            
        }

        public static string GetFilePath(string fileName)
        {
            //var url = String.Format("{0}://{1}{2}", HttpContext.Current.Request.re questUri.Scheme, Url.Request.RequestUri.Authority, ControllerContext.Configuration.VirtualPathRoot);
            return Path.Combine("Uploads/Tarefas/", fileName);
        }
    }
}
