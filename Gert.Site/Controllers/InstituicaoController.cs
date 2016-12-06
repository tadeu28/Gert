using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;
using Gert.Model.Utils;
using WebGrease.Css.Extensions;
using Gert.Model.Enumerators;

namespace Gert.Site.Controllers
{
    public class InstituicaoController : Controller
    {
        // GET: Instituicao
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CriarInstituicao()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Gravar(Instituicao instituicao)
        {
            try
            {
                var usuario = new Usuario()
                {
                    Permissao = UsuarioEnum.Instituicao,
                    Senha = "1nSt1",
                    Login = "instituicao"
                };

                GertDbFactory.Instance.InstituicaoRespository.Save(instituicao);
                GertDbFactory.Instance.UsuarioRepository.Save(usuario);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "Instituicao", "Gravar"));
            }
        }

        [HttpPost]
        public PartialViewResult Cursos()
        {
            try
            {
                var cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();

                return PartialView("_Cursos", cursos);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Cursos"));
            }
        }

        [HttpPost]
        public PartialViewResult GravarCurso(Curso curso)
        {
            try
            {
                var instituicao = GertDbFactory.Instance.InstituicaoRespository.FindFirstOrDefault();

                curso.Instituicao = instituicao;
                curso.Ativo = true;

                GertDbFactory.Instance.CursoRepository.Save(curso);

                var cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();

                return PartialView("_Cursos", cursos);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Cursos"));
            }
        }

        [HttpPost]
        public PartialViewResult ApagarCursos(string[] exclusoes)
        {
            try
            {
                var cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();
                cursos.ForEach(f =>
                {
                    if (exclusoes.ToList().Exists(e => Convert.ToInt16(e) == f.Id))
                    {
                        f.Ativo = false;
                        GertDbFactory.Instance.CursoRepository.Save(f);
                    }
                });

                cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();

                return PartialView("_Cursos", cursos);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Cursos"));
            }
        }

        [HttpPost]
        public PartialViewResult Professores()
        {            
            try
            {
                var professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();

                return PartialView("_Professores", professores);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Professores"));
            }
        }

        [HttpPost]
        public PartialViewResult GravarProfessor(Professor professor)
        {
            try
            {
                var instituicao = GertDbFactory.Instance.InstituicaoRespository.FindFirstOrDefault();

                var usuario = new Usuario()
                {
                    Professor = professor,
                    Login = professor.Nome.Substring(0, professor.Nome.Length > 5 ? 5 : professor.Nome.Length),
                    Senha = DateTime.Now.Year.ToString(),
                    Permissao = UsuarioEnum.Professor
                };

                professor.Instituicao = instituicao;
                professor.Ativa = true;

                GertDbFactory.Instance.UsuarioRepository.Save(usuario);
                
                var professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();

                return PartialView("_Professores", professores);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Professor"));
            }
        }

        [HttpPost]
        public PartialViewResult ApagarProfessores(string[] exclusoes)
        {
            try
            {
                var professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();
                professores.ForEach(f =>
                {
                    if (exclusoes.ToList().Exists(e => Convert.ToInt16(e) == f.Id))
                    {
                        f.Ativa = false;
                        GertDbFactory.Instance.ProfessorRepository.Save(f);
                    }
                });

                professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();

                return PartialView("_Professores", professores);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Cursos"));
            }
        }

        [HttpPost]
        public PartialViewResult Disciplinas()
        {
            try
            {
                var professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();
                var cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();

                var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindAll().Where(w => w.Ativa).ToList();
                
                ViewData["Professores"] = new SelectList(professores, "Id", "Nome");
                ViewData["Cursos"] = new SelectList(cursos, "Id", "Nome");

                return PartialView("_Disciplinas", disciplinas);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Disciplinas"));
            }
        }

        [HttpPost]
        public PartialViewResult GravarDisciplina(Disciplina disciplina)
        {
            try
            {
                disciplina.Curso = GertDbFactory.Instance.CursoRepository.FindById(disciplina.IdCurso);
                disciplina.Professor = GertDbFactory.Instance.ProfessorRepository.FindById(disciplina.IdProfessor);
                disciplina.Ativa = true;

                GertDbFactory.Instance.DisciplinaRepository.Save(disciplina);

                var professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();
                var cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();
                var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindAll().Where(w => w.Ativa).ToList();

                ViewData["Professores"] = new SelectList(professores, "Id", "Nome");
                ViewData["Cursos"] = new SelectList(cursos, "Id", "Nome");

                return PartialView("_Disciplinas", disciplinas);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Disciplinas"));
            }
        }

        [HttpPost]
        public PartialViewResult ApagarDisciplinas(string[] exclusoes)
        {
            try
            {
                var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindAll().Where(w => w.Ativa).ToList();
                disciplinas.ForEach(f =>
                {
                    if (exclusoes.ToList().Exists(e => Convert.ToInt16(e) == f.Id))
                    {
                        f.Ativa = false;
                        GertDbFactory.Instance.DisciplinaRepository.Save(f);
                    }
                });

                disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindAll().Where(w => w.Ativa).ToList();
                var professores = GertDbFactory.Instance.ProfessorRepository.FindAll().Where(w => w.Ativa).ToList();
                var cursos = GertDbFactory.Instance.CursoRepository.FindAll().Where(w => w.Ativo).ToList();

                ViewData["Professores"] = new SelectList(professores, "Id", "Nome");
                ViewData["Cursos"] = new SelectList(cursos, "Id", "Nome");

                return PartialView("_Disciplinas", disciplinas);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Cursos"));
            }
        }

        [HttpPost]
        public PartialViewResult Tarefas(int id)
        {
            try
            {
                var tarefas = GertDbFactory.Instance.TarefaRepository.FindByIdDisciplina(id);
                tarefas = tarefas.Where(w => w.Ativo).ToList();

                return PartialView("_Tarefas", tarefas);
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Instituicao", "Tarefas"));
            }
        }
    }
}