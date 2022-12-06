using System;
using System.Collections.Generic;
using System.Text;

namespace Blue.Core.Logs.Excecoes
{
    public static class Excecao
    {
        public static string ObterMensagemTratada(Exception exception)
        {
            if (exception.GetBaseException() is ExcecaoNegocio)
                return exception.Message;

            return $"Message: {exception.Message} - \nStackTracke: {exception.StackTrace}";
        }

        public static string ObterMensagemUnificada(Exception exception)
        {
            var stringBuilder = new StringBuilder();

            while (exception != null)
            {
                stringBuilder.AppendLine(exception.Message);
                stringBuilder.AppendLine(exception.StackTrace);

                exception = exception.InnerException;
            }

            return stringBuilder.ToString();
        }

        public static List<string> TratarExcecao(this Exception excecao)
        {
            var mensagens = new List<string>();

            while (excecao != null)
            {
                mensagens.Add(excecao.Message);
                mensagens.Add($"[DETALHES] {excecao.StackTrace}");

                excecao = excecao.InnerException;
            }

            return mensagens;
        }


    }
}
