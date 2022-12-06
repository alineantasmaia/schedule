using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using System;
using System.IO;
using System.Text;

namespace Blue.Core.Logs.Skins
{
    /// <summary>
    /// Classe responsavel por emitir o Log em Stdout quando não publicado no Elastic
    /// </summary>
    internal class ConsoleSkin : ILogEventSink
    {
        private readonly ITextFormatter _formatter;
        
        /// <summary>
        /// Construtor da classe
        /// </summary>
        /// <param name="formatter"> Formato de serializacao no stdout</param>
        public ConsoleSkin(ITextFormatter formatter)
        {
            _formatter = formatter;
        }

        /// <summary>
        /// Método chamado (Evento de ILogEventSink) quando a classe é instanciada
        /// </summary>
        /// <param name="logEvent"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Emit(LogEvent logEvent)
        {
            if (_formatter == null)
                throw new ArgumentNullException(nameof(_formatter));

            StringWriter stringWriter = new StringWriter(new StringBuilder());
            _formatter.Format(logEvent, stringWriter);

            Console.WriteLine(stringWriter);
        }
    }
}