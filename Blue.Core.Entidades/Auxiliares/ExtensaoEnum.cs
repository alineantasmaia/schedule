using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Blue.Core.Entidades.Auxiliares
{
    public static class ExtensaoEnum
    {
        public static string ObterDescricao(this Enum item)
        {
            var tipo = item.GetType();
            var campo = tipo.GetField(item.ToString());
            var atributos = campo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            return atributos?.Length > 0
                ? atributos[0].Description
                : string.Empty;
        }

        public static T ObterValorApartirDescricao<T>(string descricao)
        {
            var tipo = typeof(T);
            if (!tipo.IsEnum)
                throw new InvalidOperationException();

            foreach (var campo in tipo.GetFields())
                if (Attribute.GetCustomAttribute(campo,
                    typeof(DescriptionAttribute)) is DescriptionAttribute atributo)
                {
                    if (atributo.Description.Equals(descricao, StringComparison.OrdinalIgnoreCase))
                        return (T)campo.GetValue(null);
                }
                else
                {
                    if (campo.Name.Equals(descricao, StringComparison.OrdinalIgnoreCase))
                        return (T)campo.GetValue(null);
                }

            throw new ArgumentException("Descrição não encontrada", nameof(descricao));
        }
    }
}
