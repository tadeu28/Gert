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
    public class DisciplinaAlunoRepository : RepositoryBase<DisciplinaAluno>
    {
        public DisciplinaAlunoRepository(ISession session) : base(session) { }

        public DisciplinaAluno FindById(int id)
        {
            return this.Session.Query<DisciplinaAluno>().FirstOrDefault(f => f.Id == id);
        }

        public IList<DisciplinaAluno> FindByIdAluno(int id)
        {
            return this.Session.Query<DisciplinaAluno>().Where(f => f.Aluno.Id == id).ToList();
        }
    }
}
