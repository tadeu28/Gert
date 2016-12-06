using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;
using Gert.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gert.Site.Controllers
{
    public class AlunoController : Controller
    {
        // GET: Aluno
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public PartialViewResult Disciplinas()
        {
            try
            {
                var semestre = (DateTime.Now.Month <= 6 ? 1 : 2);
                var usuario = LoginUtils.Usuario;
                
                var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindAll();
                disciplinas = disciplinas.Where(w => w.Ativa && w.Semestre == semestre && w.Ano == DateTime.Now.Year).ToList();

                var disciplinasAluno = GertDbFactory.Instance.DisciplinaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id);
                disciplinasAluno = disciplinasAluno.Where(w => w.Disciplina.Ativa && w.Disciplina.Semestre == semestre && w.Disciplina.Ano == DateTime.Now.Year).ToList();

                var naoMatriculadas = new List<Disciplina>();
                var matriculadas = new List<Disciplina>();
                foreach (var d in disciplinas)
                {
                    var da = disciplinasAluno.FirstOrDefault(f => f.Disciplina.Id == d.Id);
                    if (da == null)
                    {
                        naoMatriculadas.Add(d);
                    }else
                    {
                        matriculadas.Add(d);
                    }
                }

                ViewBag.Matriculadas = matriculadas;
                return PartialView("_Disciplinas", naoMatriculadas);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Aluno", "Disciplinas"));
            }
        }

        public ActionResult GravarDisciplina(int id, int tipo)
        {
            try
            {
                var usuario = LoginUtils.Usuario;
                var disciplinasAluno = GertDbFactory.Instance.DisciplinaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id);

                if (tipo == 0)
                {
                    

                    var matriculada = disciplinasAluno.FirstOrDefault(f => f.Disciplina.Id == id);
                    if (matriculada == null)
                    {
                        return Json(new { success = true, Message = "Ok" });
                    }

                    if(matriculada.Disciplina.Tarefas.Count > 0)
                    {
                        return Json(new { success = false, error = true, Message = "Não é possível remover uma disciplina que já contenha tarefas cadastradas." });
                    }

                    GertDbFactory.Instance.DisciplinaAlunoRepository.Delete(matriculada);
                }else
                {
                    var disciplina = GertDbFactory.Instance.DisciplinaRepository.FindById(id);

                    if (disciplina == null)
                    {
                        return Json(new { success = false, Message = "Disciplina não encontrada." });
                    }

                    var matriculada = disciplinasAluno.FirstOrDefault(f => f.Disciplina.Id == id);
                    if (matriculada != null)
                    {
                        return Json(new { success = true, Message = "Ok" });
                    }

                    var matricula = new DisciplinaAluno()
                    {
                        Aluno = usuario.Pessoa,
                        Disciplina = disciplina,
                        DtMatricula = DateTime.Now,
                        Ativo = true
                    };

                    GertDbFactory.Instance.DisciplinaAlunoRepository.Save(matricula);
                }
                
                return Json(new { success = true, Message = "Ok"});
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = "Não foi possível gravar a discplina!" + ex.Message });
            }
        }

        [HttpPost]
        public PartialViewResult Perfil()
        {
            try
            {
                var user = LoginUtils.Usuario;
                user.Pessoa.Usuario = user;

                return PartialView("_Perfil", user.Pessoa);
            }catch(Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Aluno", "Perfil"));
            }
        }

        [HttpPost]
        public ActionResult AlterarAluno(Pessoa pessoa, Usuario usuario)
        {
            try
            {
                var user = LoginUtils.Usuario;
                user.Login = usuario.Login;
                user.Senha = usuario.Senha;

                user.Pessoa = pessoa;
                GertDbFactory.Instance.UsuarioRepository.Save(user);

                LoginUtils.Logar(user.Login, user.Senha);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Aluno", "AlterarAluno"));
            }
        }
    }
}