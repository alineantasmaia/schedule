using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Blue.Core.Entidades.Auxiliares
{
    public class ConfiguracaoSerializacao : DefaultContractResolver
    {
        protected readonly Dictionary<Type, HashSet<string>> Ignoradas;

        public ConfiguracaoSerializacao()
        {
            this.Ignoradas = new Dictionary<Type, HashSet<string>>();
        }

        public void Ignore(Type type, params string[] nomesPropriedades)
        {
            if (!this.Ignoradas.ContainsKey(type)) this.Ignoradas[type] = new HashSet<string>();

            foreach (var propriedade in nomesPropriedades)
            {
                this.Ignoradas[type].Add(propriedade);
            }
        }

        public bool Ignorada(Type type, string nomePropriedade)
        {
            if (!this.Ignoradas.ContainsKey(type)) return false;

            if (this.Ignoradas[type].Count == 0) return true;

            return this.Ignoradas[type].Contains(nomePropriedade);
        }

        public bool PodeIgnorarPorAtributo(Type type, string nomePropriedade)
        {
            var propriedade = type.GetProperties().FirstOrDefault(p => p.Name.ToLower() == nomePropriedade.ToLower());

            if (propriedade == null)
                return false;

            if (Attribute.GetCustomAttribute(propriedade, typeof(InterfaceIgnore)) is InterfaceIgnore ignoreInterface)
                return true;

            return false;
        }


        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            return type.GetProperties()
                    .Select(p => {
                        
                        var propriedade = base.CreateProperty(p, memberSerialization);

                        if (this.Ignorada(propriedade.DeclaringType, propriedade.PropertyName) 
                           || this.Ignorada(propriedade.DeclaringType.BaseType, propriedade.PropertyName) 
                           || this.PodeIgnorarPorAtributo(propriedade.DeclaringType, propriedade.PropertyName))
                        {
                            propriedade.ShouldSerialize = instance => { return false; };

                            return propriedade;
                        }

                        propriedade.ValueProvider = new ConverterValor(p);

                        return propriedade;

                    }).ToList();
        }
    }

    public class ConverterValor : IValueProvider
    {
        PropertyInfo _MemberInfo;
        public ConverterValor(PropertyInfo memberInfo)
        {
            _MemberInfo = memberInfo;
        }

        public object GetValue(object target)
        {
            object result = _MemberInfo.GetValue(target);

            if (_MemberInfo.PropertyType == typeof(string) && string.IsNullOrEmpty((string)result))
                result = null;

            // TODO: Setar lista para null caso a lista esteja instanciada mas sem itens
            if (result != null && result.GetType().IsGenericType && result.GetType().GetGenericTypeDefinition() == typeof(List<>))
            {
                var metodo = GetType().GetMethod("VerificarListaVazia");

                var listaVazia = (bool) metodo.MakeGenericMethod(result.GetType().GetGenericArguments().Single()).Invoke(this, new object[] { result });
                
                if (listaVazia)
                    result = null;
            }

            return result;

        }

        public void SetValue(object target, object value)
        {
            _MemberInfo.SetValue(target, value);
        }

        public bool VerificarListaVazia<T>(object valor)
        {
            var lista = (List<T>)valor;

            return !lista.Any();
        }
    }
}
