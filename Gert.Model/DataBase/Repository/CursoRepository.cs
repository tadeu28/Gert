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
    public class CursoRepository : RepositoryBase<Curso>
    {
        public CursoRepository(ISession session) : base(session) {}
        
        public Curso FindById(int id)
        {
            return this.Session.Query<Curso>().FirstOrDefault(f => f.Id == id);
        }
    }
}
