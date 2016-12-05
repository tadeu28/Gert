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
    public class ProfessorRepository : RepositoryBase<Professor>
    {
        public ProfessorRepository(ISession session) : base(session) { }

        public Professor FindById(int id)
        {
            return this.Session.Query<Professor>().FirstOrDefault(f => f.Id == id);
        }
    }
}
