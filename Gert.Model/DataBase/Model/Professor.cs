using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System.ComponentModel.DataAnnotations;

namespace Gert.Model.DataBase.Model
{
    public class Professor
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        public virtual string Nome { get; set; }
        public virtual Boolean Ativa { get; set; }
        public virtual Instituicao Instituicao { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

    public class ProfessorMap : ClassMapping<Professor>
    {
        public ProfessorMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<string>(x => x.Nome);
            Property<Boolean>(x => x.Ativa);

            ManyToOne(x => x.Instituicao, map =>
            {
                map.Column("IdInstituicao");
                map.Cascade(Cascade.All);
            });
        }
    }
}
