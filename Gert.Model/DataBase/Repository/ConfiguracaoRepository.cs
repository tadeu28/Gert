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
    public class ConfiguracaoRepository : RepositoryBase<Configuracoes>
    {
        public ConfiguracaoRepository(ISession session) : base(session){}

        public Configuracoes Find()
        {
            return this.Session.Query<Configuracoes>().FirstOrDefault();
        }
    }
}
