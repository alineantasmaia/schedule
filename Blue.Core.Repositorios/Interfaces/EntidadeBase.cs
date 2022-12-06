using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Blue.Core.Repositorios.Interfaces
{
    /// <summary>
    /// Classe de extensão para mapeamento do Entity
    /// </summary>
    public class EntidadeBase
    {
        /// <summary>
        /// Propriedade que deve ser sobreescrita no mapeamento das entidades
        /// </summary>
        public virtual decimal Id { get; set; }

        /// <summary>
        /// Lista de notificações geradas pelo método Valido
        /// </summary>        
        [NotMapped]
        public List<string> Notificacoes { get; set; }

        /// <summary>
        /// Método genérico para validar as entidades
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="validacao"></param>
        /// <param name="entidade"></param>
        /// <returns></returns>
        public bool Valido<T, T2>(T validacao, T2 entidade) where T : AbstractValidator<T2> where T2 : EntidadeBase
        {
            var validador = validacao.Validate(entidade);

            if (validador.IsValid) return true;

            Notificacoes = validador.Errors.Select(p => p.ErrorMessage).ToList();

            return false;
        }
    }
}
