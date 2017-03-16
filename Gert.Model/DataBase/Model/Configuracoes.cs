using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.MappingModel.ClassBased;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Gert.Model.DataBase.Model
{
    public class Configuracoes
    {
        public virtual int Id { get; set; }
        public virtual string Smtp { get; set; }
        public virtual string PortaEmail { get; set; }
        public virtual string LoginEmail { get; set; }
        public virtual string SenhaEmail { get; set; }
        public virtual bool SslEmail { get; set; }
    }

    public class ConfiguracoesMap : ClassMapping<Configuracoes>
    {
        public ConfiguracoesMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<string>(x => x.Smtp);
            Property<string>(x => x.PortaEmail);
            Property<string>(x => x.LoginEmail);
            Property<string>(x => x.SenhaEmail);
            Property<bool>(x => x.SslEmail);
        }
    }
}
