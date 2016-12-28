using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Mapping.ByCode;
using Gert.Model.Enumerators;

namespace Gert.Model.DataBase.Model
{
    public class Usuario
    {
        public virtual int? Id { get; set; }
        public virtual Pessoa Pessoa { get; set; }
        public virtual Professor Professor { get; set; }
        [Required(ErrorMessage = "Login é obrigatório")]
        [System.Web.Mvc.Remote("VerificarLogin", "Home", ErrorMessage = "Este login já existe no sistema, por favor, informe outro.")]
        public virtual string Login { get; set; }
        [Required(ErrorMessage = "Senha é obrigatória")]
        [DataType(DataType.Password)]
        public virtual string Senha { get; set; }
        [Compare("Senha", ErrorMessage = "As senhas são diferentes")]
        [Display(Name = "Confirmar Senha")]
        [DataType(DataType.Password)]
        public virtual string ConfirmarSenha { get; set; }
        public virtual UsuarioEnum Permissao { get; set; }
    }

    public class UsuarioMap : ClassMapping<Usuario>
    {
        public UsuarioMap()
        {
            Id(x => x.Id, map => map.Generator(Generators.Identity));

            Property(x => x.Login);
            Property(x => x.Senha);
            Property(x => x.Permissao);

            ManyToOne(x => x.Pessoa, map =>
            {
                map.Column("IdPessoa");
                map.Cascade(Cascade.All);
                map.Lazy(LazyRelation.NoLazy);
            });

            ManyToOne(x => x.Professor, map =>
            {
                map.Column("IdProfessor");
                map.Cascade(Cascade.All);
                map.Lazy(LazyRelation.NoLazy);
            });
        }
    }
}
