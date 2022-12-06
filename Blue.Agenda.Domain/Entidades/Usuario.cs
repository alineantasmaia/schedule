using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blue.Core.Repositorios.Interfaces;

namespace Blue.Agenda.Domain.Entidades
{
    [Description("Armazenamento de dados da agenda do usuário.")]
    [Table("AGENDATBL", Schema = "SYS")]
    public class Usuario : EntidadeBase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public override decimal Id { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("fone")]
        public string? Fone { get; set; }
    }
}