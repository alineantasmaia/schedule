using Blue.Core.Entidades.Dtos;
using Blue.Core.Eventos.Auxiliares;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Threading.Tasks;

namespace Blue.Core.Eventos.Interfaces
{
    public interface IServicoEnvioEvento : IDisposable
    {
        /// <summary>
        /// Método responsável por receber qualquer classe que herde de InterfaceCabecalhoDto e publicar a mensagem no virtual host do HOST, para que a mensagem seja processada pelos consumidores
        /// do HOST, essa rotina publica a mensagem na exchange direct padrão do RabbitMq (amq.direct)
        /// </summary>
        /// <param name="evento">Corpo do evento que será publicado</param>
        void PublicarHost<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento;

        /// <summary>
        /// Método responsável por receber qualquer classe que herde de InterfaceCabecalhoDto e publicar a mensagem no virtual host do TOS, para que a mensagem seja processada pelos consumidores
        /// do TOS, essa rotina publica a mensagem na exchange direct padrão do RabbitMq (amq.direct)
        /// </summary>
        /// <param name="evento">Corpo do evento que será publicado</param>
        void PublicarTos<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento;

        /// <summary>
        /// Método responsável por receber qualquer classe que herde de InterfaceCabecalhoDto e publicar a mensagem, essa rotina está utilizando a exchange do tipo FanOut, através do bind a mesma mensagem é publicada nas duas filas (TOS e HOST)
        /// </summary>
        /// <typeparam name="T">Body do evento que será publicado</typeparam>
        /// <param name="evento">Evento que será publicado</param>
        void PublicarTosHost<T>(InterfaceCabecalhoDto<T> evento) where T : CorpoEvento;

        /// <summary>
        /// Método responsável por receber qualquer classe que herde de CorpoEvento e publicar a mensagem, essa rotina está utilizando a exchange do tipo FanOut, através do bind a mesma mensagem é publicada nas duas filas (TOS e HOST)
        /// </summary>
        /// <typeparam name="T">Body do evento que será publicado</typeparam>
        /// <param name="evento">Evento que será publicado</param>
        void PublicarTosHost<T>(T evento) where T : CorpoEvento;

        /// <summary>
        /// Método responsável por substituir o atual canal de comunicação com o RabbitMQ por um canal já aberto
        /// </summary>
        /// <param name="canal">Canal de comunicação já aberto</param>
        void CanalComunicacao(IModel canal);
    }
}
