using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blue.Agenda.Domain.Interfaces.Servicos
{
    public interface IServicoProvedorHttpClient
    {
        Task<object> EnviarApi<T>(string uri, T parametros, string uriApi);
    }
}
