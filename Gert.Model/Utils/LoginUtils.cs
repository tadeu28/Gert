using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;

namespace Gert.Model.Utils
{
    public class LoginUtils
    {

        public static Usuario Logar(string Login, string Senha)
        {
            try
            {
                var user = GertDbFactory.Instance.UsuarioRepository.BuscarUsuario(Login, Senha);
                if (user == null)
                {
                    throw new Exception("Login ou Senha Inválidos");
                }

                HttpContext.Current.Session["Usuario"] = user;

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível realizar o Login do Usuário. " + ex.Message);
            }
        }

        public static void Deslogar()
        {
            HttpContext.Current.Session["Usuario"] = null;
            HttpContext.Current.Session.Remove("Usuario");
        }

        public static Usuario Usuario
        {
            get
            {
                return HttpContext.Current.Session["Usuario"] != null ? (Usuario) HttpContext.Current.Session["Usuario"] : null;
            }
        }
    }
}
