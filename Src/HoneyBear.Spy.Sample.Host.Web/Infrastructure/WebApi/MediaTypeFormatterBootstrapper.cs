using System.Net.Http.Formatting;
using Newtonsoft.Json.Converters;

namespace HoneyBear.Spy.Sample.Host.Web.Infrastructure.WebApi
{
    internal class MediaTypeFormatterBootstrapper : IBootstrapper
    {
        private readonly MediaTypeFormatterCollection _formatters;

        public MediaTypeFormatterBootstrapper(
            MediaTypeFormatterCollection formatters)
        {
            _formatters = formatters;
        }

        public void Init()
        {
            _formatters.Clear();

            var formatter = new JsonMediaTypeFormatter();

            formatter.SerializerSettings.Converters.Add(new IsoDateTimeConverter());
            formatter.SerializerSettings.Converters.Add(new StringEnumConverter());

            _formatters.Add(formatter);

            _formatters.Add(new JsonMediaTypeFormatter());
        }
    }
}