using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Flunt.Validations;

namespace Blue.Core.Eventos.Validacoes
{
    public static class ValidacaoCustomizada
    {
        public static Contract IsDigitCustomIfNotNullOrEmpty(this Contract contrato, string valor, string propriedade, string mensagem)
        {
            if (!String.IsNullOrEmpty(valor))
            {
                if (!decimal.TryParse(valor, out var _))
                    contrato.AddNotification(propriedade, mensagem);
            }
            return contrato;
        }

        public static Contract IsListDigitCustom(this Contract contrato, List<string> valores, string propriedade, string mensagem)
        {
            // Verificar apenas se o valor não for vazio
            if (valores.Any(x => !decimal.TryParse(x, out var _)))
                contrato.AddNotification(propriedade, mensagem);

            return contrato;
        }

        /// <summary>
        /// Verifica de acorto com o tipo de processamento, caso seja de cancelamento a propiedade em questão não é obrigatoria, caso contrario sim.
        /// </summary>
        /// <param name="contrato"></param>
        /// <param name="processType"></param>
        /// <param name="propriedade"></param>
        /// <param name="mensagem"></param>
        /// <returns></returns>
        public static Contract IsNotNullDateIfNoProcessTypeCancel(this Contract contrato,  string processType, DateTime? valor, string propriedade, string mensagem)
        {
            if(processType != "C" && valor == null)
                contrato.AddNotification(propriedade, mensagem);

            return contrato;
        }
    }
}
