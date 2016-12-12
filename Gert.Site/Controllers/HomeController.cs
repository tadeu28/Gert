using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;
using Gert.Model.Utils;
using Gert.Model.Enumerators;

namespace Gert.Site.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                var instituicoes = GertDbFactory.Instance.InstituicaoRespository.FindAll();

                if (instituicoes.Count == 0)
                {
                    return RedirectToAction("CriarInstituicao", "Instituicao");
                }
                
                return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Index"));
            }
        }
        
        public ActionResult VerificarLogin(string login)
        {
            bool loginExiste = false;
            try
            {
                loginExiste = GertDbFactory.Instance.UsuarioRepository.FindAll().Count(f => f.Login.ToLower() == login.ToLower()) > 0;

                return Json(!loginExiste, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Gravar(Pessoa pessoa, Usuario usuario)
        {
            try
            {
                usuario.Pessoa = pessoa;
                usuario.Permissao = UsuarioEnum.Aluno;

                var user = GertDbFactory.Instance.UsuarioRepository.Save(usuario);

                LoginUtils.Logar(user.Login, user.Senha);

                return RedirectToAction("Index", "Aluno");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Home", "Index"));
            }
        }

        [HttpPost]
        public ActionResult Login(Usuario usuario)
        {
            try
            {
                LoginUtils.Logar(usuario.Login, usuario.Senha);

                var url = Request.Url.AbsoluteUri.Replace("Login", "Index");

                if (LoginUtils.Usuario.Permissao == UsuarioEnum.Instituicao)
                {
                    url = url.Replace("Home", "Instituicao");
                }
                else if (LoginUtils.Usuario.Permissao == UsuarioEnum.Professor)
                {
                    url = url.Replace("Home", "Professor");
                    LoginUtils.Usuario.Professor.Usuario = usuario;
                }
                else
                {
                    url = url.Replace("Home", "Aluno");
                }

                return Json(new { success = true, Message = url});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = "" + ex.Message });
            }
        }

        public ActionResult Deslogar()
        {
            LoginUtils.Deslogar();

            return RedirectToAction("Index");
        }
    }
}