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
    public class TarefaRepository : RepositoryBase<Tarefa>
    {
        public TarefaRepository(ISession session) : base(session) { }

        public IList<Tarefa> FindById(int id)
        {
            return this.Session.Query<Tarefa>().Where(w => w.Id == id).ToList();
        }

        public IList<Tarefa> FindByIdProfessor(int id)
        {
            return this.Session.Query<Tarefa>().Where(w => w.Disciplina.Professor.Id == id).ToList();
        }

        public IList<Tarefa> FindByIdDisciplina(int id)
        {
            return this.Session.Query<Tarefa>().Where(w => w.Disciplina.Id == id).ToList();
        }
    }
}
