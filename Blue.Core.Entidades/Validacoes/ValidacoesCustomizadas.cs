using System;
using Flunt.Validations;
using System.Linq;
using System.Runtime.InteropServices;

namespace Blue.Core.Entidades.Validacoes
{
    public static class ValidacoesCustomizadas
    {
        public static Contract IsDigitCustom(this Contract contrato, string valor, string propriedade, string mensagem)
        {
            if (!string.IsNullOrEmpty(valor) && !decimal.TryParse(valor, out var numeroDecimal))
                contrato.AddNotification(propriedade, mensagem);

            return contrato;
        }

        public static Contract AnyFieldHasValue(this Contract contrato, object obj)
        {
            if(AlgumCampoPreenchido(obj))
                contrato.AddNotification(obj.ToString(), "Pelo menos um campo é obrigatório");

            return contrato;
        }

        public static Contract AreNotEqualsIsNotIsNullOrEmpty(
            this Contract contrato,
            string val,
            string text,
            string property,
            string message,
            StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
        {
            if (!string.IsNullOrEmpty(val) && val.Equals(text, comparisonType))
                contrato.AddNotification(property, message);

            return contrato;
        }

        public static bool AlgumCampoPreenchido(object obj)
        {
            if (object.ReferenceEquals(obj, null))
                return true;
            var ignorar = new string[4]{ "Notifications", "Valid", "Invalid", "_notifications" };
            return obj.GetType().GetProperties().Where(c => !ignorar.Contains(c.Name))
                .Any(x => ValidarCampo(x.GetValue(obj)));
        }

        private static bool ValidarCampo(object value) =>
            !object.ReferenceEquals(value, null) && !object.ReferenceEquals(value, string.Empty);
        
        public static string ValidarNaoZero(string valor) => valor == "0" ? default : valor;
        public static decimal? ValidarNaoZero(decimal? valor) => valor == 0 ? default : valor;
    }
}
