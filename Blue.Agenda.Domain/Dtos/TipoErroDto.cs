using Blue.Core.Logs.Notificacoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blue.Agenda.Domain.Dtos
{
    public class TipoErroDto<T> where T : Evento
    {
        public TipoErroDto(string constante, string descricao, TipoNotificacao tipoNotificacao, T evento)
        {
            ConstruirGenerico(constante, descricao, tipoNotificacao, evento);
        }

        public TipoErroDto(string constante, string descricao, TipoNotificacao tipoNotificacao, T evento, bool erroConstanteGenerica)
        {
            ErroConstanteGenerica = erroConstanteGenerica;
            ConstruirGenerico(constante, descricao, tipoNotificacao, evento);
        }

        private void ConstruirGenerico(string constante, string descricao, TipoNotificacao tipoNotificacao, T evento)
        {
            Constante = constante;
            Descricao = descricao;
            TipoNotificacao = tipoNotificacao;
            if (evento != null)
                AddCampos(evento);
            else
                Campos = new List<(string Campo, object Valor)>();
        }

        public TipoErroDto(string constante,
            TipoNotificacao tipoNotificacao,
            string valorEncontrado,
            string valorEsperado)
        {
            Constante = constante;
            TipoNotificacao = tipoNotificacao;
            ValorEncontrado = valorEncontrado;
            ValorEsperado = valorEsperado;
        }

        public string Constante { get; private set; }
        public string Descricao { get; set; }
        public TipoNotificacao TipoNotificacao { get; private set; }
        public bool ErroIncosistencia => ValorEncontrado != null && ValorEsperado != null;
        public bool ErroConstante => !string.IsNullOrEmpty(Constante);
        public bool ErroConstanteGenerica { get; private set; }

        public string ValorEsperado { get; set; }
        public string ValorEncontrado { get; set; }
        public List<(string Campo, object Valor)> Campos { get; set; }

        public List<(string Campo, object Valor)> PegarTodasPropriedades(Type tipo, object objeto)
        {
            var propriedades = new List<(string Campo, object Valor)>();
            if (objeto == null) return propriedades;

            var campos = tipo.GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance)
                .ToList();
            var propriedadesIgnoradas = new string[] { "Notificacoes" };
            campos.ForEach((campo) =>
            {
                if (campo.PropertyType.GetInterfaces().Any(x => x.Name == "IList") && !propriedadesIgnoradas.Any(p => p == campo.Name))
                {
                    List<object> valores =
                        new List<dynamic>(
                            (IEnumerable<dynamic>)objeto.GetType().GetProperty(campo.Name)?.GetValue(objeto));
                    foreach (var valor in valores)
                    {
                        propriedades.AddRange(PegarTodasPropriedades(valor.GetType(), valor));
                    }
                }
                else if (campo.PropertyType.FullName != null &&
                         campo.PropertyType.FullName.Contains("Eventos.Eventos"))
                    propriedades.AddRange(PegarTodasPropriedades(campo.PropertyType,
                        objeto.GetType().GetProperty(campo.Name)?.GetValue(objeto)));
                else
                    propriedades.Add((Campo: campo.Name,
                        Valor: campo.GetValue(objeto)));
            });

            return propriedades;
        }

        private void AddCampos(T evento)
        {
            Campos = PegarTodasPropriedades(evento.GetType(), evento);
        }
    }
}
