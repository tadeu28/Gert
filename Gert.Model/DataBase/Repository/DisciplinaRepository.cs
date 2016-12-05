using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gert.Model.DataBase.Model;
using NHibernate;
using NHibernate.Linq;

namespace Gert.Model.DataBase.Repository
{
    public class DisciplinaRepository : RepositoryBase<Disciplina>
    {
        public DisciplinaRepository(ISession session) : base(session) {}

        public Disciplina FindById(int id)
        {
            return this.Session.Query<Disciplina>().FirstOrDefault(f => f.Id == id);
        }

        public IList<Disciplina> FindByIdProfessor(int id)
        {
            return this.Session.Query<Disciplina>().Where(w => w.Professor.Id == id).ToList();
        }
    }
}
