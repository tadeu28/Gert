using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gert.Model.DataBase.Model
{
    public class DisciplinaAluno
    {
        public virtual int Id { get; set; }
        public virtual Pessoa Aluno { get; set; }
        public virtual Disciplina Disciplina { get; set; }
        public virtual DateTime DtMatricula { get; set; }
        public virtual Boolean Ativo { get; set; }
    }

    public class DisciplinaAlunoMap : ClassMapping<DisciplinaAluno>
    {
        public DisciplinaAlunoMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<DateTime>(x => x.DtMatricula);
            Property<Boolean>(x => x.Ativo);

            ManyToOne<Pessoa>(x => x.Aluno, m => m.Column("IdPessoa"));
            ManyToOne<Disciplina>(x => x.Disciplina, m => m.Column("IdDisciplina"));
        }
    }
}
