using Gert.Model.DataBase.Model;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gert.Model.DataBase.Repository
{
    public class TarefaAlunoRepository : RepositoryBase<TarefaAluno>
    {
        public TarefaAlunoRepository(ISession session) : base(session) { }

        public IList<TarefaAluno> FindById(int id)
        {
            return this.Session.Query<TarefaAluno>().Where(w => w.Id == id).ToList();
        }

        public IList<TarefaAluno> FindByIdTarefa(int id)
        {
            return this.Session.Query<TarefaAluno>().Where(w => w.Tarefa.Id == id).ToList();
        }

        public IList<TarefaAluno> FindByIdDisciplina(int id)
        {
            return this.Session.Query<TarefaAluno>().Where(w => w.Tarefa.Disciplina.Id == id).ToList();
        }

        public IList<TarefaAluno> FindByIdAluno(int id)
        {
            return this.Session.Query<TarefaAluno>().Where(w => w.Aluno.Id == id).ToList();
        }
    }
}
