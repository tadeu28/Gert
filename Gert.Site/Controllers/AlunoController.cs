using Gert.Model.DataBase;
using Gert.Model.DataBase.Model;
using Gert.Model.Enumerators;
using Gert.Model.Utils;
using Newtonsoft.Json;
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

        private void ListaTarefasAluno(ref List<Tarefa> tarefasAFazer, ref List<TarefaAluno> tarefasFazendo, ref List<TarefaAluno> tarefasFeito)
        {
            try
            {
                var semestre = (DateTime.Now.Month <= 6 ? 1 : 2);
                var usuario = LoginUtils.Usuario;

                var disciplinasAluno = GertDbFactory.Instance.DisciplinaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id);
                disciplinasAluno = disciplinasAluno.Where(w => w.Disciplina.Ativa && w.Disciplina.Semestre == semestre && w.Disciplina.Ano == DateTime.Now.Year).ToList();

                var tarefas = GertDbFactory.Instance.TarefaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id).Where(w => w.Tarefa.Ativo).ToList();
                tarefasFazendo = tarefas.Where(w => w.Situacao == SituacaoTarefaEnum.DESENVOLVENDO).ToList();
                tarefasFeito = tarefas.Where(w => w.Situacao == SituacaoTarefaEnum.FEITO).ToList();
                
                foreach(var f in disciplinasAluno)
                {
                    var t = f.Disciplina.Tarefas.Where(w => w.Ativo && w.Tarefas.Count() == 0);

                    if (t.ToList().Count() != 0)
                    {
                        tarefasAFazer.InsertRange(0, t);
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult BuscarQtdeTarefa()
        {
            try
            {
                var tarefasAFazer = new List<Tarefa>();
                var tarefasFazendo = new List<TarefaAluno>();
                var tarefasFeito = new List<TarefaAluno>();

                this.ListaTarefasAluno(ref tarefasAFazer, ref tarefasFazendo, ref tarefasFeito);

                return Json(new { success = true, Message = tarefasAFazer.Count + tarefasFazendo.Count });
            }
            catch (Exception ex)
            {
                return PartialView("Error", new HandleErrorInfo(ex, "Aluno", "BuscarQtdeTarefa"));
            }
        }

        [HttpPost]
        public PartialViewResult Trabalhos()
        {
            try {
                var tarefasAFazer = new List<Tarefa>();
                var tarefasFazendo = new List<TarefaAluno>();
                var tarefasFeito = new List<TarefaAluno>();

                this.ListaTarefasAluno(ref tarefasAFazer, ref tarefasFazendo, ref tarefasFeito);

                ViewBag.AFazer = tarefasAFazer.OrderBy(o => o.DtFinal).ToList();
                ViewBag.Fazendo = tarefasFazendo.OrderBy(o => o.Tarefa.DtFinal).ToList();
                ViewBag.Feito = tarefasFeito.OrderBy(o => o.Tarefa.DtFinal).ToList(); ;

                return PartialView("_Trabalhos");

            }catch (Exception ex) {
                return PartialView("Error", new HandleErrorInfo(ex, "Aluno", "Trabalhos"));
            }
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

        public ActionResult AlterarStatusTarefa(int id, int tipo)
        {
            try
            {
                var usuario = LoginUtils.Usuario;

                var tarefaDisciplina = GertDbFactory.Instance.TarefaRepository.FindById(id);

                var tarefasAluno = GertDbFactory.Instance.TarefaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id);
                var tarefa = tarefasAluno.FirstOrDefault(f => f.Tarefa.Id == id); 
                
                if(tarefa == null)
                {
                    tarefa = new TarefaAluno()
                    {
                        Aluno = usuario.Pessoa,
                        Tarefa = tarefaDisciplina,
                        Situacao = SituacaoTarefaEnum.PENDENTE
                    };
                }

                var dias = tarefa.Tarefa.DtFinal.Subtract(DateTime.Now).Days;

                if (tipo == 0)
                {   
                    GertDbFactory.Instance.TarefaAlunoRepository.Delete(tarefa, tarefa.Id);                    

                    return Json(new { success = true, Message = new { dia = dias } });
                }
                else if (tipo == -1)
                {
                    if(tarefa.Situacao == SituacaoTarefaEnum.DESENVOLVENDO)
                    {
                        return Json(new { success = true, Message = new { dia = dias } });
                    }

                    tarefa.Situacao = SituacaoTarefaEnum.DESENVOLVENDO;
                    tarefa.DtInicio = DateTime.Now;
                    tarefa.DtFinal = null;
                }else
                {
                    if (tarefa.Situacao == SituacaoTarefaEnum.FEITO)
                    {
                        return Json(new { success = true, Message = new { dia = dias } });
                    }

                    tarefa.Situacao = SituacaoTarefaEnum.FEITO;
                    tarefa.DtFinal = DateTime.Now;
                    tarefa.DtInicio = tarefa.DtInicio == null ? DateTime.Now : tarefa.DtInicio;
                }

                GertDbFactory.Instance.TarefaAlunoRepository.Save(tarefa);

                return Json(new { success = true, Message = new { dia = dias } });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = "Não foi possível alterar a tarefa!" + ex.Message });
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

        public ActionResult BuscarInfoTarefa(int id)
        {
            try
            {
                var usuario = LoginUtils.Usuario;
                var tarefa = GertDbFactory.Instance.TarefaRepository.FindById(id);
                if(tarefa != null)
                {
                    var t = new
                    {
                        id = tarefa.Id,
                        titulo = tarefa.Titulo,
                        desc = tarefa.Descricao,
                        inicio = tarefa.DtInicio.ToShortDateString(),
                        entrega = tarefa.DtFinal.ToShortDateString(),
                        dias = tarefa.DtFinal.Subtract(DateTime.Now).Days + 1,
                        arquivo = (tarefa.Arquivo != null ? Url.Content(FileUtils.GetFilePath(tarefa.Arquivo)) : ""),
                        disciplina = tarefa.Disciplina.Nome,
                        professor = tarefa.Disciplina.Professor.Nome
                    };

                    return Json(new { success = false,
                        tarefa = t
                    });
                }
                                
                return Json(new { success = false, Message = "Tarefa não encontada." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, Message = "Não foi possível gravar a discplina!" + ex.Message });
            }
        }
    }
}