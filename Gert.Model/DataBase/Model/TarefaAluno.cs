using Gert.Model.Enumerators;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gert.Model.DataBase.Model
{
    public class TarefaAluno
    {
        public virtual int Id { get; set; }
        public virtual SituacaoTarefaEnum Situacao { get; set; }
        public virtual DateTime? DtInicio { get; set; }
        public virtual DateTime? DtFinal { get; set; }
        public virtual Pessoa Aluno { get; set; }
        public virtual Tarefa Tarefa { get; set; }
    }

    public class TarefaAlunoMap : ClassMapping<TarefaAluno>
    {
        public TarefaAlunoMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<SituacaoTarefaEnum>(x => x.Situacao);
            Property<DateTime?>(x => x.DtInicio);
            Property<DateTime?>(x => x.DtFinal);

            ManyToOne<Pessoa>(x => x.Aluno, m => m.Column("IdPessoa"));
            ManyToOne<Tarefa>(x => x.Tarefa, m => m.Column("IdTarefa"));
        }
    }
}
