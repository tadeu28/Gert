using FluentNHibernate.MappingModel.ClassBased;
using Gert.Model.Enumerators;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gert.Model.DataBase.Model
{
    public class Tarefa
    {
        public virtual int Id { get; set; }
        [Required(ErrorMessage = "O Título é obrigatório")]
        public virtual string Titulo { get; set; }
        [Required(ErrorMessage = "O Descrição é obrigatório")]
        public virtual string Descricao { get; set; }
        public virtual string Arquivo { get; set; }
        [Display(Name = "Data Inicial")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public virtual DateTime DtInicio { get; set; }
        [Required(ErrorMessage = "A Data Final é obrigatório")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Data Final")]
        public virtual DateTime DtFinal { get; set; }
        public virtual SitiacaoTarefaEnum Situacao { get; set; }
        
        public virtual Disciplina Disciplina { get; set; }

        [Required(ErrorMessage = "A Disciplina é obrigatório")]
        [Display(Name = "Disciplina")]
        public virtual int IdDisciplina { get; set; }
    }

    public class TarefaMap : ClassMapping<Tarefa>
    {
        public TarefaMap()
        {
            Id<int>(x => x.Id, map => { map.Generator(Generators.Identity); });

            Property<string>(x => x.Titulo);
            Property<string>(x => x.Descricao);
            Property<string>(x => x.Arquivo);
            Property<DateTime>(x => x.DtInicio);
            Property<DateTime>(x => x.DtFinal);
            Property<SitiacaoTarefaEnum>(x => x.Situacao);

            ManyToOne(x => x.Disciplina, map => {
                map.Cascade(Cascade.All);
                map.Column("IdDisciplina");
            });
        }
    }
}
