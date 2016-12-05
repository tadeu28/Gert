using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Gert.Model.DataBase.Model
{
    public class Curso
    {
        public virtual int Id { get; set; }
        public virtual string Nome { get; set; }
        public virtual Boolean Ativo { get; set; }
        public virtual Instituicao Instituicao { get; set; }

    }

    public class CursoMap : ClassMapping<Curso>
    {
        public CursoMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<string>(x => x.Nome);
            Property<Boolean>(x => x.Ativo);

            ManyToOne(x => x.Instituicao, map =>
            {
                map.Column("IdInstituicao");
                map.Cascade(Cascade.All);                
            });
        }
    }
}
