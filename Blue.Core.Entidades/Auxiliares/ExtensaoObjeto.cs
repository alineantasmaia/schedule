using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blue.Core.Entidades.Auxiliares
{
    public static class ExtensaoObjeto
    {
        public static T Clonar<T>(this T origem)
        {
            var novoObjeto = (T)Activator.CreateInstance(origem.GetType());

            foreach (var originalProp in origem.GetType().GetProperties())
            {
                if (originalProp.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0)
                    continue;

                originalProp.SetValue(novoObjeto, originalProp.GetValue(origem));
            }

            return novoObjeto;
        }

        public static decimal ValorDecimal(this bool valor) => valor ? 1 : 0;
    }
}
