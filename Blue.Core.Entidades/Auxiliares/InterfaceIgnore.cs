using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Entidades.Auxiliares
{
    /// <summary>
    /// Toda propriedade que estiver com esse atributo IgnoreInterface não serão enviadas no evento publicado
    /// </summary>
    public class InterfaceIgnore : Attribute
    {
    }
}
