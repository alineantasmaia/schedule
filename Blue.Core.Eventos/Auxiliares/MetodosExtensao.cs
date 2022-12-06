using System;

namespace Blue.Core.Eventos.Auxiliares
{
    public static class MetodosExtensao
    {
        /// <summary>
        /// Converter data para o formato UTC com timeZone Brasil
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Data Utc formatada</returns>
        public static DateTime ConverterDataUtc(DateTime data)
        {
            return TimeZoneInfo.ConvertTime(new DateTime(data.Year, data.Month, data.Day, data.Hour, data.Minute, 00).AddMilliseconds(00012), TimeZoneInfo.Local);
        }

        public static decimal ToDecimal(this string texto) =>
            !string.IsNullOrEmpty(texto) ? Convert.ToDecimal(texto) : 0;

        /// <summary>
        /// Converter data para o formato UTC com timeZone Brasil
        /// </summary>
        /// <param name="data"></param>
        /// <returns>Data Utc formatada</returns>
        public static DateTime ToDataUtc(this DateTime data)
        {
            return TimeZoneInfo.ConvertTime(new DateTime(data.Year, data.Month, data.Day, data.Hour, data.Minute, 00).AddMilliseconds(00012), TimeZoneInfo.Local);
        }
    }
}
