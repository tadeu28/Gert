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
    public class Disciplina
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "Descrição é orbigatória")]
        [Display(Name = "Descrição")]
        public virtual string Nome { get; set; }

        [Range(2016, 2017, ErrorMessage = "O Ano não pode ser menor que 2017")]
        [Required(ErrorMessage = "O Ano é Obrigatório")]
        public virtual int Ano { get; set; }

        [Range(1, 2, ErrorMessage = "O Semestre não deve ser 1 ou 2")]
        [Required(ErrorMessage = "O Semestre é Obrigatório")]
        public virtual int Semestre { get; set; }
        public virtual Boolean Ativa { get; set; }

        public virtual Curso Curso { get; set; }
        public virtual Professor Professor { get; set; }

        [Display(Name ="Curso")]
        [Required(ErrorMessage = "O Curso é Obrigatório")]
        public virtual int IdCurso { get; set; }

        [Required(ErrorMessage = "O Professor é Obrigatório")]
        [Display(Name = "Professor")]
        public virtual int IdProfessor { get; set; }

        public virtual IList<Tarefa> Tarefas { get; set; }

        public Disciplina()
        {
            this.Tarefas = new List<Tarefa>();
        }
    }

    public class DisciplinaMap : ClassMapping<Disciplina>
    {
        public DisciplinaMap()
        {
            Id<int>(x => x.Id, m => m.Generator(Generators.Identity));

            Property<string>(x => x.Nome);
            Property<int>(x => x.Ano);
            Property<Boolean>(x => x.Ativa);
            Property<int>(x => x.Semestre);

            ManyToOne<Curso>(x => x.Curso, map =>
            {
                map.Column("IdCurso");
                map.Cascade(Cascade.All);
            });

            ManyToOne<Professor>(x => x.Professor, map =>
            {
                map.Column("IdProfessor");
                map.Cascade(Cascade.All);
            });

            Bag<Tarefa>(x => x.Tarefas, map =>
            {
                map.Cascade(Cascade.All);
                map.Inverse(true);
                map.Lazy(CollectionLazy.NoLazy);
                map.Key(k => k.Column("IdDisciplina"));
            }, o => o.OneToMany());
        }
    }
}
