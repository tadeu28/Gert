using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;
using Gert.Model.Utils;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gert.Site.Controllers
{
    public class ProfessorController : Controller
    {
        // GET: Professor
        public ActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public PartialViewResult Perfil()
        {
            try
            {
                return PartialView("_Perfil", LoginUtils.Usuario.Professor);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Professor", "Perfil"));
            }
        }

        [HttpPost]
        public ActionResult AlterarUsuario(Professor professor, Usuario usuario)
        {
            try
            {
                var user = LoginUtils.Usuario;
                user.Login = usuario.Login;
                user.Senha = usuario.Senha;
                user.Professor.Nome = professor.Nome;

                GertDbFactory.Instance.UsuarioRepository.Save(user);

                LoginUtils.Deslogar();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Professor", "AlterarUsuario"));
            }
        }

        public ActionResult TarefasReport(int id)
        {
            var tarefas = GertDbFactory.Instance.TarefaRepository.FindByIdProfessor(id);
            tarefas = tarefas.Where(w => w.Ativo).ToList();

            var pdf = new ViewAsPdf()
            {
                ViewName = "TarefasReport",
                Model = tarefas,
                PageSize = Rotativa.Options.Size.A4,
                PageOrientation = Rotativa.Options.Orientation.Landscape                
            };

            return pdf;
        }

        [HttpPost]
        public PartialViewResult Tarefas()
        {
            try
            {
                var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindByIdProfessor(LoginUtils.Usuario.Professor.Id);

                var tarefas = GertDbFactory.Instance.TarefaRepository.FindByIdProfessor(LoginUtils.Usuario.Professor.Id);
                tarefas = tarefas.Where(w => w.Ativo).ToList();

                ViewData["Disciplinas"] = new SelectList(disciplinas, "Id", "Nome");
                return PartialView("_Tarefas", tarefas);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Cursos"));
            }
        }

        [HttpPost]
        public ActionResult GravarTarefa(Tarefa tarefa, HttpPostedFileBase Arquivo)
        {
            try
            {
                tarefa.Disciplina = GertDbFactory.Instance.DisciplinaRepository.FindById(tarefa.IdDisciplina);
                tarefa.Arquivo = null;
                tarefa.Ativo = true;
                tarefa = GertDbFactory.Instance.TarefaRepository.Save(tarefa);

                if (Arquivo != null && Arquivo.ContentLength > 0)
                {
                    var fileName = "tarefa" + tarefa.Id + "_disciplina" + tarefa.IdDisciplina + Path.GetExtension(Arquivo.FileName);

                    var arquivo = FileUtils.GravarArquivo(Arquivo, fileName);
                    if (System.IO.File.Exists(arquivo))
                    {
                        tarefa.Arquivo = fileName;
                        tarefa = GertDbFactory.Instance.TarefaRepository.Save(tarefa);
                    }
                }
                                
                return RedirectToAction("AtualizarTerafas");
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Professor", "GravarTarefa"));
            }
        }

        public ActionResult AtualizarTerafas()
        {            
            var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindByIdProfessor(LoginUtils.Usuario.Professor.Id);
            var tarefas = GertDbFactory.Instance.TarefaRepository.FindByIdProfessor(LoginUtils.Usuario.Professor.Id);

            tarefas = tarefas.Where(w => w.Ativo).ToList();

            ViewData["Tarefas"] = tarefas;
            ViewData["Disciplinas"] = new SelectList(disciplinas, "Id", "Nome");
            ViewBag.Tarefa = true;

            return View("Index");
        }

        [HttpPost]
        public ActionResult ApagarTarefas(string[] exclusoes)
        {
            try
            {
                var tarefas = GertDbFactory.Instance.TarefaRepository.FindAll().Where(w => w.Ativo).ToList();
                tarefas.ForEach(f =>
                {
                    if (exclusoes.ToList().Exists(e => Convert.ToInt16(e) == f.Id))
                    {
                        f.Ativo = false;
                        GertDbFactory.Instance.TarefaRepository.Save(f);
                    }
                });

                return RedirectToAction("AtualizarTerafas");
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "ApagarTrabalhos"));
            }
        }
    }
}