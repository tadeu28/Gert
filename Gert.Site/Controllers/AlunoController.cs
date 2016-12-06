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
                var usuario = LoginUtils.Usuario;
                var disciplinasAluno = GertDbFactory.Instance.DisciplinaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id);
                var disciplinas = GertDbFactory.Instance.DisciplinaRepository.FindAll();
                disciplinas = disciplinas.Where(w => w.Ativa).ToList();

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
                if(tipo == 0)
                {
                    var usuario = LoginUtils.Usuario;
                    var disciplinasAluno = GertDbFactory.Instance.DisciplinaAlunoRepository.FindByIdAluno(usuario.Pessoa.Id);

                    var matriculada = disciplinasAluno.FirstOrDefault(f => f.Disciplina.Id == id);
                    if (matriculada == null)
                    {
                        return Json(new { success = true, Message = "Ok" });
                    }

                    GertDbFactory.Instance.DisciplinaAlunoRepository.Delete(matriculada);
                }else
                {
                    var disciplina = GertDbFactory.Instance.DisciplinaRepository.FindById(id);

                    if (disciplina == null)
                    {
                        return Json(new { success = true, Message = "Ok" });
                    }

                    var usuario = LoginUtils.Usuario;

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
    }
}