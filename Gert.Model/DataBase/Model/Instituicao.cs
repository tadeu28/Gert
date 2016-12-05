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
    public class Instituicao
    {
        public virtual int Id { get; set; }
        
        [Required(ErrorMessage = "Nome da Instituição é Obrigatório", AllowEmptyStrings = false)]
        public virtual string Nome { get; set; }
        public virtual string Imagem { get; set; }
    }

    public class InstituicaoMap : ClassMapping<Instituicao>
    {
        public InstituicaoMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<string>(x => x.Nome);
            Property<string>(x => x.Imagem);
        }
    }
}
