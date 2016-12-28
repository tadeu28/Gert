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
        [DataType(DataType.EmailAddress, ErrorMessage = "Este não é um e-mail válido")]
        public virtual string Email { get; set; }
        [Required(ErrorMessage = "Matrícula é obrigatória")]
        public virtual string Matricula { get; set; }
        public virtual Boolean Ativa { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual IList<DisciplinaAluno> Disciplinas { get; set; }
        public virtual IList<TarefaAluno> Tarefas { get; set; }

        public Pessoa()
        {
            this.Disciplinas = new List<DisciplinaAluno>();
            this.Tarefas = new List<TarefaAluno>();
        }
    }

    public class PessoaMap : ClassMapping<Pessoa>
    {
        public PessoaMap()
        {
            Id<int>(x => x.Id, map => map.Generator(Generators.Identity));

            Property<string>(x => x.Nome);
            Property<string>(x => x.Matricula);
            Property<string>(x => x.Email);
            Property<Boolean>(x => x.Ativa);

            Bag<DisciplinaAluno>(x => x.Disciplinas, m =>
            {
                m.Cascade(Cascade.All);
                m.Inverse(true);
                m.Lazy(CollectionLazy.NoLazy);
                m.Key(k => k.Column("IdPessoa"));
            },
            o => o.OneToMany());

            Bag<TarefaAluno>(x => x.Tarefas, m =>
            {
                m.Cascade(Cascade.All);
                m.Inverse(true);
                m.Lazy(CollectionLazy.NoLazy);
                m.Key(k => k.Column("IdPessoa"));
            },
            o => o.OneToMany());
        }
    }
}
