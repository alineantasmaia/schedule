using Serilog.Core;
using Serilog.Events;

namespace Blue.Core.Logs.Skins
{
    internal class RemoverPropriedadesPadrao : ILogEventEnricher
    {
        public void Enrich(LogEvent le, ILogEventPropertyFactory propertyFactory)
        {
            le.RemovePropertyIfPresent("SourceContext");
            le.RemovePropertyIfPresent("ActionId");
            le.RemovePropertyIfPresent("ActionName");
            le.RemovePropertyIfPresent("RequestId");
            le.RemovePropertyIfPresent("RequestPath");
            le.RemovePropertyIfPresent("CorrelationId");
            le.RemovePropertyIfPresent("ConnectionId");
        }
    }
}