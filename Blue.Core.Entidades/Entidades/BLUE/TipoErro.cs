using Blue.Core.Repositorios.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Blue.Core.Entidades.Entidades.BLUE
{
    [Description("Registra as respectivas descrições dos principais erros de interface mapeados")]
    [Table("TIPO_ERRO", Schema = "LOCALHOST")]
    public class TipoErro : EntidadeBase
    {
        public TipoErro() { }

        public TipoErro(decimal tierId,
            string tierDescricao,
            string tierConstante,
            string tierMensagemPadrao)
        {
            Id = tierId;
            TierDescricao = tierDescricao;
            TierConstante = tierConstante;
            TierMensagemPadrao = tierMensagemPadrao;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("ID")]
        public override decimal Id { get; set; }
        [Column("DESCRICAO")]
        public string TierDescricao { get; private set; }
        [Column("CONSTANTE")]
        public string TierConstante { get; private set; }
        [Column("MENSAGEM_PADRAO")]
        public string TierMensagemPadrao { get; private set; }
    }
}
