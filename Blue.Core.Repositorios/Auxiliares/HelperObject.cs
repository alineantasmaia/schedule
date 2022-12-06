using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Blue.Core.Repositorios.Auxiliares
{
    public static class HelperObject
    {
        public static PropertyInfo ObterInfoPropriedade<TEntidade, TPropriedade>(TEntidade entidade, Expression<Func<TEntidade, TPropriedade>> propriedadeLambda)
        {
            Type tipo = typeof(TEntidade);

            MemberExpression member = propriedadeLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException($"Expression '{propriedadeLambda}' é um método e não propridade.");

            PropertyInfo propInfo = member.Member as PropertyInfo;

            if (propInfo == null)
                throw new ArgumentException($"Expression '{propriedadeLambda}' é referente a um campo e não a uma propriedade.");

            if (tipo != propInfo.ReflectedType && !tipo.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException($"Expression '{propriedadeLambda}' propriedade não existe no tipo {tipo}.");

            return propInfo;
        }
    }
}
