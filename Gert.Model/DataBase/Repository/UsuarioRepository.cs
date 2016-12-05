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
    public class UsuarioRepository : RepositoryBase<Usuario>
    {
        public UsuarioRepository(ISession session) : base(session) { }

        public Usuario BuscarUsuario(string login, string senha)
        {
            return this.Session.Query<Usuario>().FirstOrDefault(f => f.Login == login && f.Senha == senha);
        }
    }
}
