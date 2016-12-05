using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gert.Model.DataBase.Model;
using NHibernate;

namespace Gert.Model.DataBase.Repository
{
    public class InstituicaoRespository : RepositoryBase<Instituicao>
    {
        public InstituicaoRespository(ISession session) : base(session) {}
    }
}
