using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Gert.Model.DataBase.Model
{
    public class Pessoa
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Nome é obrigatório")]
        public virtual string Nome { get; set; }
        [Required(ErrorMessage = "Matrícula é obrigatória")]
        public virtual string Matricula { get; set; }
        public virtual Boolean Ativa { get; set; }
        public virtual Usuario Usuario { get; set; }
    }

    public class PessoaMap : ClassMapping<Pessoa>
    {
        public PessoaMap()
        {
            Id<int>(x => x.Id, map => map.Generator(Generators.Identity));

            Property<string>(x => x.Nome);
            Property<Boolean>(x => x.Ativa);
        }
    }
}
